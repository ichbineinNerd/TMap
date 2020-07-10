using CommandLine;
using TMap;
using static TMap.Program.ModifyWorldFieldAttribute.Actions;

namespace TMapExample
{
    public class Options
    {
        #region Meta

        [Option('f', "file", HelpText = "Path of the world file to load", Required = false)]
        public string Filepath { get; set; }

        [Option('o', "out", HelpText = "Where to write the modified world file", Required = false)]
        public string Output { get; set; }

        [Option('v', "verbosity", HelpText = "Sets the verbosity", Required = false)]
        public int Verbosity { get; set; }

        #endregion

        #region FileFormatHeader

        [Program.ModifyWorldFieldAttribute("Revision", action = Increment)]
        [Option("increment-revision", HelpText = "Increases the Revision field by 1", Required = false)]
        public bool IncrementRevision { get; set; }
        
        #endregion

        #region WorldHeader

        [Program.ModifyWorldFieldAttribute("Name")]
        [Option("name", HelpText = "Changes the display name of the world", Required = false)]
        public string WorldName { get; set; }

        [Program.ModifyWorldFieldAttribute("MoonType")]
        [Option("moontype", HelpText = "Changes the World's Moon type", Required = false)]
        public byte? MoonType { get; set; }

        #region GameMode

        [Program.ModifyWorldFieldAttribute("GameMode", 1, SetToValue)]
        [Option("expert", HelpText = "If given, changes the World to Expert mode", Required = false)]
        public bool Expert { get; set; }

        [Program.ModifyWorldFieldAttribute("GameMode", 0, SetToValue)]
        [Option("normal", HelpText = "If given, changes the World to Normal mode", Required = false)]
        public bool Normal { get; set; }

        [Program.ModifyWorldFieldAttribute("GameMode", 2, SetToValue)]
        [Option("master", HelpText = "If given, changes the World to Master mode", Required = false)]
        public bool Master { get; set; }

        [Program.ModifyWorldFieldAttribute("GameMode", 3, SetToValue)]
        [Option("journey", HelpText = "If given, changes the World to Journey mode", Required = false)]
        public bool Creative { get; set; }

        #endregion

        [Program.ModifyWorldFieldAttribute("SpawnTileX")]
        [Option("spawntilex", HelpText = "Sets the x coordinate of your spawn point", Required = false)]
        public int? SpawnTileX { get; set; }

        [Program.ModifyWorldFieldAttribute("SpawnTileY")]
        [Option("spawntiley", HelpText = "Sets the y coordinate of your spawn point", Required = false)]
        public int? SpawnTileY { get; set; }
        
        [Program.ModifyWorldFieldAttribute("BloodMoon", action = Toggle)]
        [Option("toggle-bloodmoon", HelpText = "Toggles the bloodmoon event on or off", Required = false)]
        public bool ToggleBloodMoon { get; set; }


        [Program.ModifyWorldFieldAttribute("Eclipse", action = Toggle)]
        [Option("toggle-eclipse", HelpText = "Toggles the solar eclipse event on or off", Required = false)]
        public bool ToggleEclipse { get; set; }

        #endregion
    }
}