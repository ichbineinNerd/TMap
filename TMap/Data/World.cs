using System;
using System.Collections.Generic;

namespace TMap.Data
{
    public class World
    { 
        //massive thanks to http://ludwig.schafer.free.fr as well as the ILSpy team

        #region FileFormatHeader

        // 1.4.0.5 = 230
        public int Version;
        public uint Revision;
        public int[] ArrayOfPointers;
        public bool[] TileFrameImportant;

        #endregion

        #region WorldHeader

        public string Name;
        public string Seed;
        public ulong WorldGenVer;
        public Guid UniqueId;
        public int WorldId;
        public int LeftWorld, RightWorld, TopWorld, BottomWorld;
        public int MaxTilesY, MaxTilesX;
        public int GameMode;
        public bool DrunkWorld;
        public bool GitGudWorld;

        public DateTime CreationTime;

        public byte MoonType;

        //tbh I have no clue what either of those do
        public readonly int[] TreeX = new int[3];
        public readonly int[] TreeStyle = new int[4];
        public readonly int[] CaveBackX = new int[3];
        public readonly int[] CaveBackStyle = new int[4];

        public int IceBackStyle, JungleBackStyle, HellBackStyle;
        public int SpawnTileX, SpawnTileY;

        public double WorldSurface, RockLayer;
        public double Time;
        public bool DayTime, BloodMoon, Eclipse;
        public int MoonPhase;

        public int DungeonX, DungeonY;

        public bool Crimson;

        //Boss1, Boss2, Boss3, QueenBee, Mech1, Mech2, Mech3, AnyMech, PlantBoss, GolemBoss, SlimeKing
        public enum BossIndexes
        {
            EyeOfCthulhu,
            EaterOfWorlds,
            Skeletron,
            QueenBee,
            Destroyer,
            Twins,
            Prime,
            AnyMech,
            Plantera,
            Golem,
            KingSlime,
            Fishron,
            Cultist,
            MoonLord,
            Pumpking,
            MourningWood,
            IceQueen,
            SantaNk,
            EverScream,
            SolarPillar,
            VortexPillar,
            NebulaPillar,
            StardustPillar,
            EmpressOfLight,
            QueenSlime
        }

        public readonly bool[] DownedBosses = new bool[Enum.GetNames(typeof(BossIndexes)).Length];

        public enum NpcIndexes
        {
            Goblin,
            Wizard,
            Mechanic,
            Angler,
            Stylist,
            TaxCollector,
            Golfer,
            TavernKeep
        }

        public readonly bool[] SavedNpcs = new bool[Enum.GetNames(typeof(NpcIndexes)).Length];

        public enum InvasionIndexes
        {
            Goblins,
            Clown,
            FrostMoon,
            Pirates,
            Martians
        }

        public readonly bool[] DownedInvasions = new bool[Enum.GetNames(typeof(InvasionIndexes)).Length];

        public bool ShadowOrb;
        public bool SpawnMeteor;
        public byte ShadowOrbCount;

        public int AltarCount;

        public bool HardMode;

        public int InvasionDelay, InvasionSize, InvasionType;

        public double InvasionX;

        public double SlimeRainTime;

        public byte SundialCooldown;

        public bool Raining;
        public int RainTime;
        public float MaxRain;

        public int CobaltTier, MythrilTier, AdamantiteTier;

        public byte TreeBg, CorruptBg, JungleBg, SnowBg, HallowBg, CrimsonBg, DesertBg, OceanBg;

        public int CloudBgActive;
        public short NumClouds;

        public float WindSpeedTarget;

        public string[] AnglerWhoFinishedToday;

        public int AnglerQuest;

        public int InvasionSizeStart;

        public int CultistDelay;

        public int[] KillCounts;

        public bool FastForwardTime;

        public bool SolarPillarUp;
        public bool VortexPillarUp;
        public bool NebulaPillarUp;
        public bool StardustPillarUp;

        public bool LunarEventsOngoing;

        public bool ManualParty;
        public bool GenuineParty;
        public int PartyCooldown;
        public int[] CelebratingNpcs;

        public bool SandStormOngoing;
        public int SandStormTimeLeft;
        public float SandStormSeverity;
        public float SandStormIntendedSeverity;

        public byte MaxDownedDungeonDefenders2;

        public byte MushroomBg, UnderworldBg, TreeBg2, TreeBg3, TreeBg4;

        public bool CombatBookUsed;

        public int LanternNightCooldown;
        public bool LanternNightGenuine;
        public bool LanternNightManual;
        public bool LanternNightNextNightGenuine;

        public int[] TreeTopVariations;

        public bool ForceHalloween;
        public bool ForceChristmas;

        public int CopperTier, IronTier, SilverTier, GoldTier;

        public bool BoughtCat, BoughtDog, BoughtBunny;

        #endregion

        #region WorldTiles

        public Tile[,] Tiles;

        #endregion

        #region Chests

        public Chest[] Chests;

        #endregion

        #region Signs

        public Sign[] Signs;

        #endregion

        #region NPCs

        public Npc[] CurrentNpcs;

        #endregion

        #region TileEntities

        public TileEntity[] TileEntities;

        #endregion

        #region PressurePlates

        public int[] PressurePlatesX;
        public int[] PressurePlatesY;

        #endregion

        #region TownManager

        public (int NpcId, int XPos, int YPos)[] RoomLocations;

        #endregion

        #region Bestiary

        public Dictionary<string, int> BestiaryKillCounts;
        public HashSet<string> WasNearPlayer;
        public HashSet<string> ChattedWithPlayer;

        #endregion
    }
}
