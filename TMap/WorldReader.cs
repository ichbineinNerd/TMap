using System;
using System.Collections.Generic;
using System.IO;
using TMap.Data;
using static TMap.Data.World.BossIndexes;
using static TMap.Data.World.InvasionIndexes;
using static TMap.Data.World.NpcIndexes;

namespace TMap
{
    public static class WorldReader
    {
        private static void ReadFileFormatHeader(World w, BinaryReader b)
        {
            w.Version = b.ReadInt32();

            if (new string(b.ReadChars(7)) != "relogic")
                throw new ArgumentException("magic number was wrong");

            if (b.ReadByte() != 2)
                throw new ArgumentException("File type was wrong");

            w.Revision = b.ReadUInt32();

            //favorite, should always be 0?
            b.ReadUInt64();

            w.ArrayOfPointers = new int[b.ReadInt16()];

            for (int i = 0; i < w.ArrayOfPointers.Length; i++)
                w.ArrayOfPointers[i] = b.ReadInt32();

            w.TileFrameImportant = new bool[b.ReadInt16()];

            int j = 0;
            while (j != w.TileFrameImportant.Length)
            {
                byte by = b.ReadByte();
                for (int k = 0; k < 8; k++)
                {
                    w.TileFrameImportant[j++] = (by & 1) != 0;
                    if (j == w.TileFrameImportant.Length)
                        break;
                    by >>= 1;
                }
            }
        }

        private static void ReadWorldHeader(World w, BinaryReader b)
        {
            w.Name = b.ReadString();
            w.Seed = b.ReadString();
            w.WorldGenVer = b.ReadUInt64();
            w.UniqueId = new Guid(b.ReadBytes(16));
            w.WorldId = b.ReadInt32();
            w.LeftWorld = b.ReadInt32();
            w.RightWorld = b.ReadInt32();
            w.TopWorld = b.ReadInt32();
            w.BottomWorld = b.ReadInt32();
            w.MaxTilesY = b.ReadInt32();
            w.MaxTilesX = b.ReadInt32();
            w.GameMode = b.ReadInt32();
            w.DrunkWorld = b.ReadBoolean();
            w.GitGudWorld = b.ReadBoolean();
            w.CreationTime = DateTime.FromBinary(b.ReadInt64());
            w.MoonType = b.ReadByte();

            #region an absolute mess of graphic things

            w.TreeX[0] = b.ReadInt32();
            w.TreeX[1] = b.ReadInt32();
            w.TreeX[2] = b.ReadInt32();
            w.TreeStyle[0] = b.ReadInt32();
            w.TreeStyle[1] = b.ReadInt32();
            w.TreeStyle[2] = b.ReadInt32();
            w.TreeStyle[3] = b.ReadInt32();
            w.CaveBackX[0] = b.ReadInt32();
            w.CaveBackX[1] = b.ReadInt32();
            w.CaveBackX[2] = b.ReadInt32();
            w.CaveBackStyle[0] = b.ReadInt32();
            w.CaveBackStyle[1] = b.ReadInt32();
            w.CaveBackStyle[2] = b.ReadInt32();
            w.CaveBackStyle[3] = b.ReadInt32();

            w.IceBackStyle = b.ReadInt32();
            w.JungleBackStyle = b.ReadInt32();
            w.HellBackStyle = b.ReadInt32();

            #endregion

            w.SpawnTileX = b.ReadInt32();
            w.SpawnTileY = b.ReadInt32();

            w.WorldSurface = b.ReadDouble();
            w.RockLayer = b.ReadDouble();

            w.Time = b.ReadDouble();
            w.DayTime = b.ReadBoolean();
            w.MoonPhase = b.ReadInt32();
            w.BloodMoon = b.ReadBoolean();
            w.Eclipse = b.ReadBoolean();

            w.DungeonX = b.ReadInt32();
            w.DungeonY = b.ReadInt32();

            w.Crimson = b.ReadBoolean();

            for (int i = (int) EyeOfCthulhu; i <= (int) KingSlime; i++)
                w.DownedBosses[i] = b.ReadBoolean();

            for (int i = (int) Goblin; i <= (int) Mechanic; i++)
                w.SavedNpcs[i] = b.ReadBoolean();

            for (int i = (int) Goblins; i <= (int) Pirates; i++)
                w.DownedInvasions[i] = b.ReadBoolean();

            w.ShadowOrb = b.ReadBoolean();
            w.SpawnMeteor = b.ReadBoolean();
            w.ShadowOrbCount = b.ReadByte();
            w.AltarCount = b.ReadInt32();

            w.HardMode = b.ReadBoolean();

            w.InvasionDelay = b.ReadInt32();
            w.InvasionSize = b.ReadInt32();
            w.InvasionType = b.ReadInt32();
            w.InvasionX = b.ReadDouble();

            w.SlimeRainTime = b.ReadDouble();

            w.SundialCooldown = b.ReadByte();

            w.Raining = b.ReadBoolean();
            w.RainTime = b.ReadInt32();
            w.MaxRain = b.ReadSingle();

            w.CobaltTier = b.ReadInt32();
            w.MythrilTier = b.ReadInt32();
            w.AdamantiteTier = b.ReadInt32();

            w.TreeBg = b.ReadByte();
            w.CorruptBg = b.ReadByte();
            w.JungleBg = b.ReadByte();
            w.SnowBg = b.ReadByte();
            w.HallowBg = b.ReadByte();
            w.CrimsonBg = b.ReadByte();
            w.DesertBg = b.ReadByte();
            w.OceanBg = b.ReadByte();

            w.CloudBgActive = b.ReadInt32();
            w.NumClouds = b.ReadInt16();

            w.WindSpeedTarget = b.ReadSingle();

            w.AnglerWhoFinishedToday = new string[b.ReadInt32()];
            for (int i = 0; i < w.AnglerWhoFinishedToday.Length; i++)
                w.AnglerWhoFinishedToday[i] = b.ReadString();

            w.SavedNpcs[3] = b.ReadBoolean();

            w.AnglerQuest = b.ReadInt32();

            for (int i = (int) Stylist; i <= (int) Golfer; i++)
                w.SavedNpcs[i] = b.ReadBoolean();

            w.InvasionSizeStart = b.ReadInt32();

            w.CultistDelay = b.ReadInt32();

            w.KillCounts = new int[b.ReadInt16()];
            for (int i = 0; i < w.KillCounts.Length; i++)
                w.KillCounts[i] = b.ReadInt32();

            w.FastForwardTime = b.ReadBoolean();

            w.DownedBosses[(int) Fishron] = b.ReadBoolean();
            w.DownedInvasions[(int) Martians] = b.ReadBoolean();
            for (int i = (int) Cultist; i <= (int) StardustPillar; i++)
                w.DownedBosses[i] = b.ReadBoolean();

            w.SolarPillarUp = b.ReadBoolean();
            w.VortexPillarUp = b.ReadBoolean();
            w.NebulaPillarUp = b.ReadBoolean();
            w.StardustPillarUp = b.ReadBoolean();

            w.LunarEventsOngoing = b.ReadBoolean();

            w.ManualParty = b.ReadBoolean();
            w.GenuineParty = b.ReadBoolean();
            w.PartyCooldown = b.ReadInt32();
            w.CelebratingNpcs = new int[b.ReadInt32()];
            for (int i = 0; i < w.CelebratingNpcs.Length; i++)
                w.CelebratingNpcs[i] = b.ReadInt32();

            w.SandStormOngoing = b.ReadBoolean();
            w.SandStormTimeLeft = b.ReadInt32();
            w.SandStormSeverity = b.ReadSingle();
            w.SandStormIntendedSeverity = b.ReadSingle();

            w.SavedNpcs[(int) TavernKeep] = b.ReadBoolean();

            if (b.ReadBoolean())
                w.MaxDownedDungeonDefenders2 = 1;
            if (b.ReadBoolean())
                w.MaxDownedDungeonDefenders2 = 2;
            if (b.ReadBoolean())
                w.MaxDownedDungeonDefenders2 = 3;

            w.MushroomBg = b.ReadByte();
            w.UnderworldBg = b.ReadByte();
            w.TreeBg2 = b.ReadByte();
            w.TreeBg3 = b.ReadByte();
            w.TreeBg4 = b.ReadByte();

            w.CombatBookUsed = b.ReadBoolean();

            w.LanternNightCooldown = b.ReadInt32();
            w.LanternNightGenuine = b.ReadBoolean();
            w.LanternNightManual = b.ReadBoolean();
            w.LanternNightNextNightGenuine = b.ReadBoolean();

            w.TreeTopVariations = new int[b.ReadInt32()];
            for (int i = 0; i < w.TreeTopVariations.Length; i++)
                w.TreeTopVariations[i] = b.ReadInt32();

            w.ForceHalloween = b.ReadBoolean();
            w.ForceChristmas = b.ReadBoolean();

            w.CopperTier = b.ReadInt32();
            w.IronTier = b.ReadInt32();
            w.SilverTier = b.ReadInt32();
            w.GoldTier = b.ReadInt32();

            w.BoughtCat = b.ReadBoolean();
            w.BoughtDog = b.ReadBoolean();
            w.BoughtBunny = b.ReadBoolean();

            w.DownedBosses[(int) EmpressOfLight] = b.ReadBoolean();
            w.DownedBosses[(int) QueenSlime] = b.ReadBoolean();
        }

        private static void ReadWorldTiles(World w, BinaryReader b)
        {
            w.Tiles = new Tile[w.MaxTilesX, w.MaxTilesY];
            for (int i = 0; i < w.MaxTilesX; i++)
            {
                for (int j = 0; j < w.MaxTilesY; j++)
                {
                    Tile t = new Tile();
                    w.Tiles[i, j] = t;

                    byte flags1 = b.ReadByte();
                    byte flags2 = 0;
                    byte flags3 = 0;
                    if ((flags1 & 0x01) != 0)
                    {
                        flags2 = b.ReadByte();
                        if ((flags2 & 0x01) != 0)
                        {
                            flags3 = b.ReadByte();
                        }
                    }

                    if ((flags1 & 0x02) != 0)
                    {
                        t.Id = b.ReadByte();
                        if ((flags1 & 0x20) != 0)
                        {
                            t.Id = b.ReadByte() << 8 | t.Id;
                        }
                    }
                    else
                        t.Id = -1;

                    if (t.Id != -1 && w.TileFrameImportant[t.Id])
                    {
                        t.FrameX = b.ReadInt16();
                        t.FrameY = b.ReadInt16();
                    }

                    if ((flags3 & 0x08) != 0)
                    {
                        t.Color = b.ReadByte();
                    }

                    if ((flags1 & 0x04) != 0)
                    {
                        t.Wall = b.ReadByte();
                        if ((flags3 & 0x40) != 0)
                        {
                            t.Wall = (ushort) (b.ReadByte() << 8 | t.Wall);
                        }
                    }

                    if ((flags3 & 0x10) != 0)
                    {
                        t.WallColor = b.ReadByte();
                    }

                    if ((flags1 & 0x18) != 0)
                    {
                        switch ((flags1 & 0x18) >> 3)
                        {
                            case 1:
                                t.LiquidAmount = b.ReadByte();
                                break;
                            case 2:
                                t.Lava = true;
                                goto case 1;
                            case 3:
                                t.Honey = true;
                                goto case 1;
                        }
                    }

                    if ((flags2 & 0x02) != 0)
                        t.RedWire = true;
                    if ((flags2 & 0x04) != 0)
                        t.BlueWire = true;
                    if ((flags2 & 0x08) != 0)
                        t.GreenWire = true;
                    if ((flags3 & 0x20) != 0)
                        t.YellowWire = true;

                    switch ((flags2 & 0x70) >> 4)
                    {
                        case 1:
                            t.HalfBrick = true;
                            break;
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                            t.Slope = (byte) (((flags2 & 0x70) >> 4) - 1);
                            break;
                    }

                    t.Actuator = (flags3 & 0x02) != 0;
                    t.Active = (flags3 & 0x04) == 0;

                    if ((flags1 & 0xC0) != 0)
                    {
                        int repeat = b.ReadByte();
                        if ((flags1 & 0x80) != 0)
                            repeat = b.ReadByte() << 8 | repeat;

                        while (repeat > 0)
                        {
                            repeat--;
                            j++;
                            w.Tiles[i, j] = new Tile(t);
                        }
                    }
                }
            }
        }

        private static void ReadChests(World w, BinaryReader b)
        {
            w.Chests = new Chest[b.ReadInt16()];
            int itemAmount = b.ReadInt16();
            for (int i = 0; i < w.Chests.Length; i++)
            {
                Chest current = new Chest();
                w.Chests[i] = current;
                current.X = b.ReadInt32();
                current.Y = b.ReadInt32();
                current.Name = b.ReadString();
                current.Contents = new Item[itemAmount];
                for (int j = 0; j < itemAmount; j++)
                {
                    short amount = b.ReadInt16();
                    if (amount == 0)
                        current.Contents[j] = null;
                    else
                    {
                        Item currentItem = new Item();
                        current.Contents[j] = currentItem;
                        currentItem.Stack = amount;
                        currentItem.Id = b.ReadInt32();
                        currentItem.Prefix = b.ReadByte();
                    }
                }
            }
        }

        private static void ReadSigns(World w, BinaryReader b)
        {
            w.Signs = new Sign[b.ReadInt16()];
            for (int i = 0; i < w.Signs.Length; i++)
            {
                w.Signs[i] = new Sign {Text = b.ReadString(), X = b.ReadInt32(), Y = b.ReadInt32()};
            }
        }

        private static void ReadNpcs(World w, BinaryReader b)
        {
            List<Npc> npcsTmp = new List<Npc>();

            while (b.ReadBoolean())
            {
                Npc current = new Npc();
                npcsTmp.Add(current);

                current.Active = true;
                current.Id = b.ReadInt32();
                current.Name = b.ReadString();
                current.X = b.ReadSingle();
                current.Y = b.ReadSingle();
                current.Homeless = b.ReadBoolean();
                current.HomeTileX = b.ReadInt32();
                current.HomeTileY = b.ReadInt32();
                //always 0x01
                current.TownNpc = b.ReadByte() != 0;
                current.TownNpcVariationIndex = b.ReadInt32();
            }

            while (b.ReadBoolean())
            {
                //this is exclusively for celestial pillars as of now
                Npc current = new Npc();
                npcsTmp.Add(current);
                current.TownNpc = false;
                current.Active = true;
                current.Id = b.ReadInt32();
                current.X = b.ReadSingle();
                current.Y = b.ReadSingle();
            }

            w.CurrentNpcs = npcsTmp.ToArray();
        }

        private static void ReadTileEntities(World w, BinaryReader b)
        {
            w.TileEntities = new TileEntity[b.ReadInt32()];
            for (int i = 0; i < w.TileEntities.Length; i++)
            {
                TileEntity current = new TileEntity();
                w.TileEntities[i] = current;

                current.Type = b.ReadByte();
                current.Id = b.ReadInt32();
                current.X = b.ReadInt16();
                current.Y = b.ReadInt16();
                Item[] items;
                switch (current.Type) //Ignoring Pylons (7) on purpose here, since they dont store anything extra
                {
                    case 0: // Training Dummy
                        current.Npc = b.ReadInt16();
                        break;
                    case 1: // Item Frame
                    case 4: // Weapon Rack
                    case 6: // Food Platter
                        current.Items = new Item[1];
                        current.Items[0] = new Item()
                        {
                            Id = b.ReadInt16(),
                            Prefix = b.ReadByte(),
                            Stack = b.ReadInt16()
                        };
                        break;
                    case 2: // logic switch
                        current.LogicType = b.ReadByte();
                        current.Enabled = b.ReadBoolean();
                        break;
                    case 3: // Display doll
                        byte itemSlots = b.ReadByte();
                        byte dyeSlots = b.ReadByte();
                        items = new Item[16];
                        for (int j = 0; j < 8; j++)
                        {
                            if ((itemSlots & 1 << j) != 0)
                                items[j] = new Item
                                {
                                    Id = b.ReadInt16(),
                                    Prefix = b.ReadByte(),
                                    Stack = b.ReadInt16()
                                };
                            else
                                items[j] = null;
                        }
                        
                        for (int j = 0; j < 8; j++)
                        {
                            if ((dyeSlots & 1 << j) != 0)
                                items[8 + j] = new Item
                                {
                                    Id = b.ReadInt16(),
                                    Prefix = b.ReadByte(),
                                    Stack = b.ReadInt16()
                                };
                            else
                                items[8 + j] = null;
                        }

                        current.Items = items;

                        break;
                    case 5: // Hat rack
                        byte slots = b.ReadByte();
                        items = new Item[4];
                        for (int j = 0; j < 2; j++)
                        {
                            if ((slots & 1 << j) != 0)
                                items[j] = new Item
                                {
                                    Id = b.ReadInt16(),
                                    Prefix = b.ReadByte(),
                                    Stack = b.ReadInt16()
                                };
                            else
                                items[j] = null;
                        }
                        
                        for (int j = 2; j < 4; j++)
                        {
                            if ((slots & 1 << j) != 0)
                                items[j] = new Item
                                {
                                    Id = b.ReadInt16(),
                                    Prefix = b.ReadByte(),
                                    Stack = b.ReadInt16()
                                };
                            else
                                items[j] = null;
                        }

                        current.Items = items;
                        break;
                }
            }
        }

        private static void ReadPressurePlates(World w, BinaryReader b)
        {
            w.PressurePlatesX = new int[b.ReadInt32()];
            w.PressurePlatesY = new int[w.PressurePlatesX.Length];

            for (int i = 0; i < w.PressurePlatesX.Length; i++)
            {
                w.PressurePlatesX[i] = b.ReadInt32();
                w.PressurePlatesY[i] = b.ReadInt32();
            }
        }

        private static void ReadTownManager(World w, BinaryReader b)
        {
            w.RoomLocations = new (int, int, int)[b.ReadInt32()];

            for (int i = 0; i < w.RoomLocations.Length; i++)
            {
                w.RoomLocations[i].NpcId = b.ReadInt32();
                w.RoomLocations[i].XPos = b.ReadInt32();
                w.RoomLocations[i].YPos = b.ReadInt32();
            }
        }

        private static void ReadBestiary(World w, BinaryReader b)
        {
            int maxLength = b.ReadInt32();
            w.BestiaryKillCounts = new Dictionary<string, int>(maxLength);
            for (int i = 0; i < maxLength; i++)
                w.BestiaryKillCounts.Add(b.ReadString(), b.ReadInt32());

            maxLength = b.ReadInt32();
            w.WasNearPlayer = new HashSet<string>(maxLength);
            for (int i = 0; i < maxLength; i++)
                w.WasNearPlayer.Add(b.ReadString());
            
            maxLength = b.ReadInt32();
            w.ChattedWithPlayer = new HashSet<string>(maxLength);
            for (int i = 0; i < maxLength; i++)
                w.ChattedWithPlayer.Add(b.ReadString());
        }

        private static void ReadCreativePowers(World w, BinaryReader b)
        {
            List<CreativePower> powers = new List<CreativePower>();
            while (b.ReadBoolean())
            {
                CreativePower power = new CreativePower();
                switch (power.Id = b.ReadUInt16())
                {
                    case 0: //FreezeTime
                    case 9: //FreezeRain
                    case 10: //FreezeWind
                    case 12: //StopBiomeSpread
                        power.Enabled = b.ReadBoolean();
                        break;
                    case 8: //TimeRate
                    case 11: //Difficulty
                        power.SliderValue = b.ReadSingle();
                        break;
                }

                powers.Add(power);
            }

            w.CreativePowers = powers;
        }
        
        public static World ReadWorld(BinaryReader b)
        {
            World w = new World();
            ReadFileFormatHeader(w, b);
            ReadWorldHeader(w, b);
            ReadWorldTiles(w, b);
            ReadChests(w, b);
            ReadSigns(w, b);
            ReadNpcs(w, b);
            ReadTileEntities(w, b);
            ReadPressurePlates(w, b);
            ReadTownManager(w, b);
            ReadBestiary(w, b);
            ReadCreativePowers(w, b);
            
            return w;
        }
    }
}
