using System;
using System.IO;
using static TMap.World.BossIndexes;
using static TMap.World.NpcIndexes;
using static TMap.World.InvasionIndexes;

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
                    if (current.Id != 0)
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
                    else if (current.Id > 0)
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
        
        public static void WriteWorld(BinaryWriter s, World w)
        {
            int fileFormatHeaderPos = WriteFileFormatHeader(s, w);
            int worldHeaderPos = WriteWorldHeader(s, w);
            int worldTilesPos = WriteWorldTiles(s, w);
            int chestsPos = WriteChests(s, w);
            int signsPos = WriteSigns(s, w);
        }
    }
}
