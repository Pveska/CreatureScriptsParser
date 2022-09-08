using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using static CreatureScriptsParser.Packets;
using static CreatureScriptsParser.Utils;

namespace CreatureScriptsParser
{
    public class Creator
    {
        private MainForm mainForm;
        private List<object> packetsList;
        public Creator(MainForm mainForm, List<object> packets)
        { this.mainForm = mainForm; packetsList = packets; }

        public void CreateScriptsForCreatureWithGuid(string guid)
        {
            if (guid == "")
                return;

            List<object> creaturePacketsList = packetsList.Where(x => ((Packet)x).guid == guid).ToList();

            foreach (UpdateObjectPacket updatePacket in packetsList.Where(x => typeof(UpdateObjectPacket) == x.GetType() && ((UpdateObjectPacket)x).objectType == UpdateObjectPacket.ObjectType.Conversation))
            {
                if (updatePacket.conversationActors.FirstOrDefault(x => x == guid) != null)
                {
                    creaturePacketsList.Add(updatePacket);
                }
            }

            creaturePacketsList.AddRange(packetsList.Where(x => x.GetType() == typeof(SpellStartPacket) && ((SpellStartPacket)x).guid != guid && ((SpellStartPacket)x).targetGuids != null && ((SpellStartPacket)x).targetGuids.Contains(guid)));
            AuraUpdatePacket.FilterAuraPacketsForCreature(creaturePacketsList);
            creaturePacketsList = creaturePacketsList.OrderBy(x => ((Packet)x).number).ToList();

            for (int i = 0; i < creaturePacketsList.Count; i++)
            {
                Packet packet = (Packet)creaturePacketsList[i];

                if (packet.type == Packet.PacketTypes.SMSG_EMOTE)
                {
                    bool emoteRelatedToChat = false;

                    foreach (Packet chatPacket in creaturePacketsList.Where(x => x.GetType() == typeof(ChatPacket)))
                    {
                        if ((Math.Round(packet.time.TotalSeconds) == Math.Round(chatPacket.time.TotalSeconds) || Math.Round(packet.time.TotalSeconds) + 1 == Math.Round(chatPacket.time.TotalSeconds)) &&
                            IsEmoteRelatedToText(((ChatPacket)chatPacket).creatureText, ((EmotePacket)packet).emoteId))
                        {
                            emoteRelatedToChat = true;
                            break;
                        }
                    }

                    if (emoteRelatedToChat)
                        creaturePacketsList.RemoveAt(i);
                }
            }

            uint creatureEntry = GetCreatureEntryUsingGuid(packetsList, guid);
            string creatureName = GetCreatureNameFromDb(creatureEntry);
            string output = "";

            output += "Parsed packet sequence for " + creatureName + " (Entry: " + creatureEntry + ") " + "(Guid: " + guid + ")" + "\r\n\r\n";
            output += "/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*" + "\r\n";
            output += "/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*" + "\r\n";

            for (int i = 0; i < creaturePacketsList.Count; i++)
            {
                Packet packet = (Packet)creaturePacketsList[i];

                if (i > 0)
                {
                    output += "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + "\r\n";
                    output += "Time passed after previous packet: " + (packet.time - ((Packet)creaturePacketsList[i - 1]).time).TotalSeconds + " -- " + "Time passed after first packet: " + (packet.time - ((Packet)creaturePacketsList[0]).time).TotalSeconds + "\r\n";
                }

                output += "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + "\r\n";
                output += "Packet Type: " + packet.type.ToString() + " -- " + "Packet Number: " + packet.number + "\r\n";
                output += "Packet Time: " + packet.time.Hours + ":" + packet.time.Minutes + ":" + packet.time.Seconds + ":" + packet.time.Milliseconds + "\r\n";

                switch (packet.type)
                {
                    case Packet.PacketTypes.SMSG_UPDATE_OBJECT:
                    {
                        UpdateObjectPacket updateObjectPacket = (UpdateObjectPacket)packet;

                        if (updateObjectPacket.updateType == UpdateObjectPacket.UpdateType.CreateObject)
                        {
                            if (updateObjectPacket.objectType == UpdateObjectPacket.ObjectType.Creature)
                            {
                                output += "Spawn Position: " + updateObjectPacket.spawnPosition.ToString() + "\r\n";
                                output += "Position const g_" + ConverNameToCoreFormat(creatureName) + "SpawnPosition = { " + updateObjectPacket.spawnPosition.ToString() + " };" + "\r\n";

                                if (updateObjectPacket.isSummonedByPlayer)
                                {
                                    if (updateObjectPacket.hasReplacedObject)
                                    {
                                        output += "if (Creature* l_" + ConverNameToCoreFormat(creatureName) + " = " + "l_Player->SummonCreature(me->GetEntry(), me->GetPosition(), TempSummonType::TEMPSUMMON_TIMED_DESPAWN, " + GetDespawnTimerForCreatureWithGuid(guid) * 1000 + ", 0, " + "l_Player->GetObjectGuid()))" + "\r\n" + "{" + "\r\n" + AddSpacesCount(4) + "\r\n" + "}" + "\r\n";
                                    }
                                    else
                                    {
                                        output += ConverNameToCoreFormat(creatureName) + " = " + creatureEntry + "\r\n";
                                        output += "if (Creature* l_" + ConverNameToCoreFormat(creatureName) + " = " + "l_Player->SummonCreature(eCreatures::" + ConverNameToCoreFormat(creatureName) + ", " + "Positions::g_" + ConverNameToCoreFormat(creatureName) + ", TempSummonType::TEMPSUMMON_TIMED_DESPAWN, " + GetDespawnTimerForCreatureWithGuid(guid) * 1000 + ", 0, " + "l_Player->GetObjectGuid()))" + "\r\n" + "{" + "\r\n" + AddSpacesCount(4) + "\r\n" + "}" + "\r\n";
                                    }
                                }

                                if (updateObjectPacket.moveData.waypoints.Count() != 0)
                                {
                                    output += "Move Time: " + updateObjectPacket.moveData.moveTime + "\r\n";

                                    if (!updateObjectPacket.moveData.HasJump())
                                    {
                                        output += "Velocity: " + Convert.ToString(updateObjectPacket.moveData.GetWaypointsVelocity()).Replace(",", ".") + "f" + "\r\n";
                                        output += updateObjectPacket.moveData.GetSetSpeedString() + "\r\n";

                                        if (updateObjectPacket.moveData.waypoints.Count() == 1)
                                        {
                                            switch (updateObjectPacket.moveData.moveType)
                                            {
                                                case MonsterMovePacket.MoveTypes.WALK:
                                                {
                                                    output += "Position const g_" + ConverNameToCoreFormat(creatureName) + "MovePos = { " + updateObjectPacket.moveData.waypoints.First().ToString() + " };" + "\r\n";
                                                    output += "me->GetMotionMaster()->MoveSmoothPath(ePoints::MoveEnd, Positions::g_" + ConverNameToCoreFormat(creatureName) + "MovePos" + ");" + "\r\n";
                                                    break;
                                                }
                                                case MonsterMovePacket.MoveTypes.RUN:
                                                {
                                                    output += "Position const g_" + ConverNameToCoreFormat(creatureName) + "MovePos = { " + updateObjectPacket.moveData.waypoints.First().ToString() + " };" + "\r\n";
                                                    output += "me->GetMotionMaster()->MoveSmoothPath(ePoints::MoveEnd, Positions::g_" + ConverNameToCoreFormat(creatureName) + "MovePos" + ");" + "\r\n";
                                                    break;
                                                }
                                                case MonsterMovePacket.MoveTypes.FLY:
                                                {
                                                    output += "Position const g_" + ConverNameToCoreFormat(creatureName) + "FlyPos = { " + updateObjectPacket.moveData.waypoints.First().ToString() + " };" + "\r\n";
                                                    output += "me->GetMotionMaster()->MoveSmoothFlyPath(ePoints::FlyEnd, Positions::g_" + ConverNameToCoreFormat(creatureName) + "FlyPos" + ");" + "\r\n";
                                                    break;
                                                }
                                                default:
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            switch (updateObjectPacket.moveData.moveType)
                                            {
                                                case MonsterMovePacket.MoveTypes.WALK:
                                                {
                                                    output += "me->GetMotionMaster()->MoveSmoothPath(ePoints::MoveEnd, Waypoints::g_Path" + ConverNameToCoreFormat(creatureName) + ", true);" + "\r\n";
                                                    break;
                                                }
                                                case MonsterMovePacket.MoveTypes.RUN:
                                                {
                                                    output += "me->GetMotionMaster()->MoveSmoothPath(ePoints::MoveEnd, Waypoints::g_Path" + ConverNameToCoreFormat(creatureName) + ", false);" + "\r\n";
                                                    break;
                                                }
                                                case MonsterMovePacket.MoveTypes.FLY:
                                                {
                                                    output += "me->GetMotionMaster()->MoveSmoothFlyPath(ePoints::MoveEnd, Waypoints::g_Path" + ConverNameToCoreFormat(creatureName) + ");" + "\r\n";
                                                    break;
                                                }
                                                default:
                                                    break;
                                            }

                                            output += "std::vector<G3D::Vector3> const g_Path" + ConverNameToCoreFormat(creatureName) + " =" + "\r\n";
                                            output += "{" + "\r\n";

                                            for (int j = 0; j < updateObjectPacket.moveData.waypoints.Count; j++)
                                            {
                                                Position waypoint = updateObjectPacket.moveData.waypoints[j];

                                                if (j < (updateObjectPacket.moveData.waypoints.Count - 1))
                                                {
                                                    output += "{ " + waypoint.x.GetValueWithoutComma() + "f, " + waypoint.y.GetValueWithoutComma() + "f, " + waypoint.z.GetValueWithoutComma() + "f },\r\n";
                                                }
                                                else
                                                {
                                                    output += "{ " + waypoint.x.GetValueWithoutComma() + "f, " + waypoint.y.GetValueWithoutComma() + "f, " + waypoint.z.GetValueWithoutComma() + "f }\r\n";
                                                }
                                            }

                                            output += "};" + "\r\n";
                                        }
                                    }
                                    else
                                    {
                                        output += "Position const g_" + ConverNameToCoreFormat(creatureName) + "JumpPos = { " + updateObjectPacket.moveData.jumpInfo.jumpPos.ToString() + " };" + "\r\n";
                                        output += "me->GetMotionMaster()->MoveReverseJump(g_" + ConverNameToCoreFormat(creatureName) + "JumpPos" + ", " + updateObjectPacket.moveData.moveTime + ", " + updateObjectPacket.moveData.jumpInfo.jumpGravity.GetValueWithoutComma() + "f, " + "ePoints::" + ConverNameToCoreFormat(creatureName) + "JumpEnd);" + "\r\n";
                                        break;
                                    }

                                    break;
                                }
                            }
                            else if (updateObjectPacket.objectType == Packets.UpdateObjectPacket.ObjectType.Conversation)
                            {
                                output += "Creature is part of conversation: " + updateObjectPacket.conversationEntry + " (" + updateObjectPacket.guid + ")" + "\r\n";
                            }
                        }
                        else if (updateObjectPacket.updateType == UpdateObjectPacket.UpdateType.Values)
                        {
                            if (updateObjectPacket.sheatheState != null)
                            {
                                output += "me->SetSheath(" + (UpdateObjectPacket.SheathState)updateObjectPacket.sheatheState + ");" + "\r\n";
                            }

                            if (updateObjectPacket.standState != null)
                            {
                                output += "me->SetStandState(" + (UpdateObjectPacket.UnitStandStateType)updateObjectPacket.standState + ");" + "\r\n";
                            }

                            if (updateObjectPacket.emoteStateId != null)
                            {
                                output += "me->SetEmoteState(" + (EmotePacket.Emote)updateObjectPacket.emoteStateId + ");" + "\r\n";
                            }

                            if (updateObjectPacket.unitFlags != null)
                            {
                                output += $"me->SetUnitFlags({BuildUnitFlagNames(updateObjectPacket.unitFlags)});" + "\r\n";
                            }

                            if (updateObjectPacket.unitFlags2 != null)
                            {
                                output += $"me->SetUnitFlags2({BuildUnitFlag2Names(updateObjectPacket.unitFlags2)});" + "\r\n";
                            }

                            if (updateObjectPacket.unitFlags3 != null)
                            {
                                output += $"me->SetUnitFlags3({BuildUnitFlag3Names(updateObjectPacket.unitFlags3)});" + "\r\n";
                            }

                            if (updateObjectPacket.factionTemplate != 0)
                            {
                                output += $"me->setFaction({updateObjectPacket.factionTemplate});" + "\r\n";
                            }
                        }
                        else if (updateObjectPacket.updateType == UpdateObjectPacket.UpdateType.Destroy)
                        {
                            output += "me->DespawnOrUnsummon();" + "\r\n";
                        }

                        break;
                    }
                    case Packet.PacketTypes.SMSG_SPELL_START:
                    case Packet.PacketTypes.SMSG_SPELL_GO:
                    {
                        SpellStartPacket spellStartPacket = (SpellStartPacket)packet;

                        if (spellStartPacket.guid != guid)
                        {
                            uint entry = GetCreatureEntryUsingGuid(packetsList, spellStartPacket.guid);
                            string spellName = GetSpellName(spellStartPacket.spellId);

                            output += "Hitted by spell: " + spellStartPacket.spellId + " (" + spellName + ")" + "\r\n";

                            if (spellName != "Unknown")
                            {
                                output += ConverNameToCoreFormat(spellName) + " = " + spellStartPacket.spellId + "\r\n";
                            }

                            if (entry != 0)
                            {
                                output += "Caster: " + GetCreatureNameFromDb(entry) + " (Entry: " + entry + ") " + "(Guid: " + spellStartPacket.guid + ")" + "\r\n";
                            }
                            else
                            {
                                output += "Caster: Player " + "(Guid: " + spellStartPacket.guid + ")" + "\r\n";
                            }
                        }
                        else
                        {
                            string spellName = GetSpellName(spellStartPacket.spellId);

                            output += "Spell Id: " + spellStartPacket.spellId + " (" + spellName + ")" + "\r\n";

                            if (spellName != "Unknown")
                            {
                                output += ConverNameToCoreFormat(spellName) + " = " + spellStartPacket.spellId + "\r\n";
                                output += "me->CastSpell(me, eSpells::" + ConverNameToCoreFormat(spellName) + ", true);" + "\r\n";
                            }

                            if (spellStartPacket.destination.IsValid())
                            {
                                output += "Destination: " + spellStartPacket.destination.ToString() + "\r\n";
                            }

                            if (spellStartPacket.targetGuids != null)
                            {
                                if (spellStartPacket.targetGuids.Count == 1 && spellStartPacket.targetGuids.First() == spellStartPacket.guid)
                                    break;
                                else
                                {
                                    for (int j = 0; j < spellStartPacket.targetGuids.Count; j++)
                                    {
                                        uint entry = GetCreatureEntryUsingGuid(packetsList, spellStartPacket.targetGuids[j]);
                                        output += "Hit Target " + j + ": " + GetCreatureNameFromDb(entry) + " (Entry: " + entry + ") " + "(Guid: " + spellStartPacket.targetGuids[j] + ")" + "\r\n";
                                    }
                                }
                            }
                        }

                        break;
                    }
                    case Packet.PacketTypes.SMSG_ON_MONSTER_MOVE:
                    {
                        MonsterMovePacket monsterMovePacket = (MonsterMovePacket)packet;

                        if (monsterMovePacket.HasOrientation())
                        {
                            output += "me->SetFacingTo(" + monsterMovePacket.creatureOrientation.GetValueWithoutComma() + "f);" + "\r\n";
                        }

                        if (monsterMovePacket.hasFacingToPlayer)
                        {
                            output += "if (Unit* l_Owner = me->GetAnyOwner())" + "\r\n" + "{" + "\r\n" + AddSpacesCount(4) + "me->SetFacingToObject(l_Owner);" + "\r\n" + "}" + "\r\n";

                        }

                        if (monsterMovePacket.waypoints.Count() != 0)
                        {
                            output += "Move Time: " + monsterMovePacket.moveTime + "\r\n";

                            if (!monsterMovePacket.HasJump())
                            {
                                output += "Velocity: " + Convert.ToString(monsterMovePacket.GetWaypointsVelocity()).Replace(",", ".") + "f" + "\r\n";


                                output += monsterMovePacket.GetSetSpeedString() + "\r\n";

                                if (monsterMovePacket.splineFilter.filled)
                                {
                                    output += "\r\n";
                                    output += "Movement::MonsterSplineFilter l_SplineFilter;\r\n";
                                    output += $"l_SplineFilter.BaseSpeed = {monsterMovePacket.splineFilter.baseSpeed.Value.GetFloatValueInCoreFormat()};\r\n";
                                    output += $"l_SplineFilter.StartOffset = {monsterMovePacket.splineFilter.startOffset};\r\n";
                                    output += $"l_SplineFilter.DistToPrevFilterKey = {monsterMovePacket.splineFilter.distToPrevFilterKey.Value.GetFloatValueInCoreFormat()};\r\n";
                                    output += $"l_SplineFilter.AddedToStart = {monsterMovePacket.splineFilter.addedToStart};\r\n";
                                    output += $"l_SplineFilter.FilterFlags = {monsterMovePacket.splineFilter.filterFlags};\r\n\r\n";

                                    output += "l_SplineFilter.FilterKeys =\r\n";
                                    output += "{\r\n";

                                    foreach (MonsterMovePacket.MonsterSplineFilterKey splineKey in monsterMovePacket.splineFilter.filterKeys)
                                    {
                                        if (monsterMovePacket.splineFilter.filterKeys.IndexOf(splineKey) == monsterMovePacket.splineFilter.filterKeys.Count - 1)
                                        {
                                            output += $"{AddSpacesCount(4)}Movement::MonsterSplineFilterKey({splineKey.idx}, {splineKey.speed})\r\n";
                                        }
                                        else
                                        {
                                            output += $"{AddSpacesCount(4)}Movement::MonsterSplineFilterKey({splineKey.idx}, {splineKey.speed}),\r\n";
                                        }
                                    }

                                    output += "};\r\n\r\n";
                                }

                                if (monsterMovePacket.waypoints.Count() == 1)
                                {
                                    switch (monsterMovePacket.moveType)
                                    {
                                        case MonsterMovePacket.MoveTypes.WALK:
                                        {
                                            output += "Position const g_" + ConverNameToCoreFormat(creatureName) + "MovePos = { " + monsterMovePacket.waypoints.First().ToString() + " };" + "\r\n";
                                            output += "me->GetMotionMaster()->MoveSmoothPath(ePoints::MoveEnd, Positions::g_" + ConverNameToCoreFormat(creatureName) + "MovePos" + ");" + "\r\n";
                                            break;
                                        }
                                        case MonsterMovePacket.MoveTypes.RUN:
                                        {
                                            output += "Position const g_" + ConverNameToCoreFormat(creatureName) + "MovePos = { " + monsterMovePacket.waypoints.First().ToString() + " };" + "\r\n";
                                            output += "me->GetMotionMaster()->MoveSmoothPath(ePoints::MoveEnd, Positions::g_" + ConverNameToCoreFormat(creatureName) + "MovePos" + ");" + "\r\n";
                                            break;
                                        }
                                        case MonsterMovePacket.MoveTypes.FLY:
                                        {
                                            if (monsterMovePacket.tierTransitionId != 0)
                                            {
                                                if (monsterMovePacket.waypoints.First().x == monsterMovePacket.startPosition.x && monsterMovePacket.waypoints.First().y == monsterMovePacket.startPosition.y)
                                                {
                                                    float distance = monsterMovePacket.waypoints.First().z - monsterMovePacket.startPosition.z;
                                                    string distanceStr = distance.ToString().Length > 1 ? distance.GetValueWithoutComma() : distance.ToString() + ".0f";
                                                    output += $"me->GetMotionMaster()->MoveAnimTierTransition(ePoints::{ConverNameToCoreFormat(creatureName)}MoveUpEnd,  {distanceStr}, {monsterMovePacket.tierTransitionId});" + "\r\n";
                                                }
                                                else
                                                {
                                                    output += $"Position const g_{ConverNameToCoreFormat(creatureName)}MoveUpPos = {{ {monsterMovePacket.waypoints.First()} }};" + "\r\n";
                                                    output += $"me->GetMotionMaster()->MoveAnimTierTransition(ePoints::{ConverNameToCoreFormat(creatureName)}MoveUpEnd, g_{ConverNameToCoreFormat(creatureName)}MoveUpPos, {monsterMovePacket.tierTransitionId});" + "\r\n";
                                                }
                                            }
                                            else
                                            {
                                                output += "Position const g_" + ConverNameToCoreFormat(creatureName) + "FlyPos = { " + monsterMovePacket.waypoints.First().ToString() + " };" + "\r\n";
                                                output += "me->GetMotionMaster()->MoveSmoothFlyPath(ePoints::FlyEnd, Positions::g_" + ConverNameToCoreFormat(creatureName) + "FlyPos" + ");" + "\r\n";
                                            }

                                            break;
                                        }
                                        default:
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (monsterMovePacket.moveType)
                                    {
                                        case MonsterMovePacket.MoveTypes.WALK:
                                        {
                                            if (monsterMovePacket.splineFilter.filled)
                                            {
                                                output += $"me->GetMotionMaster()->MoveSmoothPath(ePoints::MoveEnd, Waypoints::g_Path{ConverNameToCoreFormat(creatureName)}, true, 0, $l_SplineFilter);\r\n";
                                            }
                                            else
                                            {
                                                output += "me->GetMotionMaster()->MoveSmoothPath(ePoints::MoveEnd, Waypoints::g_Path" + ConverNameToCoreFormat(creatureName) + ", true);" + "\r\n";
                                            }

                                            break;
                                        }
                                        case MonsterMovePacket.MoveTypes.RUN:
                                        {
                                            if (monsterMovePacket.splineFilter.filled)
                                            {
                                                output += $"me->GetMotionMaster()->MoveSmoothPath(ePoints::MoveEnd, Waypoints::g_Path{ConverNameToCoreFormat(creatureName)}, false, 0, $l_SplineFilter);\r\n";
                                            }
                                            else
                                            {
                                                output += "me->GetMotionMaster()->MoveSmoothPath(ePoints::MoveEnd, Waypoints::g_Path" + ConverNameToCoreFormat(creatureName) + ", false);" + "\r\n";
                                            }

                                            break;
                                        }
                                        case MonsterMovePacket.MoveTypes.FLY:
                                        {
                                            if (monsterMovePacket.splineFilter.filled)
                                            {
                                                output += $"me->GetMotionMaster()->MoveSmoothFlyPath(ePoints::MoveEnd, Waypoints::g_Path{ConverNameToCoreFormat(creatureName)}, 0, 0, $l_SplineFilter);\r\n";
                                            }
                                            else
                                            {
                                                output += "me->GetMotionMaster()->MoveSmoothFlyPath(ePoints::MoveEnd, Waypoints::g_Path" + ConverNameToCoreFormat(creatureName) + ");" + "\r\n";
                                            }

                                            break;
                                        }
                                        default:
                                            break;
                                    }

                                    output += "\r\n";
                                    output += "std::vector<G3D::Vector3> const g_Path" + ConverNameToCoreFormat(creatureName) + " =" + "\r\n";
                                    output += "{" + "\r\n";

                                    for (int j = 0; j < monsterMovePacket.waypoints.Count; j++)
                                    {
                                        Position waypoint = monsterMovePacket.waypoints[j];

                                        if (j < (monsterMovePacket.waypoints.Count - 1))
                                        {
                                            output += "{ " + waypoint.x.GetValueWithoutComma() + "f, " + waypoint.y.GetValueWithoutComma() + "f, " + waypoint.z.GetValueWithoutComma() + "f },\r\n";
                                        }
                                        else
                                        {
                                            output += "{ " + waypoint.x.GetValueWithoutComma() + "f, " + waypoint.y.GetValueWithoutComma() + "f, " + waypoint.z.GetValueWithoutComma() + "f }\r\n";
                                        }
                                    }

                                    output += "};" + "\r\n";
                                }
                            }
                            else
                            {
                                output += "Position const g_" + ConverNameToCoreFormat(creatureName) + "JumpPos = { " + monsterMovePacket.jumpInfo.jumpPos.ToString() + " };" + "\r\n";
                                output += "me->GetMotionMaster()->MoveReverseJump(g_" + ConverNameToCoreFormat(creatureName) + "JumpPos" + ", " + monsterMovePacket.moveTime + ", " + monsterMovePacket.jumpInfo.jumpGravity.GetValueWithoutComma() + "f, " + "ePoints::" + ConverNameToCoreFormat(creatureName) + "JumpEnd);" + "\r\n";
                                break;
                            }

                            break;
                        }

                        break;
                    }
                    case Packet.PacketTypes.SMSG_PLAY_ONE_SHOT_ANIM_KIT:
                    {
                        PlayOneShotAnimKit monsterMovePacket = (PlayOneShotAnimKit)packet;

                        output += "me->PlayOneShotAnimKitId(" + monsterMovePacket.AnimKitId + ");" + "\r\n";
                        break;
                    }
                    case Packet.PacketTypes.SMSG_CHAT:
                    {
                        ChatPacket chatPacket = (ChatPacket)packet;

                        output += "Text: " + chatPacket.creatureText + "\r\n";
                        break;
                    }
                    case Packet.PacketTypes.SMSG_EMOTE:
                    {
                        EmotePacket emotePacket = (EmotePacket)packet;
                        output += "me->HandleEmoteCommand(" + (EmotePacket.Emote)emotePacket.emoteId + ");" + "\r\n";
                        break;
                    }
                    case Packet.PacketTypes.SMSG_AURA_UPDATE:
                    {
                        AuraUpdatePacket auraPacket = (AuraUpdatePacket)packet;

                        string spellName = ConverNameToCoreFormat(GetSpellName(auraPacket.spellId));

                        output += spellName + " = " + auraPacket.spellId + "\r\n";
                        output += "me->RemoveAura(eSpells::" + spellName + ");" + "\r\n";
                        break;
                    }
                    case Packet.PacketTypes.SMSG_SET_AI_ANIM_KIT:
                    {
                        SetAiAnimKit animKitPacket = (SetAiAnimKit)packet;

                        output += "me->SetAIAnimKitId(" + animKitPacket.AiAnimKitId + ");" + "\r\n";
                        break;
                    }
                    case Packet.PacketTypes.SMSG_PLAY_SPELL_VISUAL_KIT:
                    {
                        PlaySpellVisualKit playSpellVisualKitPacket = (PlaySpellVisualKit)packet;

                        output += "me->SendPlaySpellVisualKit(" + playSpellVisualKitPacket.KitRecId + ", " + playSpellVisualKitPacket.KitType + ", " + playSpellVisualKitPacket.Duration +  ");" + "\r\n";
                        break;
                    }
                    case Packet.PacketTypes.SMSG_PLAY_OBJECT_SOUND:
                    {
                        PlayObjectSoundPacket playObjectSoundPacket = (PlayObjectSoundPacket)packet;
                        Position spawnPos = ((UpdateObjectPacket)packetsList.FirstOrDefault(x => ((Packet)x).type == Packet.PacketTypes.SMSG_UPDATE_OBJECT && ((Packet)x).guid == guid && ((UpdateObjectPacket)x).updateType == UpdateObjectPacket.UpdateType.CreateObject)).spawnPosition;
                        spawnPos.orientation = 0.0f;

                        if (spawnPos == playObjectSoundPacket.position)
                        {
                            output += $"me->PlayDistanceSound(me, {playObjectSoundPacket.SoundId}, me, me->GetPositionX(), me->GetPositionY(), me->GetPositionZ());" + "\r\n";
                        }
                        else
                        {
                            output += $"me->PlayDistanceSound(me, {playObjectSoundPacket.SoundId}, me, {playObjectSoundPacket.position.x.GetValueWithoutComma()}f, {playObjectSoundPacket.position.y.GetValueWithoutComma()}f, {playObjectSoundPacket.position.z.GetValueWithoutComma()}f);" + "\r\n";
                        }

                        break;
                    }
                    default:
                        break;
                }

                output += "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + "\r\n";
                output += "/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*" + "\r\n";
                output += "/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*" + "\r\n";
            }

            mainForm.textBox_Output.Text = output;
        }

        private uint GetDespawnTimerForCreatureWithGuid(string guid)
        {
            Packet spawnPacket = (Packet)packetsList.FirstOrDefault(x => x.GetType() == typeof(UpdateObjectPacket) && ((Packet)x).guid == guid && ((UpdateObjectPacket)x).updateType == UpdateObjectPacket.UpdateType.CreateObject);
            Packet despawnPacket = (Packet)packetsList.FirstOrDefault(x => x.GetType() == typeof(UpdateObjectPacket) && ((Packet)x).guid == guid && ((UpdateObjectPacket)x).updateType == UpdateObjectPacket.UpdateType.Destroy);

            if (spawnPacket != null && despawnPacket != null)
            {
                return (uint)(despawnPacket.time - spawnPacket.time).TotalSeconds;
            }

            return 0;
        }

        private string BuildUnitFlagNames(long? unitFlags)
        {
            string unitFlagNames = "";
            List<long> unitFlagsList = new List<long>();

            if (unitFlags != 0)
            {
                var flagsArray = Enum.GetValues(typeof(UpdateObjectPacket.UnitFlags));
                Array.Reverse(flagsArray);

                foreach (long flag in flagsArray)
                {
                    if (unitFlags - flag >= 0)
                    {
                        unitFlagsList.Add(flag);
                        unitFlags -= flag;
                    }
                }
            }
            else
                return unitFlagNames = "eUnitFlags(0)";

            if (unitFlagsList.Count > 1)
            {
                unitFlagNames += $"eUnitFlags(" + unitFlagsList.Aggregate(unitFlagNames, (current, itr) => current + ((UpdateObjectPacket.UnitFlags)itr + " | ")) + ")";
                return unitFlagNames.Replace(" | )", ")");
            }
            else
                return unitFlagNames = ((UpdateObjectPacket.UnitFlags)unitFlagsList.FirstOrDefault()).ToString();
        }

        private string BuildUnitFlag2Names(long? unitFlags)
        {
            string unitFlagNames = "";
            List<long> unitFlagsList = new List<long>();

            if (unitFlags != 0)
            {
                var flagsArray = Enum.GetValues(typeof(UpdateObjectPacket.UnitFlags2));
                Array.Reverse(flagsArray);

                foreach (long flag in flagsArray)
                {
                    if (unitFlags - flag >= 0)
                    {
                        unitFlagsList.Add(flag);
                        unitFlags -= flag;
                    }
                }
            }
            else
                return unitFlagNames = "eUnitFlags2(0)";

            if (unitFlagsList.Count > 1)
            {
                unitFlagNames += $"eUnitFlags2(" + unitFlagsList.Aggregate(unitFlagNames, (current, itr) => current + ((UpdateObjectPacket.UnitFlags2)itr + " | ")) + ")";
                return unitFlagNames.Replace(" | )", ")");
            }
            else
                return unitFlagNames = ((UpdateObjectPacket.UnitFlags2)unitFlagsList.FirstOrDefault()).ToString();
        }

        private string BuildUnitFlag3Names(long? unitFlags)
        {
            string unitFlagNames = "";
            List<long> unitFlagsList = new List<long>();

            if (unitFlags != 0)
            {
                var flagsArray = Enum.GetValues(typeof(UpdateObjectPacket.UnitFlags3));
                Array.Reverse(flagsArray);

                foreach (long flag in flagsArray)
                {
                    if (unitFlags - flag >= 0)
                    {
                        unitFlagsList.Add(flag);
                        unitFlags -= flag;
                    }
                }
            }
            else
                return unitFlagNames = "eUnitFlags3(0)";

            if (unitFlagsList.Count > 1)
            {
                unitFlagNames += $"eUnitFlags3(" + unitFlagsList.Aggregate(unitFlagNames, (current, itr) => current + ((UpdateObjectPacket.UnitFlags3)itr + " | ")) + ")";
                return unitFlagNames.Replace(" | )", ")");
            }
            else
                return unitFlagNames = ((UpdateObjectPacket.UnitFlags3)unitFlagsList.FirstOrDefault()).ToString();
        }
    }
}
