using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CreatureScriptsParser
{
    public static class Packets
    {
        [Serializable]
        public class Packet
        {
            public PacketTypes type;
            public TimeSpan time;
            public long number;
            public string guid = "";
            public long startIndex;
            public long endIndex;
            public Dictionary<long, long> indexes;

            public Packet(PacketTypes type, TimeSpan time, long number)
            { this.type = type;  this.time = time; this.number = number; }

            public Packet(PacketTypes type, TimeSpan time, long number, long start, long end)
            { this.type = type; this.time = time; this.number = number; startIndex = start; endIndex = end; }

            public Packet(PacketTypes type, TimeSpan time, long number, Dictionary<long, long> indexes)
            { this.type = type; this.time = time; this.number = number; this.indexes = indexes; }

            public enum PacketTypes
            {
                Unknown_PACKET,
                SMSG_UPDATE_OBJECT,
                SMSG_SPELL_START,
                SMSG_SPELL_GO,
                SMSG_ON_MONSTER_MOVE,
                SMSG_PLAY_ONE_SHOT_ANIM_KIT,
                SMSG_CHAT,
                SMSG_EMOTE,
                SMSG_AURA_UPDATE,
                SMSG_SET_AI_ANIM_KIT,
                SMSG_PLAY_SPELL_VISUAL_KIT,
                SMSG_PLAY_OBJECT_SOUND,
                SMSG_PLAY_SPELL_VISUAL
            }
        }

        [Serializable]
        public class UpdateObjectPacket : Packet
        {
            public ObjectType objectType;
            public UpdateType updateType;
            public uint creatureEntry;
            public uint conversationEntry;
            public Position spawnPosition;
            public uint? emoteStateId;
            public uint? sheatheState;
            public uint? standState;
            public bool isSummonedByPlayer;
            public bool hasReplacedObject;
            public List<string> conversationActors;
            public MonsterMovePacket moveData;
            public long? unitFlags;
            public long? unitFlags2;
            public long? unitFlags3;
            public uint factionTemplate;
            public uint mountDisplayId;

            public UpdateObjectPacket(PacketTypes packetType, TimeSpan time, long number, UpdateType updateType, ObjectType objectType) : base(packetType, time, number)
            { this.updateType = updateType; this.objectType = objectType; }

            public enum UpdateType
            {
                CreateObject = 1,
                Values       = 2,
                Destroy      = 3
            }

            public enum ObjectType
            {
                Creature      = 1,
                Conversation = 2
            }

            public enum SheathState
            {
                SHEATH_STATE_UNARMED = 0,
                SHEATH_STATE_MELEE   = 1,
                SHEATH_STATE_RANGED  = 2
            };

            public enum UnitStandStateType
            {
                UNIT_STAND_STATE_STAND             = 0,
                UNIT_STAND_STATE_SIT               = 1,
                UNIT_STAND_STATE_SIT_CHAIR         = 2,
                UNIT_STAND_STATE_SLEEP             = 3,
                UNIT_STAND_STATE_SIT_LOW_CHAIR     = 4,
                UNIT_STAND_STATE_SIT_MEDIUM_CHAIR  = 5,
                UNIT_STAND_STATE_SIT_HIGH_CHAIR    = 6,
                UNIT_STAND_STATE_DEAD              = 7,
                UNIT_STAND_STATE_KNEEL             = 8,
                UNIT_STAND_STATE_SUBMERGED         = 9
            };

            public enum UnitFlags : long
            {
                NotClientControlled        = 0x00000001,
                Spawning                   = 0x00000002,
                RemoveClientControl        = 0x00000004,
                PlayerControlled           = 0x00000008,
                Rename                     = 0x00000010,
                Preparation                = 0x00000020,
                Unk6                       = 0x00000040,
                NoAttack                   = 0x00000080,
                ImmunePC                   = 0x00000100,
                ImmuneNPC                  = 0x00000200,
                Looting                    = 0x00000400,
                PetIsAttackingTarget       = 0x00000800,
                PVP                        = 0x00001000,
                Silenced                   = 0x00002000,
                CannotSwim                 = 0x00004000,
                CanSwim                    = 0x00008000,
                NoAttack2                  = 0x00010000,
                Pacified                   = 0x00020000,
                Stunned                    = 0x00040000,
                AffectingCombat            = 0x00080000,
                OnTaxi                     = 0x00100000,
                Disarmed                   = 0x00200000,
                Confused                   = 0x00400000,
                Feared                     = 0x00800000,
                PossessedByPlayer          = 0x01000000,
                Uninteractible             = 0x02000000,
                Skinnable                  = 0x04000000,
                Mount                      = 0x08000000,
                PreventKneelingWhenLooting = 0x10000000,
                PreventEmotes              = 0x20000000,
                Sheath                     = 0x40000000,
                Immune                     = 0x80000000
            };

            public enum UnitFlags2 : long
            {
                UNIT_FLAG2_FEIGN_DEATH                                     = 0x00000001,
                UNIT_FLAG2_HIDE_BODY                                       = 0x00000002,
                UNIT_FLAG2_IGNORE_REPUTATION                               = 0x00000004,
                UNIT_FLAG2_COMPREHEND_LANG                                 = 0x00000008,
                UNIT_FLAG2_MIRROR_IMAGE                                    = 0x00000010,
                UNIT_FLAG2_INSTANTLY_DONT_FADE_IN                          = 0x00000020,
                UNIT_FLAG2_FORCE_MOVEMENT                                  = 0x00000040,
                UNIT_FLAG2_DISARM_OFFHAND                                  = 0x00000080,
                UNIT_FLAG2_DISABLE_PRED_STATS                              = 0x00000100,
                UNIT_FLAG2_ALLOW_CHANGING_TALENTS                          = 0x00000200,
                UNIT_FLAG2_DISARM_RANGED                                   = 0x00000400,
                UNIT_FLAG2_REGENERATE_POWER                                = 0x00000800,
                UNIT_FLAG2_RESTRICT_PARTY_INTERACTION                      = 0x00001000,
                UNIT_FLAG2_PREVENT_SPELL_CLICK                             = 0x00002000,
                UNIT_FLAG2_INTERACT_WHILE_HOSTILE                          = 0x00004000,
                UNIT_FLAG2_CANNOT_TURN                                     = 0x00008000,
                UNIT_FLAG2_UNK2                                            = 0x00010000,
                UNIT_FLAG2_PLAY_DEATH_ANIM                                 = 0x00020000,
                UNIT_FLAG2_ALLOW_CHEAT_SPELLS                              = 0x00040000,
                UNIT_FLAG2_SUPPRESS_HIGHLIGHT_WHEN_TARGETED_OR_MOUSED_OVER = 0x00080000,
                UNIT_FLAG2_TREAT_AS_RAID_UNIT_FOR_HELPFUL_SPELLS           = 0x00100000,
                UNIT_FLAG2_LARGE_AOI                                       = 0x00200000,
                UNIT_FLAG2_GIGANTIC_AOI                                    = 0x00400000,
                UNIT_FLAG2_NO_ACTIONS                                      = 0x00800000,
                UNIT_FLAG2_AI_WILL_ONLY_SWIM_IF_TARGET_SWIMS               = 0x01000000,
                UNIT_FLAG2_DONT_GENERATE_COMBAT_LOG_WHEN_ENGAGED_WITH_NPCS = 0x02000000,
                UNIT_FLAG2_UNTARGETABLE_BY_CLIENT                          = 0x04000000,
                UNIT_FLAG2_ATTACKER_IGNORES_MINIMUM_RANGES                 = 0x08000000,
                UNIT_FLAG2_UNINTERACTIBLE_IF_HOSTILE                       = 0x10000000,
                UNIT_FLAG2_UNK13                                           = 0x20000000,
                UNIT_FLAG2_INFINITE_AOI                                    = 0x40000000,
                UNIT_FLAG2_UNK15                                           = 0x80000000
            };

            public enum UnitFlags3 : long
            {
                UNIT_FLAG3_PASSIVE_AI                                  = 0x00000001,
                UNIT_FLAG3_UNCONSCIOUS_ON_DEATH                        = 0x00000002,
                UNIT_FLAG3_ALLOW_MOUNTED_COMBAT                        = 0x00000004,
                UNIT_FLAG3_GARRISON_PET                                = 0x00000008,
                UNIT_FLAG3_UI_CAN_GET_POSITION                         = 0x00000010,
                UNIT_FLAG3_AI_OBSTACLE                                 = 0x00000020,
                UNIT_FLAG3_ALTERNATIVE_DEFAULT_LANGUAGE                = 0x00000040,
                UNIT_FLAG3_SUPPRESS_ALL_NPC_FEEDBACK                   = 0x00000080,
                UNIT_FLAG3_IGNORE_COMBAT                               = 0x00000100,
                UNIT_FLAG3_SUPPRESS_NPC_FEEDBACK                       = 0x00000200,
                UNIT_FLAG3_UNK11                                       = 0x00000400,
                UNIT_FLAG3_UNK12                                       = 0x00000800,
                UNIT_FLAG3_UNK13                                       = 0x00001000,
                UNIT_FLAG3_FAKE_DEAD                                   = 0x00002000,
                UNIT_FLAG3_NO_FACING_ON_INTERACT_AND_FAST_FACING_CHASE = 0x00004000,
                UNIT_FLAG3_UNTARGETABLE_FROM_UI                        = 0x00008000,
                UNIT_FLAG3_NO_FACING_ON_INTERACT_WHILE_FAKE_DEAD       = 0x00010000,
                UNIT_FLAG3_ALREADY_SKINNED                             = 0x00020000,
                UNIT_FLAG3_SUPPRESS_ALL_NPC_SOUNDS                     = 0x00040000,
                UNIT_FLAG3_SUPPRESS_NPC_SOUNDS                         = 0x00080000,
                UNIT_FLAG3_UNK21                                       = 0x00100000,
                UNIT_FLAG3_UNK22                                       = 0x00200000,
                UNIT_FLAG3_DONT_FADE_OUT                               = 0x00400000,
                UNIT_FLAG3_UNK24                                       = 0x00800000,
                UNIT_FLAG3_HIDE_NAMEPLATE                              = 0x01000000,
                UNIT_FLAG3_UNK26                                       = 0x02000000,
                UNIT_FLAG3_UNK27                                       = 0x04000000,
                UNIT_FLAG3_UNK28                                       = 0x08000000,
                UNIT_FLAG3_UNK29                                       = 0x10000000,
                UNIT_FLAG3_UNK30                                       = 0x20000000,
                UNIT_FLAG3_UNK31                                       = 0x40000000,
                UNIT_FLAG3_UNK32                                       = 0x80000000
            };

            public static bool IsLineValidForObjectParse(string line)
            {
                if (line == null)
                    return false;

                if (line == "")
                    return false;

                if (line.Contains("UpdateType: 1 (CreateObject1)"))
                    return false;

                if (line.Contains("UpdateType: 2 (CreateObject2)"))
                    return false;

                if (line.Contains("UpdateType: 0 (Values)"))
                    return false;

                if (line.Contains("DataSize"))
                    return false;

                return true;
            }

            public static uint GetEntryFromLine(string line)
            {
                Regex entryRegexField = new Regex(@"EntryID:{1}\s*\d+");
                if (entryRegexField.IsMatch(line))
                    return Convert.ToUInt32(entryRegexField.Match(line).ToString().Replace("EntryID: ", ""));
                return 0;
            }

            public static Position GetSpawnPositionFromLine(string xyzLine, string oriLine)
            {
                Position spawnPosition = new Position();

                if (xyzLine.Contains("TransportPosition"))
                {
                    Regex xyzRegex = new Regex(@"TransportPosition:\s{1}X:{1}\s{1}");
                    if (xyzRegex.IsMatch(xyzLine))
                    {
                        string[] splittedLine = xyzLine.Split(' ');

                        spawnPosition.x = float.Parse(splittedLine[4], CultureInfo.InvariantCulture.NumberFormat);
                        spawnPosition.y = float.Parse(splittedLine[6], CultureInfo.InvariantCulture.NumberFormat);
                        spawnPosition.z = float.Parse(splittedLine[8], CultureInfo.InvariantCulture.NumberFormat);
                        spawnPosition.orientation = float.Parse(splittedLine[10], CultureInfo.InvariantCulture.NumberFormat);
                    }
                }
                else
                {
                    Regex xyzRegex = new Regex(@"Position:\s{1}X:{1}\s{1}");
                    if (xyzRegex.IsMatch(xyzLine))
                    {
                        string[] splittedLine = xyzLine.Split(' ');

                        spawnPosition.x = float.Parse(splittedLine[3], CultureInfo.InvariantCulture.NumberFormat);
                        spawnPosition.y = float.Parse(splittedLine[5], CultureInfo.InvariantCulture.NumberFormat);
                        spawnPosition.z = float.Parse(splittedLine[7], CultureInfo.InvariantCulture.NumberFormat);
                    }

                    Regex oriRegex = new Regex(@"Orientation:\s{1}");
                    if (oriRegex.IsMatch(oriLine))
                    {
                        string[] splittedLine = oriLine.Split(' ');

                        spawnPosition.orientation = float.Parse(splittedLine[2], CultureInfo.InvariantCulture.NumberFormat);
                    }
                }

                return spawnPosition;
            }

            public static uint? GetEmoteStateFromLine(string line)
            {
                Regex emoteRegex = new Regex(@"EmoteState:{1}\s{1}\w+");
                if (emoteRegex.IsMatch(line))
                    return Convert.ToUInt32(emoteRegex.Match(line).ToString().Replace("EmoteState: ", ""));

                return null;
            }

            public static uint? GetSheatheStateFromLine(string line)
            {
                Regex sheatheStateRegex = new Regex(@"SheatheState:{1}\s{1}\w+");
                if (sheatheStateRegex.IsMatch(line))
                    return Convert.ToUInt32(sheatheStateRegex.Match(line).ToString().Replace("SheatheState: ", ""));

                return null;
            }

            public static uint? GetStandStateFromLine(string line)
            {
                Regex standstateRegex = new Regex(@"StandState:{1}\s{1}\w+");
                if (standstateRegex.IsMatch(line))
                    return Convert.ToUInt32(standstateRegex.Match(line).ToString().Replace("StandState: ", ""));

                return null;
            }

            public static bool IsSummonedByPlayer (string line)
            {
                return (line.Contains("SummonedBy: TypeName: Player") || line.Contains("CreatedBy: TypeName: Player") || line.Contains("DemonCreator: TypeName: Player"));
            }

            public static bool HasReplacedObject(string line)
            {
                return line.Contains("ReplaceObject: TypeName: Creature");
            }

            public static bool? GetFlyingFromLine(string line)
            {
                if (line.Contains("SplineFlags:"))
                {
                    if (line.Contains("Flying"))
                        return true;
                    else
                        return false;
                }

                return null;
            }

            public static uint GetDurationFromLine(string line)
            {
                Regex durationRegex = new Regex(@"] Duration:{1}\s{1}\w+");
                if (durationRegex.IsMatch(line))
                    return Convert.ToUInt32(durationRegex.Match(line).ToString().Replace("] Duration: ", ""));

                return 0;
            }

            public static uint? GetUnitFlagsFromLine(string line)
            {
                Regex unitFlagsRegex = new Regex(@"\(UnitData\) Flags:{1}\s{1}\w+");
                if (unitFlagsRegex.IsMatch(line))
                    return Convert.ToUInt32(unitFlagsRegex.Match(line).ToString().Replace("(UnitData) Flags: ", ""));

                return null;
            }

            public static uint? GetUnitFlags2FromLine(string line)
            {
                Regex unitFlags2Regex = new Regex(@"\(UnitData\) Flags2:{1}\s{1}\w+");
                if (unitFlags2Regex.IsMatch(line))
                    return Convert.ToUInt32(unitFlags2Regex.Match(line).ToString().Replace("(UnitData) Flags2: ", ""));

                return null;
            }

            public static uint? GetUnitFlags3FromLine(string line)
            {
                Regex unitFlags3Regex = new Regex(@"\(UnitData\) Flags3:{1}\s{1}\w+");
                if (unitFlags3Regex.IsMatch(line))
                    return Convert.ToUInt32(unitFlags3Regex.Match(line).ToString().Replace("(UnitData) Flags3: ", ""));

                return null;
            }

            public static uint GetMountDisplayIdFromLine(string line)
            {
                Regex mountTemplateRegex = new Regex(@"\(UnitData\) MountDisplayID:{1}\s{1}\w+");
                if (mountTemplateRegex.IsMatch(line))
                    return Convert.ToUInt32(mountTemplateRegex.Match(line).ToString().Replace("(UnitData) MountDisplayID: ", ""));

                return 0;
            }

            public static uint GetFactionTemplateFromLine(string line)
            {
                Regex factionTemplateRegex = new Regex(@"\(UnitData\) FactionTemplate:{1}\s{1}\w+");
                if (factionTemplateRegex.IsMatch(line))
                    return Convert.ToUInt32(factionTemplateRegex.Match(line).ToString().Replace("(UnitData) FactionTemplate: ", ""));

                return 0;
            }

            public static IEnumerable<UpdateObjectPacket> ParseObjectUpdatePacket(string[] lines, Packet packet)
            {
                SynchronizedCollection<UpdateObjectPacket> updatePacketsList = new SynchronizedCollection<UpdateObjectPacket>();

                foreach(var itr in packet.indexes)
                {
                    if ((lines[itr.Key].Contains("UpdateType: CreateObject1") || lines[itr.Key].Contains("UpdateType: CreateObject2")) && lines[itr.Key + 1].IsCreatureLine())
                    {
                        UpdateObjectPacket updatePacket = new UpdateObjectPacket(packet.type, packet.time, packet.number, UpdateType.CreateObject, ObjectType.Creature);
                        updatePacket.moveData = new MonsterMovePacket(PacketTypes.SMSG_ON_MONSTER_MOVE, packet.time, packet.number);

                        for (long index = itr.Key; index < itr.Value; index++)
                        {
                            if (LineGetters.GetGuidFromLine(lines[index]) != "")
                                updatePacket.guid = LineGetters.GetGuidFromLine(lines[index]);

                            else if (IsSummonedByPlayer(lines[index]))
                                updatePacket.isSummonedByPlayer = true;

                            else if (HasReplacedObject(lines[index]))
                                updatePacket.hasReplacedObject = true;

                            else if (GetSpawnPositionFromLine(lines[index], lines[index + 1]).IsValid())
                                updatePacket.spawnPosition = GetSpawnPositionFromLine(lines[index], lines[index + 1]);

                            else if (GetEntryFromLine(lines[index]) != 0)
                                updatePacket.creatureEntry = GetEntryFromLine(lines[index]);

                            else if (GetFlyingFromLine(lines[index]) != null)
                                updatePacket.moveData.moveType = GetFlyingFromLine(lines[index]) == true ? MonsterMovePacket.MoveTypes.FLY : MonsterMovePacket.MoveTypes.UNKNOWN;

                            else if (GetDurationFromLine(lines[index]) != 0)
                                updatePacket.moveData.moveTime = GetDurationFromLine(lines[index]);

                            else if (MonsterMovePacket.GetPointPositionFromLine(lines[index]).IsValid())
                            {
                                do
                                {
                                    if (MonsterMovePacket.GetPointPositionFromLine(lines[index]).IsValid())
                                    {
                                        updatePacket.moveData.waypoints.Add(MonsterMovePacket.GetPointPositionFromLine(lines[index]));
                                    }

                                    index++;
                                }
                                while (lines[index].Contains("Points"));

                                Dictionary<int, float> distancesDictionary = new Dictionary<int, float>();

                                for (int i = 0; i < updatePacket.moveData.waypoints.Count(); i++)
                                {
                                    distancesDictionary.Add(i, updatePacket.spawnPosition.GetDistance(updatePacket.moveData.waypoints[i]));
                                }

                                for (int i = distancesDictionary.First(x => x.Value == distancesDictionary.Values.Min()).Key - 1; i >= 0; i--)
                                {
                                    updatePacket.moveData.waypoints.RemoveAt(i);
                                }
                            }
                        }

                        updatePacket.moveData.startPosition = updatePacket.spawnPosition;

                        if (updatePacket.moveData.moveType == MonsterMovePacket.MoveTypes.UNKNOWN && updatePacket.moveData.GetWaypointsVelocity() != 0.0f)
                        {
                            if (updatePacket.moveData.GetWaypointsVelocity() >= 4.2)
                            {
                                updatePacket.moveData.moveType = MonsterMovePacket.MoveTypes.RUN;
                            }
                            else
                            {
                                updatePacket.moveData.moveType = MonsterMovePacket.MoveTypes.WALK;
                            }
                        }

                        if (updatePacket.creatureEntry != 0 && updatePacket.guid != "")
                        {
                            updatePacketsList.Add(updatePacket);
                        }
                    }
                    else if (lines[itr.Key].Contains("UpdateType: Values") && lines[itr.Key + 1].IsCreatureLine())
                    {
                        UpdateObjectPacket updatePacket = new UpdateObjectPacket(PacketTypes.SMSG_UPDATE_OBJECT, packet.time, packet.number, UpdateType.Values, ObjectType.Creature);

                        Parallel.For(itr.Key, itr.Value, index =>
                        {
                            if (LineGetters.GetGuidFromLine(lines[index]) != "")
                                updatePacket.guid = LineGetters.GetGuidFromLine(lines[index]);

                            else if (GetEmoteStateFromLine(lines[index]) != null)
                                updatePacket.emoteStateId = GetEmoteStateFromLine(lines[index]);

                            else if (GetSheatheStateFromLine(lines[index]) != null)
                                updatePacket.sheatheState = GetSheatheStateFromLine(lines[index]);

                            else if (GetStandStateFromLine(lines[index]) != null)
                                updatePacket.standState = GetStandStateFromLine(lines[index]);

                            else if (GetUnitFlagsFromLine(lines[index]) != null)
                                updatePacket.unitFlags = GetUnitFlagsFromLine(lines[index]);

                            else if (GetUnitFlags2FromLine(lines[index]) != null)
                                updatePacket.unitFlags2 = GetUnitFlags2FromLine(lines[index]);

                            else if (GetUnitFlags3FromLine(lines[index]) != null)
                                updatePacket.unitFlags3 = GetUnitFlags3FromLine(lines[index]);

                            else if (GetFactionTemplateFromLine(lines[index]) != 0)
                                updatePacket.factionTemplate = GetFactionTemplateFromLine(lines[index]);

                            else if (GetMountDisplayIdFromLine(lines[index]) != 0)
                                updatePacket.mountDisplayId = GetMountDisplayIdFromLine(lines[index]);
                        });

                        if (updatePacket.guid != "" && (updatePacket.emoteStateId != null || updatePacket.sheatheState != null ||
                            updatePacket.standState != null || updatePacket.unitFlags != null || updatePacket.unitFlags2 != null ||
                            updatePacket.unitFlags3 != null || updatePacket.factionTemplate != 0 || updatePacket.mountDisplayId != 0))
                        {
                            updatePacketsList.Add(updatePacket);
                        }
                    }
                    else if (lines[itr.Key].Contains("DestroyedObjCount"))
                    {
                        Parallel.For(itr.Key, itr.Value, index =>
                        {
                            UpdateObjectPacket updatePacket = new UpdateObjectPacket(PacketTypes.SMSG_UPDATE_OBJECT, packet.time, packet.number, UpdateType.Destroy, ObjectType.Creature);

                            if (LineGetters.GetGuidFromLine(lines[index]) != "")
                                updatePacket.guid = LineGetters.GetGuidFromLine(lines[index]);

                            if (updatePacket.guid != "")
                            {
                                updatePacketsList.Add(updatePacket);
                            }
                        });
                    }
                    else if ((lines[itr.Key].Contains("UpdateType: CreateObject1") || lines[itr.Key].Contains("UpdateType: CreateObject2")) && lines[itr.Key + 1].IsConversationLine())
                    {
                        UpdateObjectPacket updatePacket = new UpdateObjectPacket(PacketTypes.SMSG_UPDATE_OBJECT, packet.time, packet.number, UpdateType.CreateObject, ObjectType.Conversation);
                        updatePacket.conversationActors = new List<string>();

                        Parallel.For(itr.Key, itr.Value, index =>
                        {
                            if (LineGetters.GetGuidFromLine(lines[index]) != "")
                                updatePacket.guid = LineGetters.GetGuidFromLine(lines[index]);

                            else if (GetEntryFromLine(lines[index]) != 0)
                                updatePacket.conversationEntry = GetEntryFromLine(lines[index]);

                            else if (LineGetters.GetGuidFromLine(lines[index], conversationActorGuid: true) != "")
                            {
                                lock(updatePacket.conversationActors)
                                {
                                    updatePacket.conversationActors.Add(LineGetters.GetGuidFromLine(lines[index], conversationActorGuid: true));
                                }
                            }
                        });

                        if (updatePacket.conversationEntry != 0 && updatePacket.guid != "" &&
                            updatePacket.conversationActors != null)
                        {
                            updatePacketsList.Add(updatePacket);
                        }
                    }
                }

                return updatePacketsList;
            }
        }

        [Serializable]
        public class SpellStartPacket : Packet
        {
            public string castGuid;
            public uint spellId;
            public Position destination;
            public List<string> targetGuids = new List<string>();

            public SpellStartPacket(PacketTypes packetType, TimeSpan time, long number) : base(packetType, time, number) { }
            public static uint GetSpellIdFromLine(string line)
            {
                Regex spellIdRegex = new Regex(@"SpellID:{1}\s*\d+");
                if (spellIdRegex.IsMatch(line))
                    return Convert.ToUInt32(spellIdRegex.Match(line).ToString().Replace("SpellID: ", ""));

                return 0;
            }

            public static Position GetSpellDestinationFromLine(string line)
            {
                Position destPosition = new Position();

                Regex xyzRegex = new Regex(@"Location:\s{1}X:{1}\s{1}");
                if (xyzRegex.IsMatch(line))
                {
                    string[] splittedLine = line.Split(' ');

                    destPosition.x = float.Parse(splittedLine[5], CultureInfo.InvariantCulture.NumberFormat);
                    destPosition.y = float.Parse(splittedLine[7], CultureInfo.InvariantCulture.NumberFormat);
                    destPosition.z = float.Parse(splittedLine[9], CultureInfo.InvariantCulture.NumberFormat);
                }

                return destPosition;
            }

            public static List<string> GetHitTargetGuidsFromLine(string[] lines, long index)
            {
                List<string> targetGuids = new List<string>();

                Regex hitTargetRegex = new Regex(@"\(Go\){1}\s{1}\[{1}\d]{1}\s{1}HitTarget:{1}");
                if (hitTargetRegex.IsMatch(lines[index]))
                {
                    do
                    {
                        if (LineGetters.GetGuidFromLine(lines[index], hitTargetGuid: true) != "")
                            targetGuids.Add(LineGetters.GetGuidFromLine(lines[index], hitTargetGuid: true));

                        index++;
                    }
                    while (!lines[index].Contains("HitStatusReason"));
                }

                return targetGuids;
            }

            public static bool IsCreatureSpellCastLine(string line)
            {
                return line.Contains("CasterGUID: TypeName: Creature;") || line.Contains("CasterGUID: TypeName: Vehicle;");
            }

            public static void FilterSpellPackets(List<object> packetList)
            {
                List<object> copyofPacketsList = new List<object>(packetList);

                Parallel.For(0, copyofPacketsList.Count(), i =>
                {
                    if (copyofPacketsList[i].GetType() == typeof(SpellStartPacket))
                    {
                        SpellStartPacket startPacket = (SpellStartPacket)copyofPacketsList[i];

                        if (startPacket.type == PacketTypes.SMSG_SPELL_START)
                        {
                            object packet = copyofPacketsList.FirstOrDefault(x => x.GetType() == typeof(SpellStartPacket) && ((SpellStartPacket)x).type == PacketTypes.SMSG_SPELL_GO && ((SpellStartPacket)x).castGuid == startPacket.castGuid);
                            if (packet != null)
                            {
                                SpellStartPacket goPacket = (SpellStartPacket)packet;
                                startPacket.destination = goPacket.destination;
                                startPacket.targetGuids = goPacket.targetGuids;

                                lock (packetList)
                                {
                                    packetList.Remove(goPacket);
                                }
                            }
                        }
                    }
                });

                Parallel.For(0, copyofPacketsList.Count(), i =>
                {
                    if (copyofPacketsList[i].GetType() == typeof(SpellStartPacket))
                    {
                        SpellStartPacket goPacket = (SpellStartPacket)copyofPacketsList[i];

                        if (goPacket.type == PacketTypes.SMSG_SPELL_GO)
                        {
                            object startPacket = copyofPacketsList.FirstOrDefault(x => x.GetType() == typeof(SpellStartPacket) && ((SpellStartPacket)x).type == PacketTypes.SMSG_SPELL_START && ((SpellStartPacket)x).castGuid == goPacket.castGuid);
                            if (startPacket != null)
                            {
                                lock (packetList)
                                {
                                    packetList.Remove(goPacket);
                                }
                            }
                        }
                    }
                });
            }

            public static SpellStartPacket ParseSpellStartPacket(string[] lines, Packet packet)
            {
                SpellStartPacket spellPacket = new SpellStartPacket(packet.type, packet.time, packet.number);

                if (packet.type == PacketTypes.SMSG_SPELL_START)
                {
                    Parallel.For(packet.startIndex, packet.endIndex, x =>
                    {
                        if (LineGetters.GetGuidFromLine(lines[x], casterGuid: true) != "")
                            spellPacket.guid = LineGetters.GetGuidFromLine(lines[x], casterGuid: true);

                        else if(LineGetters.GetGuidFromLine(lines[x], castGuid: true) != "")
                            spellPacket.castGuid = LineGetters.GetGuidFromLine(lines[x], castGuid: true);

                        else if(GetSpellIdFromLine(lines[x]) != 0)
                            spellPacket.spellId = GetSpellIdFromLine(lines[x]);
                    });
                }
                else
                {
                    Parallel.For(packet.startIndex, packet.endIndex, x =>
                    {
                        if (LineGetters.GetGuidFromLine(lines[x], casterGuid: true) != "")
                            spellPacket.guid = LineGetters.GetGuidFromLine(lines[x], casterGuid: true);

                        else if(LineGetters.GetGuidFromLine(lines[x], castGuid: true) != "")
                            spellPacket.castGuid = LineGetters.GetGuidFromLine(lines[x], castGuid: true);

                        else if(GetSpellIdFromLine(lines[x]) != 0)
                            spellPacket.spellId = GetSpellIdFromLine(lines[x]);

                        else if(GetSpellDestinationFromLine(lines[x]).IsValid())
                            spellPacket.destination = GetSpellDestinationFromLine(lines[x]);

                        else if(GetHitTargetGuidsFromLine(lines, x).Count != 0)
                        {
                            lock (spellPacket.targetGuids)
                            {
                                spellPacket.targetGuids = spellPacket.targetGuids.Union(GetHitTargetGuidsFromLine(lines, x)).ToList();
                            }
                        }
                    });

                }

                return spellPacket;
            }
        };

        [Serializable]
        public class MonsterMovePacket : Packet
        {
            public float creatureOrientation;
            public JumpInfo jumpInfo;
            public bool hasFacingToPlayer;
            public uint moveTime;
            public MoveTypes moveType = MoveTypes.UNKNOWN;
            public List<Position> waypoints = new List<Position>();
            public Position startPosition;
            public uint tierTransitionId;
            public MonsterSplineFilter splineFilter = new MonsterSplineFilter();

            [Serializable]
            public struct JumpInfo
            {
                public float jumpGravity;
                public Position jumpPos;

                public JumpInfo(float gravity, Position positon)
                { jumpGravity = gravity; jumpPos = positon; }

                public bool IsValid()
                {
                    return jumpGravity != 0.0f;
                }
            }

            public enum MoveTypes
            {
                WALK = 1,
                RUN = 2,
                FLY = 3,
                UNKNOWN = 4
            }

            [Serializable]
            public struct MonsterSplineFilterKey
            {
                public uint idx;
                public uint speed;
            };

            [Serializable]
            public struct MonsterSplineFilter
            {
                public List<MonsterSplineFilterKey?> filterKeys;
                public uint? filterFlags;
                public float? baseSpeed;
                public uint? startOffset;
                public float? distToPrevFilterKey;
                public uint? addedToStart;
                public bool filled;
            };

            public MonsterMovePacket(PacketTypes type, TimeSpan time, long number) : base(type, time, number) { }

            public static float GetFaceDirectionFromLine(string line)
            {
                Regex facingRegex = new Regex(@"FaceDirection:{1}\s+\d+\.+\d+");
                if (facingRegex.IsMatch(line))
                    return float.Parse(facingRegex.Match(line).ToString().Replace("FaceDirection: ", ""), CultureInfo.InvariantCulture.NumberFormat);

                return 0.0f;
            }
            public static Position GetPointPositionFromLine(string line)
            {
                Position pointPosition = new Position();

                Regex xyzRegex = new Regex(@"Points:{1}\s{1}X:{1}.+");
                if (xyzRegex.IsMatch(line))
                {
                    string[] splittedLine = xyzRegex.Match(line).ToString().Replace("Points: X: ", "").Split(' ');

                    pointPosition.x = float.Parse(splittedLine[0], CultureInfo.InvariantCulture.NumberFormat);
                    pointPosition.y = float.Parse(splittedLine[2], CultureInfo.InvariantCulture.NumberFormat);
                    pointPosition.z = float.Parse(splittedLine[4], CultureInfo.InvariantCulture.NumberFormat);
                }

                return pointPosition;
            }

            public static Position GetWayPointPositionFromLine(string line)
            {
                Position wayPointPosition = new Position();

                Regex xyzRegex = new Regex(@"WayPoints:{1}\s{1}X:{1}.+");
                if (xyzRegex.IsMatch(line))
                {
                    string[] splittedLine = xyzRegex.Match(line).ToString().Replace("WayPoints: X: ", "").Split(' ');

                    wayPointPosition.x = float.Parse(splittedLine[0], CultureInfo.InvariantCulture.NumberFormat);
                    wayPointPosition.y = float.Parse(splittedLine[2], CultureInfo.InvariantCulture.NumberFormat);
                    wayPointPosition.z = float.Parse(splittedLine[4], CultureInfo.InvariantCulture.NumberFormat);
                }

                return wayPointPosition;
            }

            public static float GetJumpGravityFromLine(string line)
            {
                Regex jumpGravityRegex = new Regex(@"JumpGravity:{1}\s+.+");
                if (jumpGravityRegex.IsMatch(line))
                    return float.Parse((jumpGravityRegex.Match(line).ToString().Replace("JumpGravity: ", "")), CultureInfo.InvariantCulture.NumberFormat);

                return 0.0f;
            }

            public static uint GetMoveTimeFromLine(string line)
            {
                Regex moveTimeRegex = new Regex(@"MoveTime:{1}\s+\d+");
                if (moveTimeRegex.IsMatch(line))
                    return Convert.ToUInt32(moveTimeRegex.Match(line).ToString().Replace("MoveTime: ", ""));

                return 0;
            }

            public static bool? GetFlyingFromLine(string line)
            {
                if (line.Contains("Flags:") && !line.Contains("FilterFlags"))
                {
                    if (line.Contains("Flying"))
                        return true;
                    else
                        return false;
                }

                return null;
            }

            public static Position GetPositionFromLine(string line)
            {
                Position position = new Position();

                Regex xyzRegex = new Regex(@"Position:{1}\s{1}X:{1}.+");
                if (xyzRegex.IsMatch(line))
                {
                    string[] splittedLine = xyzRegex.Match(line).ToString().Replace("Position: X: ", "").Split(' ');

                    position.x = float.Parse(splittedLine[0], CultureInfo.InvariantCulture.NumberFormat);
                    position.y = float.Parse(splittedLine[2], CultureInfo.InvariantCulture.NumberFormat);
                    position.z = float.Parse(splittedLine[4], CultureInfo.InvariantCulture.NumberFormat);
                }

                return position;
            }

            public static bool HasFacingToPlayer(string line)
            {
                return line.Contains("FacingGUID: TypeName: Player; Full:");
            }

            public bool HasOrientation()
            {
                return creatureOrientation != 0.0f;
            }

            public bool HasJump()
            {
                return jumpInfo.IsValid();
            }

            public string GetSetSpeedString()
            {
                switch (moveType)
                {
                    case MoveTypes.WALK:
                        return "me->SetSpeed(MOVE_WALK, " + GetSpeedRateString() + ", true);";
                    case MoveTypes.RUN:
                        return "me->SetSpeed(MOVE_RUN, " + GetSpeedRateString() + ", true);";
                    case MoveTypes.FLY:
                        return tierTransitionId == 0 ? "me->SetSpeed(MOVE_FLIGHT, " + GetSpeedRateString() + ", true);" : "me->SetSpeed(MOVE_RUN, " + GetSpeedRateString() + ", true);";
                    default:
                        return "";
                }
            }

            private string GetSpeedRateString()
            {
                string speedRate = "";

                switch (moveType)
                {
                    case MoveTypes.WALK:
                    {
                        speedRate += Convert.ToString(Math.Round((GetWaypointsVelocity() / 2.5f), 1)).Replace(",", ".");
                        break;
                    }
                    case MoveTypes.RUN:
                    {
                        speedRate += Convert.ToString(Math.Round((GetWaypointsVelocity() / 7.0f), 1)).Replace(",", ".");
                        break;
                    }
                    case MoveTypes.FLY:
                    {
                        speedRate += Convert.ToString(Math.Round((GetWaypointsVelocity() / 7.0f), 1)).Replace(",", ".");
                        break;
                    }
                    default:
                        break;
                }

                if (!speedRate.Contains("."))
                    return speedRate += ".0f";
                else
                    return speedRate += "f";
            }

            public static bool ConsistsOfPoints(string pointLine, string nextLine)
            {
                if (pointLine.Contains("[0] Points: X:") && nextLine.Contains("[1] Points: X:"))
                    return true;

                return false;
            }

            public float GetWaypointsDistance()
            {
                if (waypoints.Count() == 0)
                    return 0.0f;

                float distance = startPosition.GetDistance(waypoints.First());

                for (int i = 1; i < waypoints.Count(); i++)
                {
                    distance += waypoints[i - 1].GetDistance(waypoints[i]);
                }

                return distance;
            }

            public float GetWaypointsVelocity()
            {
                return GetWaypointsDistance() / moveTime * 1000;
            }

            public static uint GetTierTransitionIdFromLine(string line)
            {
                Regex tierTransitionIdRegex = new Regex(@"TierTransitionID:{1}\s+\d+");
                if (tierTransitionIdRegex.IsMatch(line))
                    return Convert.ToUInt32(tierTransitionIdRegex.Match(line).ToString().Replace("TierTransitionID: ", ""));

                return 0;
            }

            public static float? GetBaseSpeedFromLine(string line)
            {
                Regex baseSpeedFloatRegex = new Regex(@"BaseSpeed:{1}\s+\d+\.+\d+");
                if (baseSpeedFloatRegex.IsMatch(line))
                    return float.Parse(baseSpeedFloatRegex.Match(line).ToString().Replace("BaseSpeed: ", ""), CultureInfo.InvariantCulture.NumberFormat);
                else
                {
                    Regex baseSpeedIntRegex = new Regex(@"BaseSpeed:{1}\s+\d+");
                    if (baseSpeedIntRegex.IsMatch(line))
                        return float.Parse(baseSpeedIntRegex.Match(line).ToString().Replace("BaseSpeed: ", ""), CultureInfo.InvariantCulture.NumberFormat);
                }

                return null;
            }


            public static uint? GetStartOffsetFromLine(string line)
            {
                Regex startOffsetRegex = new Regex(@"StartOffset:{1}\s+\d+");
                if (startOffsetRegex.IsMatch(line))
                    return Convert.ToUInt32(startOffsetRegex.Match(line).ToString().Replace("StartOffset: ", ""));

                return null;
            }

            public static float? GetDistToPrevFilterKeyFromLine(string line)
            {
                Regex distToPrevFilterKeyFloatRegex = new Regex(@"DistToPrevFilterKey:{1}\s+\d+\.+\d+");
                if (distToPrevFilterKeyFloatRegex.IsMatch(line))
                    return float.Parse(distToPrevFilterKeyFloatRegex.Match(line).ToString().Replace("DistToPrevFilterKey: ", ""), CultureInfo.InvariantCulture.NumberFormat);
                else
                {
                    Regex distToPrevFilterKeyIntRegex = new Regex(@"DistToPrevFilterKey:{1}\s+\d+");
                    if (distToPrevFilterKeyIntRegex.IsMatch(line))
                        return float.Parse(distToPrevFilterKeyIntRegex.Match(line).ToString().Replace("DistToPrevFilterKey: ", ""), CultureInfo.InvariantCulture.NumberFormat);
                }

                return null;
            }

            public static uint? GetAddedToStartFromLine(string line)
            {
                Regex addedToStartRegex = new Regex(@"AddedToStart:{1}\s+\d+");
                if (addedToStartRegex.IsMatch(line))
                    return Convert.ToUInt32(addedToStartRegex.Match(line).ToString().Replace("AddedToStart: ", ""));

                return null;
            }

            public static uint? GetFilterFlagsFromLine(string line)
            {
                Regex filterFlagsRegex = new Regex(@"FilterFlags:{1}\s+\d+");
                if (filterFlagsRegex.IsMatch(line))
                    return Convert.ToUInt32(filterFlagsRegex.Match(line).ToString().Replace("FilterFlags: ", ""));

                return null;
            }

            public static MonsterSplineFilterKey? GetFilterKeyFromLine(string[] lines, long idx)
            {
                if (lines[idx].Contains("IDx"))
                {
                    MonsterSplineFilterKey monsterSplineFilterKey = new MonsterSplineFilterKey();

                    Regex idxRegex = new Regex(@"IDx:{1}\s+\d+");
                    Regex speedRegex = new Regex(@"Speed:{1}\s+\d+");

                    if (idxRegex.IsMatch(lines[idx]))
                        monsterSplineFilterKey.idx = Convert.ToUInt32(idxRegex.Match(lines[idx]).ToString().Replace("IDx: ", ""));

                    if (speedRegex.IsMatch(lines[idx + 1]))
                        monsterSplineFilterKey.speed = Convert.ToUInt32(speedRegex.Match(lines[idx + 1]).ToString().Replace("Speed: ", ""));

                    return monsterSplineFilterKey;
                }

                return null;
            }

            public static void ParseSplineFlagsIfPossible(string[] lines, long idx, ref MonsterMovePacket movePacket)
            {
                if (movePacket.splineFilter.filled)
                    return;

                if (lines[idx].Contains("MonsterSplineFilter"))
                {
                    do
                    {
                        if (GetBaseSpeedFromLine(lines[idx]) != null)
                        {
                            movePacket.splineFilter.baseSpeed = GetBaseSpeedFromLine(lines[idx]);
                        }
                        else if (GetStartOffsetFromLine(lines[idx]) != null)
                        {
                            movePacket.splineFilter.startOffset = GetStartOffsetFromLine(lines[idx]);
                        }
                        else if (GetDistToPrevFilterKeyFromLine(lines[idx]) != null)
                        {
                            movePacket.splineFilter.distToPrevFilterKey = GetDistToPrevFilterKeyFromLine(lines[idx]);
                        }
                        else if (GetAddedToStartFromLine(lines[idx]) != null)
                        {
                            movePacket.splineFilter.addedToStart = GetAddedToStartFromLine(lines[idx]);
                        }
                        else if (GetFilterFlagsFromLine(lines[idx]) != null)
                        {
                            movePacket.splineFilter.filterFlags = GetFilterFlagsFromLine(lines[idx]);
                        }
                        else if (GetFilterKeyFromLine(lines, idx) != null)
                        {
                            if (movePacket.splineFilter.filterKeys == null)
                            {
                                movePacket.splineFilter.filterKeys = new List<MonsterSplineFilterKey?>();
                                movePacket.splineFilter.filterKeys.Add(GetFilterKeyFromLine(lines, idx));
                            }
                            else
                            {
                                movePacket.splineFilter.filterKeys.Add(GetFilterKeyFromLine(lines, idx));
                            }
                        }

                        idx++;
                    }
                    while (lines[idx].Contains("MonsterSplineFilter"));

                    movePacket.splineFilter.filled = true;
                }
            }

            public static MonsterMovePacket ParseMovementPacket(string[] lines, Packet packet)
            {
                MonsterMovePacket movePacket = new MonsterMovePacket(packet.type, packet.time, packet.number);

                if (lines[packet.startIndex + 1].IsCreatureLine())
                {
                    Position lastPosition = new Position();

                    for (long x = packet.startIndex; x < packet.endIndex; x++)
                    {
                        if (LineGetters.GetGuidFromLine(lines[x], moverGuid: true) != "")
                            movePacket.guid = LineGetters.GetGuidFromLine(lines[x], moverGuid: true);

                        else if (GetPositionFromLine(lines[x]).IsValid())
                            movePacket.startPosition = GetPositionFromLine(lines[x]);

                        else if(GetFaceDirectionFromLine(lines[x]) != 0.0f)
                            movePacket.creatureOrientation = GetFaceDirectionFromLine(lines[x]);

                        else if(GetMoveTimeFromLine(lines[x]) != 0)
                            movePacket.moveTime = GetMoveTimeFromLine(lines[x]);

                        else if(GetJumpGravityFromLine(lines[x]) != 0.0f)
                            movePacket.jumpInfo.jumpGravity = GetJumpGravityFromLine(lines[x]);

                        else if(HasFacingToPlayer(lines[x]))
                            movePacket.hasFacingToPlayer = true;

                        else if (GetFlyingFromLine(lines[x]) != null)
                            movePacket.moveType = GetFlyingFromLine(lines[x]) == true ? MoveTypes.FLY : MoveTypes.UNKNOWN;

                        else if (GetPointPositionFromLine(lines[x]).IsValid())
                        {
                            if (ConsistsOfPoints(lines[x], lines[x + 1]))
                            {
                                do
                                {
                                    if (GetPointPositionFromLine(lines[x]).IsValid())
                                    {
                                        movePacket.waypoints.Add(GetPointPositionFromLine(lines[x]));
                                    }

                                    x++;
                                }
                                while (lines[x].Contains("Points"));
                            }
                            else
                            {
                                if (GetPointPositionFromLine(lines[x]).IsValid())
                                    lastPosition = GetPointPositionFromLine(lines[x]);

                                if (GetWayPointPositionFromLine(lines[x + 1]).IsValid())
                                {
                                    do
                                    {
                                        if (GetWayPointPositionFromLine(lines[x]).IsValid())
                                        {
                                            movePacket.waypoints.Add(GetWayPointPositionFromLine(lines[x]));
                                        }

                                        x++;
                                    }
                                    while (lines[x].Contains("WayPoints"));
                                }
                            }

                            if (lastPosition.IsValid())
                            {
                                movePacket.waypoints.Add(lastPosition);
                            }
                        }

                        else if (GetTierTransitionIdFromLine(lines[x]) != 0)
                            movePacket.tierTransitionId = GetTierTransitionIdFromLine(lines[x]);

                        ParseSplineFlagsIfPossible(lines, x, ref movePacket);
                    }
                }

                if (movePacket.HasJump())
                {
                    movePacket.jumpInfo.jumpPos = movePacket.waypoints.First();
                }

                if (movePacket.hasFacingToPlayer)
                {
                    movePacket.creatureOrientation = 0.0f;
                }

                if (movePacket.moveType == MoveTypes.UNKNOWN && movePacket.GetWaypointsVelocity() != 0.0f)
                {
                    if (movePacket.GetWaypointsVelocity() >= 4.2)
                    {
                        movePacket.moveType = MoveTypes.RUN;
                    }
                    else
                    {
                        movePacket.moveType = MoveTypes.WALK;
                    }
                }

                return movePacket;
            }
        }

        [Serializable]
        public class PlayOneShotAnimKit : Packet
        {
            public uint? AnimKitId;

            public PlayOneShotAnimKit(PacketTypes type, TimeSpan time, long number) : base(type, time, number) { }

            public static uint? GetAnimKitIdFromLine(string line)
            {
                Regex animKitRegex = new Regex(@"AnimKitID:{1}\s+\d+");
                if (animKitRegex.IsMatch(line))
                    return Convert.ToUInt32(animKitRegex.Match(line).ToString().Replace("AnimKitID: ", ""));

                return null;
            }

            public static PlayOneShotAnimKit ParsePlayOneShotAnimKitPacket(string[] lines, Packet packet)
            {
                PlayOneShotAnimKit animKitPacket = new PlayOneShotAnimKit(packet.type, packet.time, packet.number);

                if (lines[packet.startIndex + 1].IsCreatureLine())
                {
                    Parallel.For(packet.startIndex, packet.endIndex, x =>
                    {
                        if (LineGetters.GetGuidFromLine(lines[x], oneShotAnimKitGuid: true) != "")
                            animKitPacket.guid = LineGetters.GetGuidFromLine(lines[x], oneShotAnimKitGuid: true);

                        else if(GetAnimKitIdFromLine(lines[x]) != null)
                            animKitPacket.AnimKitId = GetAnimKitIdFromLine(lines[x]);
                    });
                }

                return animKitPacket;
            }
        }

        [Serializable]
        public class ChatPacket : Packet
        {
            public string creatureText;

            public ChatPacket(PacketTypes type, TimeSpan time, long number) : base(type, time, number) { }

            public static bool IsCreatureText(string line)
            {
                if (LineGetters.GetGuidFromLine(line, senderGuid: true) != "")
                    return true;

                return false;
            }

            public static string GetTextFromLine(string line)
            {
                if (line.Contains("Text:"))
                    return line.Replace("Text: ", "");

                return "";
            }

            public static ChatPacket ParseChatPacket(string[] lines, Packet packet)
            {
                ChatPacket chatPacket = new ChatPacket(packet.type, packet.time, packet.number);

                if (IsCreatureText(lines[packet.startIndex + 3]))
                {
                    Parallel.For(packet.startIndex, packet.endIndex, x =>
                    {
                        if (LineGetters.GetGuidFromLine(lines[x], senderGuid: true) != "")
                            chatPacket.guid = LineGetters.GetGuidFromLine(lines[x], senderGuid: true);

                        else if(GetTextFromLine(lines[x]) != "")
                            chatPacket.creatureText = GetTextFromLine(lines[x]);
                    });
                }

                return chatPacket;
            }
        }

        [Serializable]
        public class EmotePacket : Packet
        {
            public uint emoteId;

            public EmotePacket(PacketTypes type, TimeSpan time, long number) : base(type, time, number) { }

            public enum Emote
            {
                EMOTE_ONESHOT_NONE                             = 0,
                EMOTE_ONESHOT_TALK                             = 1,
                EMOTE_ONESHOT_BOW                              = 2,
                EMOTE_ONESHOT_WAVE                             = 3,
                EMOTE_ONESHOT_CHEER                            = 4,
                EMOTE_ONESHOT_EXCLAMATION                      = 5,
                EMOTE_ONESHOT_QUESTION                         = 6,
                EMOTE_ONESHOT_EAT                              = 7,
                EMOTE_STATE_DANCE                              = 10,
                EMOTE_ONESHOT_LAUGH                            = 11,
                EMOTE_STATE_SLEEP                              = 12,
                EMOTE_STATE_SIT                                = 13,
                EMOTE_ONESHOT_RUDE                             = 14,
                EMOTE_ONESHOT_ROAR                             = 15,
                EMOTE_ONESHOT_KNEEL                            = 16,
                EMOTE_ONESHOT_KISS                             = 17,
                EMOTE_ONESHOT_CRY                              = 18,
                EMOTE_ONESHOT_CHICKEN                          = 19,
                EMOTE_ONESHOT_BEG                              = 20,
                EMOTE_ONESHOT_APPLAUD                          = 21,
                EMOTE_ONESHOT_SHOUT                            = 22,
                EMOTE_ONESHOT_FLEX                             = 23,
                EMOTE_ONESHOT_SHY                              = 24,
                EMOTE_ONESHOT_POINT                            = 25,
                EMOTE_STATE_STAND                              = 26,
                EMOTE_STATE_READYUNARMED                       = 27,
                EMOTE_STATE_WORK_SHEATHED                      = 28,
                EMOTE_STATE_POINT                              = 29,
                EMOTE_STATE_NONE                               = 30,
                EMOTE_ONESHOT_WOUND                            = 33,
                EMOTE_ONESHOT_WOUND_CRITICAL                   = 34,
                EMOTE_ONESHOT_ATTACK_UNARMED                   = 35,
                EMOTE_ONESHOT_ATTACK1H                         = 36,
                EMOTE_ONESHOT_ATTACK2HTIGHT                    = 37,
                EMOTE_ONESHOT_ATTACK2HLOOSE                    = 38,
                EMOTE_ONESHOT_PARRYUNARMED                     = 39,
                EMOTE_ONESHOT_PARRY_SHIELD                     = 43,
                EMOTE_ONESHOT_READYUNARMED                     = 44,
                EMOTE_ONESHOT_READY1H                          = 45,
                EMOTE_ONESHOT_READYBOW                         = 48,
                EMOTE_ONESHOT_SPELLPRECAST                     = 50,
                EMOTE_ONESHOT_SPELL_CAST                       = 51,
                EMOTE_ONESHOT_BATTLEROAR                       = 53,
                EMOTE_ONESHOT_SPECIALATTACK1H                  = 54,
                EMOTE_ONESHOT_KICK                             = 60,
                EMOTE_ONESHOT_ATTACKTHROWN                     = 61,
                EMOTE_STATE_STUN                               = 64,
                EMOTE_STATE_DEAD                               = 65,
                EMOTE_ONESHOT_SALUTE                           = 66,
                EMOTE_STATE_KNEEL                              = 68,
                EMOTE_STATE_USE_STANDING                       = 69,
                EMOTE_ONESHOT_WAVE_NOSHEATHE                   = 70,
                EMOTE_ONESHOT_CHEER_NOSHEATHE                  = 71,
                EMOTE_ONESHOT_EAT_NOSHEATHE                    = 92,
                EMOTE_STATE_STUN_NOSHEATHE                     = 93,
                EMOTE_ONESHOT_DANCE                            = 94,
                EMOTE_ONESHOT_SALUTE_NOSHEATH                  = 113,
                EMOTE_STATE_USE_STANDING_NO_SHEATHE            = 133,
                EMOTE_ONESHOT_LAUGH_NOSHEATHE                  = 153,
                EMOTE_STATE_WORK                               = 173,
                EMOTE_STATE_SPELLPRECAST                       = 193,
                EMOTE_ONESHOT_READYRIFLE                       = 213,
                EMOTE_STATE_READYRIFLE                         = 214,
                EMOTE_STATE_WORK_MINING                        = 233,
                EMOTE_STATE_WORK_CHOPWOOD                      = 234,
                EMOTE_STATE_APPLAUD                            = 253,
                EMOTE_ONESHOT_LIFTOFF                          = 254,
                EMOTE_ONESHOT_YES                              = 273,
                EMOTE_ONESHOT_NO                               = 274,
                EMOTE_ONESHOT_TRAIN                            = 275,
                EMOTE_ONESHOT_LAND                             = 293,
                EMOTE_STATE_AT_EASE                            = 313,
                EMOTE_STATE_READY1H                            = 333,
                EMOTE_STATE_SPELLKNEELSTART                    = 353,
                EMOTE_STAND_STATE_SUBMERGED                    = 373,
                EMOTE_ONESHOT_SUBMERGE                         = 374,
                EMOTE_STATE_READY2H                            = 375,
                EMOTE_STATE_READYBOW                           = 376,
                EMOTE_ONESHOT_MOUNTSPECIAL                     = 377,
                EMOTE_STATE_TALK                               = 378,
                EMOTE_STATE_FISHING                            = 379,
                EMOTE_ONESHOT_FISHING                          = 380,
                EMOTE_ONESHOT_LOOT                             = 381,
                EMOTE_STATE_WHIRLWIND                          = 382,
                EMOTE_STATE_DROWNED                            = 383,
                EMOTE_STATE_HOLD_BOW                           = 384,
                EMOTE_STATE_HOLD_RIFLE                         = 385,
                EMOTE_STATE_HOLD_THROWN                        = 386,
                EMOTE_ONESHOT_DROWN                            = 387,
                EMOTE_ONESHOT_STOMP                            = 388,
                EMOTE_ONESHOT_ATTACKOFF                        = 389,
                EMOTE_ONESHOT_ATTACKOFFPIERCE                  = 390,
                EMOTE_STATE_ROAR                               = 391,
                EMOTE_STATE_LAUGH                              = 392,
                EMOTE_ONESHOT_CREATURE_SPECIAL                 = 393,
                EMOTE_ONESHOT_JUMPLANDRUN                      = 394,
                EMOTE_ONESHOT_JUMPEND                          = 395,
                EMOTE_ONESHOT_TALK_NO_SHEATHE                  = 396,
                EMOTE_ONESHOT_POINT_NO_SHEATHE                 = 397,
                EMOTE_STATE_CANNIBALIZE                        = 398,
                EMOTE_ONESHOT_JUMPSTART                        = 399,
                EMOTE_STATE_DANCESPECIAL                       = 400,
                EMOTE_ONESHOT_DANCESPECIAL                     = 401,
                EMOTE_ONESHOT_CUSTOM_SPELL_01                  = 402,
                EMOTE_ONESHOT_CUSTOM_SPELL_02                  = 403,
                EMOTE_ONESHOT_CUSTOM_SPELL_03                  = 404,
                EMOTE_ONESHOT_CUSTOM_SPELL_04                  = 405,
                EMOTE_ONESHOT_CUSTOM_SPELL_05                  = 406,
                EMOTE_ONESHOT_CUSTOM_SPELL_06                  = 407,
                EMOTE_ONESHOT_CUSTOM_SPELL_07                  = 408,
                EMOTE_ONESHOT_CUSTOM_SPELL_08                  = 409,
                EMOTE_ONESHOT_CUSTOM_SPELL_09                  = 410,
                EMOTE_ONESHOT_CUSTOM_SPELL_10                  = 411,
                EMOTE_STATE_EXCLAIM                            = 412,
                EMOTE_STATE_DANCE_CUSTOM                       = 413,
                EMOTE_STATE_SIT_CHAIR_MED                      = 415,
                EMOTE_STATE_CUSTOM_SPELL_01                    = 416,
                EMOTE_STATE_CUSTOM_SPELL_02                    = 417,
                EMOTE_STATE_EAT                                = 418,
                EMOTE_STATE_CUSTOM_SPELL_04                    = 419,
                EMOTE_STATE_CUSTOM_SPELL_03                    = 420,
                EMOTE_STATE_CUSTOM_SPELL_05                    = 421,
                EMOTE_STATE_SPELLEFFECT_HOLD                   = 422,
                EMOTE_STATE_EAT_NO_SHEATHE                     = 423,
                EMOTE_STATE_MOUNT                              = 424,
                EMOTE_STATE_READY2HL                           = 425,
                EMOTE_STATE_SIT_CHAIR_HIGH                     = 426,
                EMOTE_STATE_FALL                               = 427,
                EMOTE_STATE_LOOT                               = 428,
                EMOTE_STATE_SUBMERGED                          = 429,
                EMOTE_ONESHOT_COWER                            = 430,
                EMOTE_STATE_COWER                              = 431,
                EMOTE_ONESHOT_USESTANDING                      = 432,
                EMOTE_STATE_STEALTH_STAND                      = 433,
                EMOTE_ONESHOT_OMNICAST_GHOUL                   = 434,
                EMOTE_ONESHOT_ATTACKBOW                        = 435,
                EMOTE_ONESHOT_ATTACKRIFLE                      = 436,
                EMOTE_STATE_SWIM_IDLE                          = 437,
                EMOTE_STATE_ATTACK_UNARMED                     = 438,
                EMOTE_ONESHOT_SPELLCAST_NEW                    = 439,
                EMOTE_ONESHOT_DODGE                            = 440,
                EMOTE_ONESHOT_PARRY1H                          = 441,
                EMOTE_ONESHOT_PARRY2H                          = 442,
                EMOTE_ONESHOT_PARRY2HL                         = 443,
                EMOTE_STATE_FLYFALL                            = 444,
                EMOTE_ONESHOT_FLYDEATH                         = 445,
                EMOTE_STATE_FLY_FALL                           = 446,
                EMOTE_ONESHOT_FLY_SIT_GROUND_DOWN              = 447,
                EMOTE_ONESHOT_FLY_SIT_GROUND_UP                = 448,
                EMOTE_ONESHOT_EMERGE                           = 449,
                EMOTE_ONESHOT_DRAGONSPIT                       = 450,
                EMOTE_STATE_SPECIALUNARMED                     = 451,
                EMOTE_ONESHOT_FLYGRAB                          = 452,
                EMOTE_STATE_FLYGRABCLOSED                      = 453,
                EMOTE_ONESHOT_FLYGRABTHROWN                    = 454,
                EMOTE_STATE_FLY_SIT_GROUND                     = 455,
                EMOTE_STATE_WALKBACKWARDS                      = 456,
                EMOTE_ONESHOT_FLYTALK                          = 457,
                EMOTE_ONESHOT_FLYATTACK1H                      = 458,
                EMOTE_STATE_CUSTOMSPELL08                      = 459,
                EMOTE_ONESHOT_FLY_DRAGONSPIT                   = 460,
                EMOTE_STATE_SIT_CHAIR_LOW                      = 461,
                EMOTE_ONE_SHOT_STUN                            = 462,
                EMOTE_ONESHOT_SPELL_CAST_OMNI                  = 463,
                EMOTE_STATE_READYTHROWN                        = 465,
                EMOTE_ONESHOT_WORK_CHOPWOOD                    = 466,
                EMOTE_ONESHOT_WORK_MINING                      = 467,
                EMOTE_STATE_SPELL_CHANNEL_OMNI                 = 468,
                EMOTE_STATE_SPELL_CHANNEL_DIRECTED             = 469,
                EMOTE_STAND_STATE_NONE                         = 470,
                EMOTE_STATE_READYJOUST                         = 471,
                EMOTE_STATE_STRANGULATE                        = 472,
                EMOTE_STATE_STRANGULATE2                       = 473,
                EMOTE_STATE_READYSPELLOMNI                     = 474,
                EMOTE_STATE_HOLD_JOUST                         = 475,
                EMOTE_ONESHOT_CRY_JAINA                        = 476,
                EMOTE_ONESHOT_SPECIALUNARMED                   = 477,
                EMOTE_STATE_DANCE_NOSHEATHE                    = 478,
                EMOTE_ONESHOT_SNIFF                            = 479,
                EMOTE_ONESHOT_DRAGONSTOMP                      = 480,
                EMOTE_ONESHOT_KNOCKDOWN                        = 482,
                EMOTE_STATE_READ                               = 483,
                EMOTE_ONESHOT_FLYEMOTETALK                     = 485,
                EMOTE_STATE_READ_ALLOWMOVEMENT                 = 492,
                EMOTE_STATE_CUSTOM_SPELL_06                    = 498,
                EMOTE_STATE_CUSTOM_SPELL_07                    = 499,
                EMOTE_STATE_CUSTOM_SPELL_08                    = 500,
                EMOTE_STATE_CUSTOM_SPELL_09                    = 501,
                EMOTE_STATE_CUSTOM_SPELL_10                    = 502,
                EMOTE_STATE_READY1H_ALLOW_MOVEMENT             = 505,
                EMOTE_STATE_READY2H_ALLOW_MOVEMENT             = 506,
                EMOTE_ONESHOT_MONKOFFENSE_ATTACKUNARMED        = 507,
                EMOTE_ONESHOT_MONKOFFENSE_SPECIALUNARMED       = 508,
                EMOTE_ONESHOT_MONKOFFENSE_PARRYUNARMED         = 509,
                EMOTE_STATE_MONKOFFENSE_READYUNARMED           = 510,
                EMOTE_ONESHOT_PALMSTRIKE                       = 511,
                EMOTE_STATE_CRANE                              = 512,
                EMOTE_ONESHOT_OPEN                             = 517,
                EMOTE_STATE_READ_CHRISTMAS                     = 518,
                EMOTE_ONESHOT_FLYATTACK2HL                     = 526,
                EMOTE_ONESHOT_FLYATTACKTHROWN                  = 527,
                EMOTE_STATE_FLYREADYSPELLDIRECTED              = 528,
                EMOTE_STATE_FLY_READY_1H                       = 531,
                EMOTE_STATE_MEDITATE                           = 533,
                EMOTE_STATE_FLY_READY_2HL                      = 534,
                EMOTE_ONESHOT_TOGROUND                         = 535,
                EMOTE_ONESHOT_TOFLY                            = 536,
                EMOTE_STATE_ATTACKTHROWN                       = 537,
                EMOTE_STATE_SPELL_CHANNEL_DIRECTED_NOSOUND     = 538,
                EMOTE_ONESHOT_WORK                             = 539,
                EMOTE_STATE_READYUNARMED_NOSOUND               = 540,
                EMOTE_ONESHOT_MONKOFFENSE_ATTACKUNARMEDOFF     = 543,
                EMOTE_RECLINED_MOUNT_PASSENGER                 = 546,
                EMOTE_ONESHOT_QUESTION_NEW                     = 547,
                EMOTE_ONESHOT_SPELL_CHANNEL_DIRECTED_NOSOUND   = 549,
                EMOTE_STATE_KNEEL_NEW                          = 550,
                EMOTE_ONESHOT_FLYATTACKUNARMED                 = 551,
                EMOTE_ONESHOT_FLYCOMBATWOUND                   = 552,
                EMOTE_ONESHOT_MOUNTSELFSPECIAL                 = 553,
                EMOTE_ONESHOT_ATTACKUNARMED_NOSOUND            = 554,
                EMOTE_STATE_WOUNDCRITICAL_DOESNTWORK           = 555,
                EMOTE_ONESHOT_ATTACK1H_NOSOUND_DOESNTWORK      = 556,
                EMOTE_STATE_MOUNT_SELF_IDLE                    = 557,
                EMOTE_ONESHOT_WALK                             = 558,
                EMOTE_STATE_OPENED                             = 559,
                EMOTE_ONESHOT_YELL_USE_ONESHOT_SHOUT           = 560,
                EMOTE_ONESHOT_BREATHOFFIRE                     = 565,
                EMOTE_STATE_ATTACK1H                           = 567,
                EMOTE_STATE_USESTANDING                        = 572,
                EMOTE_ONESHOT_LAUGH_NOSOUND                    = 574,
                EMOTE_RECLINED_MOUNT                           = 575,
                EMOTE_ONESHOT_ATTACK1H_NEW                     = 577,
                EMOTE_STATE_CRY_NOSOUND                        = 578,
                EMOTE_ONESHOT_CRY_NOSOUND                      = 579,
                EMOTE_ONESHOT_COMBATCRITICAL                   = 584,
                EMOTE_STATE_TRAIN                              = 585,
                EMOTE_STATE_WORK_CHOPWOOD_NEW                  = 586, // lumber Axe
                EMOTE_ONESHOT_SPECIALATTACK2H                  = 587,
                EMOTE_STATE_READ_AND_TALK                      = 588,
                EMOTE_ONESHOT_STAND_VAR1                       = 589,
                EMOTE_REXXAR_STRANGLES_GOBLIN                  = 590,
                EMOTE_ONESHOT_STAND_VAR2                       = 591,
                EMOTE_ONESHOT_DEATH                            = 592,
                EMOTE_STATE_TALKONCE                           = 595,
                EMOTE_STATE_ATTACK2H                           = 596,
                EMOTE_STATE_SIT_GROUND                         = 598,
                EMOTE_STATE_WORK_CHOPWOOD_GARR                 = 599, // (NO AXE, GARRISON)
                EMOTE_STATE_CUSTOMSPELL01                      = 601,
                EMOTE_ONESHOT_COMBATWOUND                      = 602,
                EMOTE_ONESHOT_TALK_EXCLAMATION                 = 603,
                EMOTE_ONESHOT_QUESTION_SWIM_ALLOW              = 604, // (Allow While Swim)
                EMOTE_STATE_CRY                                = 605,
                EMOTE_STATE_USESTANDING_LOOP                   = 606, // (BUZZSAW)
                EMOTE_STATE_WORK_SMITH                         = 613, // (BLACKSMITH HAMMER)
                EMOTE_STATE_WORK_CHOPWOOD_GARR_FLESH           = 614, // (NO AXE, GARRISON, FLESH)
                EMOTE_STATE_CUSTOMSPELL02                      = 615,
                EMOTE_STATE_READ_AND_SIT                       = 616,
                EMOTE_STATE_READYSPELLDIRECTED                 = 617,
                EMOTE_STATE_PARRY_UNARMED                      = 619,
                EMOTE_STATE_STATE_BLOCK_SHIELD                 = 620,
                EMOTE_STATE_STATE_SIT_GROUND                   = 621,
                EMOTE_STATE_ONESHOT_MOUNT_SPECIAL              = 628,
                EMOTE_STATE_DRAGONS_PITHOVER                   = 629,
                EMOTE_STATE_EMOTE_SETTLE                       = 635,
                EMOTE_STATE_ONESHOT_SETTLE                     = 636,
                EMOTE_STATE_STATE_ATTACK_UNARMED_STILL         = 638,
                EMOTE_STATE_STATE_READ_BOOK_AND_TALK           = 641,
                EMOTE_STATE_ONESHOT_SLAM                       = 642,
                EMOTE_STATE_ONESHOT_GRABTHROWN                 = 643,
                EMOTE_STATE_ONESHOT_READYSPELLDIRECTED_NOSOUND = 644,
                EMOTE_STATE_STATE_READYSPELLOMNI_WITH_SOUND    = 645,
                EMOTE_STATE_ONESHOT_TALK_BARSERVER             = 646,
                EMOTE_STATE_ONESHOT_WAVE_BARSERVER             = 647,
                EMOTE_STATE_STATE_WORK_MINING                  = 648,
                EMOTE_STATE_STATE_READY2HL_ALLOW_MOVEMENT      = 654,
                EMOTE_STATE_STATE_USESTANDING_NOSHEATHE        = 655,
                EMOTE_STATE_ONESHOT_WORK                       = 657,
                EMOTE_STATE_STATE_HOLD_THROWN                  = 658,
                EMOTE_STATE_ONESHOT_CANNIBALIZE                = 659,
                EMOTE_STATE_ONESHOT_NO                         = 661,
                EMOTE_STATE_ONESHOT_ATTACKUNARMED_NOFLAGS      = 662,
                EMOTE_STATE_STATE_READYGLV                     = 663,
                EMOTE_STATE_ONESHOT_COMBATABILITYGLV01         = 664,
                EMOTE_STATE_ONESHOT_COMBATABILITYGLVOFF01      = 665,
                EMOTE_STATE_ONESHOT_COMBATABILITYGLVBIG02      = 666,
                EMOTE_STATE_ONESHOT_PARRYGLV                   = 667,
                EMOTE_STATE_STATE_WORK_MININGNOMOVEMENT        = 668,
                EMOTE_STATE_ONESHOT_TALK_NOSHEATHE             = 669,
                EMOTE_STATE_ONESHOT_STAND_VAR3                 = 671,
                EMOTE_STATE_STATE_KNEEL                        = 672,
                EMOTE_STATE_ONESHOT_CUSTOM0                    = 673,
                EMOTE_STATE_ONESHOT_CUSTOM1                    = 674,
                EMOTE_STATE_ONESHOT_CUSTOM2                    = 675,
                EMOTE_STATE_ONESHOT_CUSTOM3                    = 676,
                EMOTE_STATE_STATE_FLY_READY_UNARMED            = 677,
                EMOTE_STATE_ONESHOT_CHEER_FORTHEALLIANCE       = 679,
                EMOTE_STATE_ONESHOT_CHEER_FORTHEHORDE          = 680,
                EMOTE_STATE_UNKLEGION                          = 689, ///< No name in DB2
                EMOTE_STATE_ONESHOT_STAND_VAR4                 = 690,
                EMOTE_STATE_ONESHOT_FLYEMOTEEXCLAMATION        = 691,
                EMOTE_STATE_UNKLEGION_1                        = 696, ///< No name in DB2
                EMOTE_STATE_UNKLEGION_2                        = 699, ///< No name in DB2
                EMOTE_STATE_EMOTE_EAT                          = 700,
                EMOTE_STATE_UNKLEGION_3                        = 701, ///< No name in DB2
                EMOTE_STATE_MONKHEAL_CHANNELOMNI               = 705,
                EMOTE_STATE_MONKDEFENSE_READYUNARMED           = 706,
                EMOTE_ONESHOT_STAND                            = 707,
                EMOTE_STATE_UNKLEGION_4                        = 708, ///< No name in DB2
                EMOTE_STATE_MONK2HLIDLE                        = 712,
                EMOTE_STATE_WAPERCH                            = 713,
                EMOTE_STATE_WAGUARDSTAND01                     = 714,
                EMOTE_STATE_READ_AND_SIT_CHAIR_MED             = 715,
                EMOTE_STATE_WAGUARDSTAND02                     = 716,
                EMOTE_STATE_WAGUARDSTAND03                     = 717,
                EMOTE_STATE_WAGUARDSTAND04                     = 718,
                EMOTE_STATE_WACHANT02                          = 719,
                EMOTE_STATE_WALEAN01                           = 720,
                EMOTE_STATE_DRUNKWALK                          = 721,
                EMOTE_STATE_WASCRUBBING                        = 722,
                EMOTE_STATE_WACHANT01                          = 723,
                EMOTE_STATE_WACHANT03                          = 724,
                EMOTE_STATE_WASUMMON01                         = 725,
                EMOTE_STATE_WATRANCE01                         = 726,
                EMOTE_STATE_CUSTOMSPELL05                      = 727,
                EMOTE_STATE_WAHAMMERLOOP                       = 728,
                EMOTE_STATE_WABOUND01                          = 729,
                EMOTE_STATE_WABOUND02                          = 730,
                EMOTE_STATE_WASACKHOLD                         = 731,
                EMOTE_STATE_WASIT01                            = 732,
                EMOTE_STATE_WAROWINGSTANDLEFT                  = 733,
                EMOTE_STATE_WAROWINGSTANDRIGHT                 = 734,
                EMOTE_STATE_LOOT_BITE_SOUND                    = 735,
                EMOTE_ONESHOT_WASUMMON01                       = 736,
                EMOTE_ONESHOT_STAND_VAR2_ULTRA                 = 737,
                EMOTE_ONESHOT_FALCONEER_START                  = 738,
                EMOTE_STATE_FALCONEER_LOOP                     = 739,
                EMOTE_ONESHOT_FALCONEER_END                    = 740,
                EMOTE_STATE_WAPERCH_NOINTERACT                 = 741,
                EMOTE_ONESHOT_WASTANDDRINK                     = 742,
                EMOTE_STATE_WALEAN02                           = 743,
                EMOTE_ONESHOT_READ_END                         = 744,
                EMOTE_STATE_WAGUARDSTAND04_ALLOW_MOVEMENT      = 745,
                EMOTE_STATE_READYCROSSBOW                      = 747,
                EMOTE_ONESHOT_WASTANDDRINK_NOSHEAT             = 748,
                EMOTE_STATE_WAHANG01                           = 749,
                EMOTE_STATE_WABEGGARSTAND                      = 750,
                EMOTE_STATE_WADRUNKSTAND                       = 751,
                EMOTE_ONESHOT_WACRIERTALK                      = 753,
                EMOTE_STATE_HOLD_CROSSBOW                      = 754,
                EMOTE_STATE_WASIT02                            = 757,
                EMOTE_STATE_WACRANKSTAND                       = 761,
                EMOTE_ONESHOT_READ_START                       = 762,
                EMOTE_ONESHOT_READ_LOOP                        = 763,
                EMOTE_ONESHOT_WADRUNKDRINK                     = 765,
                EMOTE_STATE_SIT_CHAIR_MED_EAT                  = 766,
                EMOTE_STATE_KNEEL_COPY                         = 767,
                EMOTE_STATE_WORK_CHOPMEAT_NOSHEATHE_CLEAVER    = 868,
                EMOTE_UNK_BFA_1                                = 869, ///< no name
                EMOTE_ONESHOT_BARPATRON_POINT                  = 870,
                EMOTE_STATE_STAND_NOSOUND                      = 871,
                EMOTE_STATE_MOUNT_FLIGHT_IDLE_NOSOUND          = 872,
                EMOTE_STATE_USESTANDING_LOOP_SKINNING          = 873,
                EMOTE_ONESHOT_VEHICLEGRAB                      = 874,
                EMOTE_STATE_USESTANDING_LOOP_JEWELCRAFTING     = 875,
                EMOTE_STATE_BARPATRON_STAND                    = 876,
                EMOTE_ONESHOT_WABEGGARPOINT                    = 877,
                EMOTE_STATE_WACRIERSTAND01                     = 878,
                EMOTE_ONESHOT_WABEGGARBEG                      = 879,
                EMOTE_STATE_WABOATWHEELSTAND                   = 880,
                EMOTE_STATE_WASIT03                            = 882,
                EMOTE_STATE_BARSWEEP_STAND                     = 883,
                EMOTE_STATE_WAGUARDSTAND03_NO_INTERUPT         = 884,
                EMOTE_STATE_WAGUARDSTAND02_NO_INTERUPT         = 885,
                EMOTE_STATE_BARTENDSTAND                       = 886,
                EMOTE_STATE_WAHAMMERLOOP_KULTIRAN              = 887, ///< Kul Tiran Hammer & Nail Sound
            };

            public static uint GetEmoteIdFromLine(string line)
            {
                Regex emoteRegex = new Regex(@"EmoteID:{1}\s{1}\d+");
                if (emoteRegex.IsMatch(line))
                    return Convert.ToUInt32(emoteRegex.Match(line).ToString().Replace("EmoteID: ", ""));

                return 0;
            }

            public static EmotePacket ParseEmotePacket(string[] lines, Packet packet)
            {
                EmotePacket emotePacket = new EmotePacket(packet.type, packet.time, packet.number);

                Parallel.For(packet.startIndex, packet.endIndex, x =>
                {
                    if (LineGetters.GetGuidFromLine(lines[x], emoteGuid: true) != "")
                        emotePacket.guid = LineGetters.GetGuidFromLine(lines[x], emoteGuid: true);

                    else if(GetEmoteIdFromLine(lines[x]) != 0)
                        emotePacket.emoteId = GetEmoteIdFromLine(lines[x]);
                });

                return emotePacket;
            }
        }

        [Serializable]
        public class AuraUpdatePacket : Packet
        {
            public uint? slot;
            public uint spellId;
            public UpdateType updateType;
            public uint duration;
            public long linkedPacketNumber;

            public enum UpdateType
            {
                AddAura    = 1,
                RemoveAura = 2
            }

            public AuraUpdatePacket(PacketTypes type, TimeSpan time, long number) : base(type, time, number) { }

            public static uint? GetAuraSlotFromLine(string line)
            {
                Regex slotRegex = new Regex(@"Slot:{1}\s{1}\d+");
                if (slotRegex.IsMatch(line))
                    return Convert.ToUInt32(slotRegex.Match(line).ToString().Replace("Slot: ", ""));

                return null;
            }

            public static bool? GetHasAuraFromLine(string line)
            {
                Regex hasAuraRegex = new Regex(@"HasAura:{1}\s{1}\w+");
                if (hasAuraRegex.IsMatch(line))
                    return hasAuraRegex.Match(line).ToString().Replace("HasAura: ", "") == "True";

                return null;
            }

            public static uint GetDurationFromLine(string line)
            {
                Regex slotRegex = new Regex(@"Duration:{1}\s{1}\d+");
                if (slotRegex.IsMatch(line))
                    return Convert.ToUInt32(slotRegex.Match(line).ToString().Replace("Duration: ", ""));

                return 0;
            }

            public static bool IsLineValidForAuraUpdateParsing(string line)
            {
                if (line == null)
                    return false;

                if (line == "")
                    return false;

                if (line.Contains("Slot:"))
                    return false;

                return true;
            }

            public static IEnumerable<AuraUpdatePacket> ParseAuraUpdatePacket(string[] lines, Packet packet)
            {
                SynchronizedCollection<AuraUpdatePacket> aurasCollection = new SynchronizedCollection<AuraUpdatePacket>();

                string guid = LineGetters.GetGuidFromLine(lines[packet.indexes.Last().Value], unitGuid: true);

                if (guid != "")
                {
                    foreach(var itr in packet.indexes)
                    {
                        AuraUpdatePacket auraUpdatePacket = new AuraUpdatePacket(PacketTypes.SMSG_AURA_UPDATE, packet.time, packet.number);
                        auraUpdatePacket.guid = guid;

                        Parallel.For(itr.Key, itr.Value, index =>
                        {
                            if (GetAuraSlotFromLine(lines[index]) != null)
                                auraUpdatePacket.slot = GetAuraSlotFromLine(lines[index]);

                            else if (GetHasAuraFromLine(lines[index]) != null)
                                auraUpdatePacket.updateType = GetHasAuraFromLine(lines[index]).ToString() == "True" ? UpdateType.AddAura : UpdateType.RemoveAura;

                            else if (SpellStartPacket.GetSpellIdFromLine(lines[index]) != 0)
                                auraUpdatePacket.spellId = SpellStartPacket.GetSpellIdFromLine(lines[index]);

                            else if (GetDurationFromLine(lines[index]) != 0)
                                auraUpdatePacket.duration = GetDurationFromLine(lines[index]);
                        });

                        aurasCollection.Add(auraUpdatePacket);
                    }
                }

                return aurasCollection;
            }

            public static void FilterAuraPacketsForCreature(List<object> creaturePackets)
            {
                List<object> sortedAuraPackestList = creaturePackets.Where(x => x.GetType() == typeof(AuraUpdatePacket)).OrderBy(x => ((Packet)x).number).ToList();
                List<object> outputAuraRemovePacketsList = new List<object>();

                for (int i = 0; i < sortedAuraPackestList.Count(); i++)
                {
                    AuraUpdatePacket addAuraPacket = (AuraUpdatePacket)sortedAuraPackestList[i];

                    if (addAuraPacket.updateType == UpdateType.AddAura)
                    {
                        for (int j = i + 1; j < sortedAuraPackestList.Count(); j++)
                        {
                            AuraUpdatePacket removeAuraPacket = (AuraUpdatePacket)sortedAuraPackestList[j];

                            if (removeAuraPacket.updateType == UpdateType.RemoveAura && removeAuraPacket.slot == addAuraPacket.slot)
                            {
                                removeAuraPacket.linkedPacketNumber = addAuraPacket.number;
                                removeAuraPacket.spellId = addAuraPacket.spellId;
                                break;
                            }
                        }
                    }
                }

                for (int i = 0; i < sortedAuraPackestList.Count(); i++)
                {
                    AuraUpdatePacket addAuraPacket = (AuraUpdatePacket)sortedAuraPackestList[i];
                    AuraUpdatePacket removeAuraPacket = (AuraUpdatePacket)sortedAuraPackestList.FirstOrDefault(x => ((AuraUpdatePacket)x).linkedPacketNumber == addAuraPacket.number);
                    if (removeAuraPacket != null && (addAuraPacket.duration == 0 || addAuraPacket.duration > (uint)(removeAuraPacket.time - addAuraPacket.time).TotalMilliseconds))
                    {
                        outputAuraRemovePacketsList.Add(removeAuraPacket);
                    }
                }

                creaturePackets.RemoveAll(x => x.GetType() == typeof(AuraUpdatePacket));
                creaturePackets.AddRange(outputAuraRemovePacketsList);
            }
        }

        [Serializable]
        public class SetAiAnimKit : Packet
        {
            public uint? AiAnimKitId;

            public SetAiAnimKit(PacketTypes type, TimeSpan time, long number) : base(type, time, number) { }

            public static uint? GetAiAnimKitIdFromLine(string line)
            {
                Regex animKitRegex = new Regex(@"AiAnimKitId:{1}\s+\d+");
                if (animKitRegex.IsMatch(line))
                    return Convert.ToUInt32(animKitRegex.Match(line).ToString().Replace("AiAnimKitId: ", ""));

                return null;
            }

            public static SetAiAnimKit ParseSetAiAnimKitPacket(string[] lines, Packet packet)
            {
                SetAiAnimKit animKitPacket = new SetAiAnimKit(packet.type, packet.time, packet.number);

                if (LineGetters.IsCreatureLine(lines[packet.startIndex + 1]))
                {
                    Parallel.For(packet.startIndex, packet.endIndex, x =>
                    {
                        if (LineGetters.GetGuidFromLine(lines[x], oneShotAnimKitGuid: true) != "")
                            animKitPacket.guid = LineGetters.GetGuidFromLine(lines[x], oneShotAnimKitGuid: true);

                        else if (GetAiAnimKitIdFromLine(lines[x]) != null)
                            animKitPacket.AiAnimKitId = GetAiAnimKitIdFromLine(lines[x]);
                    });
                }

                return animKitPacket;
            }
        }

        [Serializable]
        public class PlaySpellVisualKit : Packet
        {
            public uint KitRecId;
            public uint? KitType;
            public uint? Duration;

            public PlaySpellVisualKit(PacketTypes type, TimeSpan time, long number) : base(type, time, number) { }

            public static uint GetKitRecIdFromLine(string line)
            {
                Regex kitRecRegex = new Regex(@"KitRecID:{1}\s+\d+");
                if (kitRecRegex.IsMatch(line))
                    return Convert.ToUInt32(kitRecRegex.Match(line).ToString().Replace("KitRecID: ", ""));

                return 0;
            }

            public static uint? GetKitTypeFromLine(string line)
            {
                Regex kitTypeRegex = new Regex(@"KitType:{1}\s+\d+");
                if (kitTypeRegex.IsMatch(line))
                    return Convert.ToUInt32(kitTypeRegex.Match(line).ToString().Replace("KitType: ", ""));

                return null;
            }

            public static uint? GetDurationFromLine(string line)
            {
                Regex durationRegexRegex = new Regex(@"Duration:{1}\s+\d+");
                if (durationRegexRegex.IsMatch(line))
                    return Convert.ToUInt32(durationRegexRegex.Match(line).ToString().Replace("Duration: ", ""));

                return null;
            }

            public static PlaySpellVisualKit ParsePlaySpellVisualKitPacket(string[] lines, Packet packet)
            {
                PlaySpellVisualKit spellVisualPacket = new PlaySpellVisualKit(packet.type, packet.time, packet.number);

                if (LineGetters.IsCreatureLine(lines[packet.startIndex + 1]))
                {
                    Parallel.For(packet.startIndex, packet.endIndex, x =>
                    {
                        if (LineGetters.GetGuidFromLine(lines[x], oneShotAnimKitGuid: true) != "")
                            spellVisualPacket.guid = LineGetters.GetGuidFromLine(lines[x], oneShotAnimKitGuid: true);

                        else if (GetKitRecIdFromLine(lines[x]) != 0)
                            spellVisualPacket.KitRecId = GetKitRecIdFromLine(lines[x]);

                        else if (GetKitTypeFromLine(lines[x]) != null)
                            spellVisualPacket.KitType = GetKitTypeFromLine(lines[x]);

                        else if (GetDurationFromLine(lines[x]) != null)
                            spellVisualPacket.Duration = GetDurationFromLine(lines[x]);
                    });
                }

                return spellVisualPacket;
            }
        }

        [Serializable]
        public class PlayObjectSoundPacket : Packet
        {
            public uint SoundId;
            public Position position;

            public PlayObjectSoundPacket(PacketTypes type, TimeSpan time, long number) : base(type, time, number) { }

            public static uint GetSoundKitIdFromLine(string line)
            {
                Regex soundIdRegexRegex = new Regex(@"SoundKitId:{1}\s+\d+");
                if (soundIdRegexRegex.IsMatch(line))
                    return Convert.ToUInt32(soundIdRegexRegex.Match(line).ToString().Replace("SoundKitId: ", ""));

                return 0;
            }

            public static PlayObjectSoundPacket ParsePlayObjectSoundPacketPacket(string[] lines, Packet packet)
            {
                PlayObjectSoundPacket soundPacket = new PlayObjectSoundPacket(packet.type, packet.time, packet.number);

                Parallel.For(packet.startIndex, packet.endIndex, x =>
                {
                    if (LineGetters.GetGuidFromLine(lines[x], sourceObjectGuid: true) != "")
                        soundPacket.guid = LineGetters.GetGuidFromLine(lines[x], sourceObjectGuid: true);

                    else if (GetSoundKitIdFromLine(lines[x]) != 0)
                        soundPacket.SoundId = GetSoundKitIdFromLine(lines[x]);

                    else if (MonsterMovePacket.GetPositionFromLine(lines[x]).IsValid())
                        soundPacket.position = MonsterMovePacket.GetPositionFromLine(lines[x]);
                });

                return soundPacket;
            }
        }

        [Serializable]
        public class PlaySpellVisual : Packet
        {
            public bool targetIsPlayer;
            public Position targetPosition;
            public uint SpellVisualId;
            public uint? TravelSpeed;
            public uint? LaunchDelay;
            public bool? SpeedAsTime;

            public PlaySpellVisual(PacketTypes type, TimeSpan time, long number) : base(type, time, number) { }

            public static Position GetTargetPositionFromLine(string line)
            {
                Position position = new Position();

                Regex xyzRegex = new Regex(@"TargetPosition:{1}\s{1}X:{1}.+");
                if (xyzRegex.IsMatch(line))
                {
                    string[] splittedLine = xyzRegex.Match(line).ToString().Replace("TargetPosition: X: ", "").Split(' ');

                    position.x = float.Parse(splittedLine[0], CultureInfo.InvariantCulture.NumberFormat);
                    position.y = float.Parse(splittedLine[2], CultureInfo.InvariantCulture.NumberFormat);
                    position.z = float.Parse(splittedLine[4], CultureInfo.InvariantCulture.NumberFormat);
                }

                return position;
            }

            public static uint GetSpellVisualIdFromLine(string line)
            {
                Regex spellVisualIdRegex = new Regex(@"SpellVisualID:{1}\s+\d+");
                if (spellVisualIdRegex.IsMatch(line))
                    return Convert.ToUInt32(spellVisualIdRegex.Match(line).ToString().Replace("SpellVisualID: ", ""));

                return 0;
            }

            public static uint? GetTravelSpeedFromLine(string line)
            {
                Regex travelSpeedRegex = new Regex(@"TravelSpeed:{1}\s+\d+");
                if (travelSpeedRegex.IsMatch(line))
                    return Convert.ToUInt32(travelSpeedRegex.Match(line).ToString().Replace("TravelSpeed: ", ""));

                return null;
            }

            public static uint? GetLaunchDelayFromLine(string line)
            {
                Regex launchDelayRegex = new Regex(@"LaunchDelay:{1}\s+\d+");
                if (launchDelayRegex.IsMatch(line))
                    return Convert.ToUInt32(launchDelayRegex.Match(line).ToString().Replace("LaunchDelay: ", ""));

                return null;
            }

            public static bool? GetSpeedAsTimeFromLine(string line)
            {
                Regex durationRegexRegex = new Regex(@"SpeedAsTime:{1}\s{1}\w+");
                if (durationRegexRegex.IsMatch(line))
                    return !line.Contains("False");

                return null;
            }

            public static PlaySpellVisual ParsePlaySpellVisualPacket(string[] lines, Packet packet)
            {
                PlaySpellVisual spellVisualPacket = new PlaySpellVisual(packet.type, packet.time, packet.number);

                if (LineGetters.IsCreatureLine(lines[packet.startIndex + 1]))
                {
                    Parallel.For(packet.startIndex, packet.endIndex, x =>
                    {
                        if (lines[x].Contains("Target: TypeName:"))
                        {
                            spellVisualPacket.targetIsPlayer = lines[x].Contains("Target: TypeName: Player;");
                        }

                        else if (GetTargetPositionFromLine(lines[x]).IsValid())
                            spellVisualPacket.targetPosition = GetTargetPositionFromLine(lines[x]);

                        else if (LineGetters.GetGuidFromLine(lines[x], sourceGuid: true) != "")
                            spellVisualPacket.guid = LineGetters.GetGuidFromLine(lines[x], sourceGuid: true);

                        else if (GetSpellVisualIdFromLine(lines[x]) != 0)
                            spellVisualPacket.SpellVisualId = GetSpellVisualIdFromLine(lines[x]);

                        else if (GetTravelSpeedFromLine(lines[x]) != null)
                            spellVisualPacket.TravelSpeed = GetTravelSpeedFromLine(lines[x]);

                        else if (GetLaunchDelayFromLine(lines[x]) != null)
                            spellVisualPacket.LaunchDelay = GetLaunchDelayFromLine(lines[x]);

                        else if (GetSpeedAsTimeFromLine(lines[x]) != null)
                            spellVisualPacket.SpeedAsTime = GetSpeedAsTimeFromLine(lines[x]);
                    });
                }

                return spellVisualPacket;
            }
        }
    }
}
