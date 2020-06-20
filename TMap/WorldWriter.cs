using System;
using System.Collections.Generic;
using System.IO;
using TMap.Data;
using static TMap.Data.World.BossIndexes;
using static TMap.Data.World.NpcIndexes;
using static TMap.Data.World.InvasionIndexes;

namespace TMap
{
    public static class WorldWriter
    {
        private static int WriteFileFormatHeader(BinaryWriter s, World w)
        {
            s.Write(w.Version);
            s.Write("relogic".ToCharArray());
            s.Write((byte) 2);
            s.Write(w.Revision);
            s.Write(0L);
            s.Write((short) w.ArrayOfPointers.Length);
            foreach (int _ in w.ArrayOfPointers)
                s.Write(0);
            s.Write((short) w.TileFrameImportant.Length);
            byte b = 0;
            byte b2 = 1;
            foreach (bool tfi in w.TileFrameImportant)
            {
                if (tfi)
                {
                    b = (byte) (b | b2);
                }

                if (b2 == 128)
                {
                    s.Write(b);
                    b = 0;
                    b2 = 1;
                }
                else
                {
                    b2 = (byte) (b2 << 1);
                }
            }

            if (b2 != 1)
                s.Write(b);

            s.Flush();

            return (int) s.BaseStream.Position;
        }

        private static int WriteWorldHeader(BinaryWriter s, World w)
        {
            s.Write(w.Name);
            s.Write(w.Seed);
            s.Write(w.WorldGenVer);
            s.Write(w.UniqueId.ToByteArray());
            s.Write(w.WorldId);
            s.Write(w.LeftWorld);
            s.Write(w.RightWorld);
            s.Write(w.TopWorld);
            s.Write(w.BottomWorld);
            s.Write(w.MaxTilesY);
            s.Write(w.MaxTilesX);
            s.Write(w.GameMode);
            s.Write(w.DrunkWorld);
            s.Write(w.GitGudWorld);
            s.Write(w.CreationTime.ToBinary());
            s.Write(w.MoonType);

            #region graphic mess

            s.Write(w.TreeX[0]);
            s.Write(w.TreeX[1]);
            s.Write(w.TreeX[2]);
            s.Write(w.TreeStyle[0]);
            s.Write(w.TreeStyle[1]);
            s.Write(w.TreeStyle[2]);
            s.Write(w.TreeStyle[3]);
            s.Write(w.CaveBackX[0]);
            s.Write(w.CaveBackX[1]);
            s.Write(w.CaveBackX[2]);
            s.Write(w.CaveBackStyle[0]);
            s.Write(w.CaveBackStyle[1]);
            s.Write(w.CaveBackStyle[2]);
            s.Write(w.CaveBackStyle[3]);

            s.Write(w.IceBackStyle);
            s.Write(w.JungleBackStyle);
            s.Write(w.HellBackStyle);

            #endregion

            s.Write(w.SpawnTileX);
            s.Write(w.SpawnTileY);

            s.Write(w.WorldSurface);
            s.Write(w.RockLayer);

            s.Write(w.Time);
            s.Write(w.DayTime);
            s.Write(w.MoonPhase);
            s.Write(w.BloodMoon);
            s.Write(w.Eclipse);

            s.Write(w.DungeonX);
            s.Write(w.DungeonY);

            s.Write(w.Crimson);

            for (int i = (int) EyeOfCthulhu; i <= (int) KingSlime; i++)
                s.Write(w.DownedBosses[i]);

            for (int i = (int) Goblin; i <= (int) Mechanic; i++)
                s.Write(w.SavedNpcs[i]);

            for (int i = (int)Goblins; i <= (int)Pirates; i++)
                s.Write(w.DownedInvasions[i]);

            s.Write(w.ShadowOrb);
            s.Write(w.SpawnMeteor);
            s.Write(w.ShadowOrbCount);
            s.Write(w.AltarCount);

            s.Write(w.HardMode);

            s.Write(w.InvasionDelay);
            s.Write(w.InvasionSize);
            s.Write(w.InvasionType);
            s.Write(w.InvasionX);

            s.Write(w.SlimeRainTime);

            s.Write(w.SundialCooldown);

            s.Write(w.Raining);
            s.Write(w.RainTime);
            s.Write(w.MaxRain);

            s.Write(w.CobaltTier);
            s.Write(w.MythrilTier);
            s.Write(w.AdamantiteTier);

            s.Write(w.TreeBg);
            s.Write(w.CorruptBg);
            s.Write(w.JungleBg);
            s.Write(w.SnowBg);
            s.Write(w.HallowBg);
            s.Write(w.CrimsonBg);
            s.Write(w.DesertBg);
            s.Write(w.OceanBg);

            s.Write(w.CloudBgActive);
            s.Write(w.NumClouds);

            s.Write(w.WindSpeedTarget);

            s.Write(w.AnglerWhoFinishedToday.Length);
            foreach (string finishedAnglerQuestToday in w.AnglerWhoFinishedToday)
                s.Write(finishedAnglerQuestToday);

            s.Write(w.SavedNpcs[(int) Angler]);

            s.Write(w.AnglerQuest);

            for (int i = (int) Stylist; i <= (int) Golfer; i++)
            {
                s.Write(w.SavedNpcs[i]);
            }

            s.Write(w.InvasionSizeStart);

            s.Write(w.CultistDelay);

            s.Write((short) w.KillCounts.Length);
            foreach (int amountKilled in w.KillCounts)
                s.Write(amountKilled);

            s.Write(w.FastForwardTime);

            s.Write(w.DownedBosses[(int) Fishron]);
            s.Write(w.DownedInvasions[(int) Martians]);
            for (int i = (int) Cultist; i <= (int) StardustPillar; i++)
                s.Write(w.DownedBosses[i]);

            s.Write(w.SolarPillarUp);
            s.Write(w.VortexPillarUp);
            s.Write(w.NebulaPillarUp);
            s.Write(w.StardustPillarUp);

            s.Write(w.LunarEventsOngoing);

            s.Write(w.ManualParty);
            s.Write(w.GenuineParty);
            s.Write(w.PartyCooldown);
            s.Write(w.CelebratingNpcs.Length);
            foreach (int npc in w.CelebratingNpcs)
                s.Write(npc);

            s.Write(w.SandStormOngoing);
            s.Write(w.SandStormTimeLeft);
            s.Write(w.SandStormSeverity);
            s.Write(w.SandStormIntendedSeverity);

            s.Write(w.SavedNpcs[(int) TavernKeep]);
            
            s.Write(w.MaxDownedDungeonDefenders2 >= 1);
            s.Write(w.MaxDownedDungeonDefenders2 >= 2);
            s.Write(w.MaxDownedDungeonDefenders2 >= 3);

            s.Write(w.MushroomBg);
            s.Write(w.UnderworldBg);
            s.Write(w.TreeBg2);
            s.Write(w.TreeBg3);
            s.Write(w.TreeBg4);

            s.Write(w.CombatBookUsed);

            s.Write(w.LanternNightCooldown);
            s.Write(w.LanternNightGenuine);
            s.Write(w.LanternNightManual);
            s.Write(w.LanternNightNextNightGenuine);

            s.Write(w.TreeTopVariations.Length);
            foreach (int treeTopVariation in w.TreeTopVariations)
                s.Write(treeTopVariation);
            
            s.Write(w.ForceHalloween);
            s.Write(w.ForceChristmas);

            s.Write(w.CopperTier);
            s.Write(w.IronTier);
            s.Write(w.SilverTier);
            s.Write(w.GoldTier);

            s.Write(w.BoughtCat);
            s.Write(w.BoughtDog);
            s.Write(w.BoughtBunny);

            s.Write(w.DownedBosses[(int) EmpressOfLight]);
            s.Write(w.DownedBosses[(int) QueenSlime]);
            
            return (int) s.BaseStream.Position;
        }

        private static int WriteWorldTiles(BinaryWriter s, World w)
        {
            for (int i = 0; i < w.Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < w.Tiles.GetLength(1); /*incrementing done in repeat thingy*/)
                {
                    Tile current = w.Tiles[i, j];

                    int repeat = 0;
                    j++;
                    while (repeat <= Int16.MaxValue && j < w.Tiles.GetLength(1))
                    {
                        if (w.Tiles[i, j] == current)
                            repeat++;
                        else
                            break;
                        j++;
                    }
                    
                    byte flags3 = 0;
                    if (current.Wall > 255)
                        flags3 |= 0x40;
                    if (current.YellowWire)
                        flags3 |= 0x20;
                    if (current.WallColor != 0)
                        flags3 |= 0x10;
                    if (current.Color != 0)
                        flags3 |= 0x08;
                    if (!current.Active)
                        flags3 |= 0x04;
                    if (current.Actuator)
                        flags3 |= 0x02;

                    byte flags2 = 0;
                    if (current.HalfBrick)
                        flags2 |= 0x10;
                    else if (current.Slope != 0)
                        flags2 |= (byte) (current.Slope + 1 << 4);
                    if (current.GreenWire)
                        flags2 |= 0x08;
                    if (current.BlueWire)
                        flags2 |= 0x04;
                    if (current.RedWire)
                        flags2 |= 0x02;
                    if (flags3 != 0)
                        flags2 |= 0x01;

                    byte flags1 = 0;
                    if (repeat > 255)
                        flags1 |= 0x80;
                    else if (repeat > 0)
                        flags1 |= 0x40;
                    if (current.Id > 255)
                        flags1 |= 0x20;
                    if (current.LiquidAmount != 0)
                    {
                        if (current.Lava)
                            flags1 |= 0x10;
                        else if (current.Honey)
                            flags1 |= 0x18;
                        else
                            flags1 |= 0x08;
                    }

                    if (current.Wall != 0)
                        flags1 |= 0x04;
                    if (current.Id != -1)
                        flags1 |= 0x02;
                    if (flags2 != 0)
                        flags1 |= 0x01;
                    
                    s.Write(flags1);
                    if (flags2 != 0)
                        s.Write(flags2);
                    if (flags3 != 0)
                        s.Write(flags3);
                    if (current.Id > 255)
                        s.Write((ushort) current.Id);
                    else if (current.Id > -1)
                        s.Write((byte) current.Id);
                    if (current.FrameX >= 0)
                    {
                        s.Write(current.FrameX);
                        s.Write(current.FrameY);
                    }

                    if (current.Color != 0)
                        s.Write(current.Color);
                    if (current.Wall > 255)
                        s.Write(current.Wall);
                    else if (current.Wall > 0)
                        s.Write((byte) current.Wall);
                    if (current.WallColor != 0)
                        s.Write(current.WallColor);
                    if (current.LiquidAmount != 0)
                        s.Write(current.LiquidAmount);
                    if (repeat > 255)
                        s.Write((ushort) repeat);
                    else if (repeat > 0)
                        s.Write((byte) repeat);

                }
            }
            
            return (int) s.BaseStream.Position;
        }

        private static int WriteChests(BinaryWriter s, World w)
        {
            s.Write((short)w.Chests.Length);
            s.Write((short)40);
            foreach (Chest c in w.Chests)
            {
                s.Write(c.X);
                s.Write(c.Y);
                s.Write(c.Name);
                foreach (Item i in c.Contents)
                {
                    if (i == null)
                        s.Write((short)0);
                    else
                    {
                        s.Write(i.Stack);
                        s.Write(i.Id);
                        s.Write(i.Prefix);
                    }
                }
            }
            
            return (int) s.BaseStream.Position;
        }

        private static int WriteSigns(BinaryWriter s, World w)
        {
            s.Write((short)w.Signs.Length);
            foreach (Sign current in w.Signs)
            {
                if (current == null)
                    continue;
                s.Write(current.Text);
                s.Write(current.X);
                s.Write(current.Y);
            }
            
            return (int) s.BaseStream.Position;
        }

        private static int WriteNpcs(BinaryWriter s, World w)
        {
            foreach (Npc current in w.CurrentNpcs)
            {
                if (current.TownNpc)
                {
                    s.Write(current.Active);
                    s.Write(current.Id);
                    s.Write(current.Name);
                    s.Write(current.X);
                    s.Write(current.Y);
                    s.Write(current.Homeless);
                    s.Write(current.HomeTileX);
                    s.Write(current.HomeTileY);
                    s.Write((byte) 0x01);
                    s.Write(current.TownNpcVariationIndex);
                }
            }

            s.Write(false);
            
            foreach (Npc current in w.CurrentNpcs)
            {
                if (!current.TownNpc)
                {
                    s.Write(current.Active);
                    s.Write(current.Id);
                    s.Write(current.X);
                    s.Write(current.Y);
                }
            }

            s.Write(false);
            
            return (int) s.BaseStream.Position;
        }

        private static int WriteTileEntities(BinaryWriter s, World w)
        {
            s.Write(w.TileEntities.Length);

            foreach (TileEntity current in w.TileEntities)
            {
                s.Write(current.Type);
                s.Write(current.Id);
                s.Write(current.X);
                s.Write(current.Y);
                
                switch (current.Type) //Ignoring Pylons (7) on purpose here, since they dont store anything extra
                {
                    case 0: // Training Dummy
                        s.Write(current.Npc);
                        break;
                    case 1: // Item Frame
                    case 4: // Weapon Rack
                    case 6: // Food Platter
                        s.Write((short)current.Items[0].Id);
                        s.Write(current.Items[0].Prefix);
                        s.Write(current.Items[0].Stack);
                        break;
                    case 2: // logic switch
                        s.Write(current.LogicType);
                        s.Write(current.Enabled);
                        break;
                    case 3: // Display doll
                        byte itemSlots = 0;
                        for (int i = 0; i < 8; i++)
                            if (current.Items[i] != null)
                                itemSlots |= (byte) (1 << i);
                        
                        byte dyeSlots = 0;
                        for (int i = 0; i < 8; i++)
                            if (current.Items[8 + i] != null)
                                itemSlots |= (byte) (1 << i);

                        s.Write(itemSlots);
                        s.Write(dyeSlots);

                        for (int i = 0; i < 16; i++)
                        {
                            if (current.Items[i] == null)
                                continue;
                            s.Write((short)current.Items[i].Id);
                            s.Write(current.Items[i].Prefix);
                            s.Write(current.Items[i].Stack);
                        }

                        break;
                    case 5: // hat rack
                        byte slots = 0;
                        for (int i = 0; i < 4; i++)
                            if (current.Items[i] != null)
                                slots |= (byte) (1 << i);

                        s.Write(slots);

                        for (int i = 0; i < 4; i++)
                        {
                            if (current.Items[i] == null)
                                continue;
                            s.Write((short)current.Items[i].Id);
                            s.Write(current.Items[i].Prefix);
                            s.Write(current.Items[i].Stack);
                        }
                        break;
                }
            }
            
            return (int) s.BaseStream.Position;
        }

        private static int WritePressurePlates(BinaryWriter s, World w)
        {
            s.Write(w.PressurePlatesX.Length);

            for (int i = 0; i < w.PressurePlatesX.Length; i++)
            {
                s.Write(w.PressurePlatesX[i]);
                s.Write(w.PressurePlatesY[i]);
            }

            return (int) s.BaseStream.Position;
        }

        private static int WriteTownManager(BinaryWriter s, World w)
        {
            s.Write(w.RoomLocations.Length);

            foreach ((int npcId, int xPos, int yPos) roomLocation in w.RoomLocations)
            {
                s.Write(roomLocation.npcId);
                s.Write(roomLocation.xPos);
                s.Write(roomLocation.yPos);
            }

            return (int) s.BaseStream.Position;
        }

        private static int WriteBestiary(BinaryWriter s, World w)
        {
            s.Write(w.BestiaryKillCounts.Count);
            foreach (KeyValuePair<string, int> kvp in w.BestiaryKillCounts)
            {
                s.Write(kvp.Key);
                s.Write(kvp.Value);
            }

            s.Write(w.WasNearPlayer.Count);
            foreach (string s2 in w.WasNearPlayer)
                s.Write(s2);

            s.Write(w.ChattedWithPlayer.Count);
            foreach (string s2 in w.ChattedWithPlayer)
                s.Write(s2);
            
            return (int) s.BaseStream.Position;
        }

        private static int WriteCreativePowers(BinaryWriter s, World w)
        {
            foreach (CreativePower power in w.CreativePowers)
            {
                s.Write(true);
                s.Write(power.Id);
                switch (power.Id)
                {
                    case 0: //FreezeTime
                    case 9: //FreezeRain
                    case 10: //FreezeWind
                    case 12: //StopBiomeSpread
                        s.Write(power.Enabled);
                        break;
                    case 8: //TimeRate
                    case 11: //Difficulty
                        s.Write(power.SliderValue);
                        break;
                }

                s.Write(false);
            }

            return (int) s.BaseStream.Position;
        }
        
        private static void WriteFooter(BinaryWriter s, World w)
        {
            s.Write(true);
            s.Write(w.Name);
            s.Write(w.WorldId);
        }
        
        public static void WriteWorld(BinaryWriter s, World w)
        {
            int fileFormatHeaderPos = WriteFileFormatHeader(s, w);
            int worldHeaderPos = WriteWorldHeader(s, w);
            int worldTilesPos = WriteWorldTiles(s, w);
            int chestsPos = WriteChests(s, w);
            int signsPos = WriteSigns(s, w);
            int npcsPos = WriteNpcs(s, w);
            int tileEntitiesPos = WriteTileEntities(s, w);
            int pressurePlatePos = WritePressurePlates(s, w);
            int townManagerPos = WriteTownManager(s, w);
            int bestiaryPos = WriteBestiary(s, w);
            int creativePowersPos = WriteCreativePowers(s, w);
            WriteFooter(s, w);

            
            s.BaseStream.Position = 26L;
            s.Write(fileFormatHeaderPos);
            s.Write(worldHeaderPos);
            s.Write(worldTilesPos);
            s.Write(chestsPos);
            s.Write(signsPos);
            s.Write(npcsPos);
            s.Write(tileEntitiesPos);
            s.Write(pressurePlatePos);
            s.Write(townManagerPos);
            s.Write(bestiaryPos);
            s.Write(creativePowersPos);
        }
    }
}
