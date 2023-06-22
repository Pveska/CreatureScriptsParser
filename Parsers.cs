using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using static CreatureScriptsParser.Packets;

namespace CreatureScriptsParser
{
    public class Parsers
    {
        private MainForm mainForm;
        public Parsers(MainForm mainForm) { this.mainForm = mainForm; }

        private void ChangeButtonsState(bool state)
        {
            mainForm.toolStripButton_ImportSniff.Enabled = state;
            mainForm.toolStripTextBox_CreatureGuid.Enabled = state;
            mainForm.toolStripButton_Search.Enabled = state;
            mainForm.Update();
        }

        public List<object> GetPacketsFromSniffFile(string fileName, bool createDataFile)
        {
            ChangeButtonsState(false);
            mainForm.toolStripStatusLabel_FileStatus.Text = "Current status: Reading all lines...";
            mainForm.Update();
            string[] lines = File.ReadAllLines(fileName);
            List<object> packetsList = GetDataFromPackets(lines, GetPacketIndexesFromLines(lines));

            if (createDataFile)
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                using (FileStream fileStream = new FileStream(fileName.Replace("_parsed.txt", "_packets.dat"), FileMode.OpenOrCreate))
                {
                    binaryFormatter.Serialize(fileStream, packetsList);
                }
            }

            ChangeButtonsState(true);
            return packetsList;
        }

        public List<object> GetPacketsFromDataFile(string fileName)
        {
            ChangeButtonsState(false);
            mainForm.toolStripStatusLabel_FileStatus.Text = "Current status: Getting packets from data file...";

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            List<object> packetsList;

            using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                packetsList = (List<object>)binaryFormatter.Deserialize(fileStream);
            }

            ChangeButtonsState(true);
            return packetsList;
        }

        private SynchronizedCollection<Packet> GetPacketIndexesFromLines(string[] lines)
        {
            mainForm.toolStripStatusLabel_FileStatus.Text = "Current status: Getting packet indexes...";
            mainForm.Update();

            SynchronizedCollection<Packet> packetIndexesDictionary = new SynchronizedCollection<Packet>();

            Parallel.For(0, lines.Length, index =>
            {
                foreach (var packetName in Enum.GetNames(typeof(Packet.PacketTypes)))
                {
                    if (lines[index].Contains(packetName))
                    {
                        long i = index;

                        Enum.TryParse(packetName, out Packet.PacketTypes packetType);

                        switch (packetType)
                        {
                            case Packet.PacketTypes.SMSG_UPDATE_OBJECT:
                            {
                                Dictionary<long, long> indexesDictionary = new Dictionary<long, long>();
                                bool hasDestroyObjects = false;

                                do
                                {
                                    i++;

                                    if (!hasDestroyObjects && lines[i].Contains("DestroyedObjCount") && !lines[i].Contains("DestroyedObjCount : 0"))
                                    {
                                        hasDestroyObjects = true;
                                    }

                                    if (lines[i].Contains("UpdateType:") || ((lines[i].Contains("DestroyedObjCount") || lines[i].Contains("DataSize")) && hasDestroyObjects) || lines[i] == "")
                                    {
                                        if (indexesDictionary.Count() == 0)
                                        {
                                            indexesDictionary.Add(i, 0);
                                        }
                                        else if (indexesDictionary.Last().Value == 0)
                                        {
                                            indexesDictionary[indexesDictionary.Last().Key] = i;

                                            if (lines[i] != "" && !lines[i].Contains("DataSize"))
                                            {
                                                indexesDictionary.Add(i, 0);
                                            }
                                        }
                                        else
                                        {
                                            indexesDictionary.Add(i, 0);
                                        }
                                    }
                                }
                                while (lines[i] != "");

                                packetIndexesDictionary.Add(new Packet(packetType, LineGetters.GetTimeSpanFromLine(lines[index]), LineGetters.GetPacketNumberFromLine(lines[index]), indexesDictionary));
                                break;
                            }
                            case Packet.PacketTypes.SMSG_AURA_UPDATE:
                            {
                                Dictionary<long, long> indexesDictionary = new Dictionary<long, long>();

                                do
                                {
                                    i++;

                                    if (lines[i].Contains("Slot") || lines[i] == "")
                                    {
                                        if (indexesDictionary.Count() == 0)
                                        {
                                            indexesDictionary.Add(i, 0);
                                        }
                                        else if (indexesDictionary.Last().Value == 0)
                                        {
                                            indexesDictionary[indexesDictionary.Last().Key] = i - 1;

                                            if (lines[i] != "")
                                            {
                                                indexesDictionary.Add(i, 0);
                                            }
                                        }
                                    }
                                }
                                while (lines[i] != "");

                                packetIndexesDictionary.Add(new Packet(packetType, LineGetters.GetTimeSpanFromLine(lines[index]), LineGetters.GetPacketNumberFromLine(lines[index]), indexesDictionary));

                                break;
                            }
                            default:
                            {
                                do
                                {
                                    i++;
                                }
                                while (lines[i] != "");

                                packetIndexesDictionary.Add(new Packet(packetType, LineGetters.GetTimeSpanFromLine(lines[index]), LineGetters.GetPacketNumberFromLine(lines[index]), index, i));
                                break;
                            }
                        }

                        break;
                    }
                }
            });

            return packetIndexesDictionary;
        }

        private List<object> GetDataFromPackets(string[] lines, SynchronizedCollection<Packet> packets)
        {
            mainForm.toolStripStatusLabel_FileStatus.Text = "Current status: Getting data from packets...";
            mainForm.Update();

            List<object> packetsList = new List<object>();

            foreach(Packet packet in packets)
            {
                switch (packet.type)
                {
                    case Packet.PacketTypes.SMSG_UPDATE_OBJECT:
                    {
                        foreach(var updatePacket in UpdateObjectPacket.ParseObjectUpdatePacket(lines, packet))
                        {
                            packetsList.Add(updatePacket);
                        }

                        break;
                    }
                    case Packet.PacketTypes.SMSG_SPELL_START:
                    case Packet.PacketTypes.SMSG_SPELL_GO:
                    {
                        SpellStartPacket spellPacket = SpellStartPacket.ParseSpellStartPacket(lines, packet);

                        if (spellPacket.guid != "")
                        {
                            packetsList.Add(spellPacket);
                        }

                        break;
                    }
                    case Packet.PacketTypes.SMSG_ON_MONSTER_MOVE:
                    {
                        MonsterMovePacket monsterMovePacket = MonsterMovePacket.ParseMovementPacket(lines, packet);

                        if (monsterMovePacket.guid != "" && (monsterMovePacket.waypoints.Count() != 0 || monsterMovePacket.HasJump() ||
                        monsterMovePacket.HasOrientation() || monsterMovePacket.hasFacingToPlayer))
                        {
                            packetsList.Add(monsterMovePacket);
                        }

                        break;
                    }
                    case Packet.PacketTypes.SMSG_PLAY_ONE_SHOT_ANIM_KIT:
                    {
                        PlayOneShotAnimKit playOneShotAnimKitPacket = PlayOneShotAnimKit.ParsePlayOneShotAnimKitPacket(lines, packet);

                        if (playOneShotAnimKitPacket.guid != "")
                        {
                            packetsList.Add(playOneShotAnimKitPacket);
                        }

                        break;
                    }
                    case Packet.PacketTypes.SMSG_CHAT:
                    {
                        ChatPacket chatPacket = ChatPacket.ParseChatPacket(lines, packet);

                        if (chatPacket.guid != "")
                        {
                            packetsList.Add(chatPacket);
                        }

                        break;
                    }
                    case Packet.PacketTypes.SMSG_EMOTE:
                    {
                        EmotePacket emotePacket = EmotePacket.ParseEmotePacket(lines, packet);

                        if (emotePacket.guid != "")
                        {
                            packetsList.Add(emotePacket);
                        }

                        break;
                    }
                    case Packet.PacketTypes.SMSG_AURA_UPDATE:
                    {
                        foreach(var auraPacket in AuraUpdatePacket.ParseAuraUpdatePacket(lines, packet))
                        {
                            packetsList.Add(auraPacket);
                        }

                        break;
                    }
                    case Packet.PacketTypes.SMSG_SET_AI_ANIM_KIT:
                    {
                        SetAiAnimKit setAiAnimKitPacket = SetAiAnimKit.ParseSetAiAnimKitPacket(lines, packet);

                        if (setAiAnimKitPacket.guid != "")
                        {
                            packetsList.Add(setAiAnimKitPacket);
                        }

                        break;
                    }
                    case Packet.PacketTypes.SMSG_PLAY_SPELL_VISUAL_KIT:
                    {
                        PlaySpellVisualKit playSpellVisualKitPacket = PlaySpellVisualKit.ParsePlaySpellVisualKitPacket(lines, packet);

                        if (playSpellVisualKitPacket.guid != "")
                        {
                            packetsList.Add(playSpellVisualKitPacket);
                        }

                        break;
                    }
                    case Packet.PacketTypes.SMSG_PLAY_OBJECT_SOUND:
                    {
                        PlayObjectSoundPacket playObjectSoundPacket = PlayObjectSoundPacket.ParsePlayObjectSoundPacketPacket(lines, packet);

                        if (playObjectSoundPacket.guid != "")
                        {
                            packetsList.Add(playObjectSoundPacket);
                        }

                        break;
                    }
                    case Packet.PacketTypes.SMSG_PLAY_SPELL_VISUAL:
                    {
                        PlaySpellVisual playSpellVisualPacket = PlaySpellVisual.ParsePlaySpellVisualPacket(lines, packet);

                        if (playSpellVisualPacket.guid != "")
                        {
                            packetsList.Add(playSpellVisualPacket);
                        }

                        break;
                    }
                    default:
                        break;
                }
            }

            SpellStartPacket.FilterSpellPackets(packetsList);

            return packetsList;
        }
    }
}
