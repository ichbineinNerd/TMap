using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using CommandLine;
using TMap.Data;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper Complains about the setters not being used in Options, even though they are (from the Attribute)

namespace TMap
{
    static class Program
    {
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
        private class ModifyWorldFieldAttribute : Attribute
        {
            public FieldInfo fi;
            public object val;

            public ModifyWorldFieldAttribute(string fi, object val)
            {
                this.fi = typeof(World).GetField(fi);
                this.val = val;
            }

            public ModifyWorldFieldAttribute(string fi)
            {
                this.fi = typeof(World).GetField(fi);
                this.val = null;
            }
        }

        private class Options
        {
            //I'm sorry you have to see this, it's spaghetti as hell

            //SeV => ModifyWorldField.Value
            //Set => Property.GetValue()
            //Tog => Toggle boolean field
            //Inc => Increment numeric field

            #region Meta

            [Option('f', "file", HelpText = "Path of the world file to load", Required = true)]
            public string Filepath { get; set; }

            [Option('o', "out", HelpText = "Where to write the modified world file", Required = false)]
            public string Output { get; set; }

            [Option('v', "version", HelpText = "Sets the verbosity", Required = false)]
            public int Verbosity { get; set; }

            #endregion

            [Option("increment-revision", HelpText = "Increases the Revision field by 1", Required = false)]
            public bool RevisionIncrement
            {
                get => this.IncrementRevision ?? false;
                set => this.IncrementRevision = value;
            }

            [ModifyWorldField("Revision")]
            public bool? IncrementRevision { get; set; }

            #region Modifying WorldHeader

            [ModifyWorldField("Name")]
            [Option("name", HelpText = "Changes the display name of the world", Required = false)]
            public string SetName { get; set; }

            [ModifyWorldField("MoonType")]
            [Option("moontype", HelpText = "Changes the World's Moon type", Required = false)]
            public byte? SetMoonType { get; set; }

            #region GameMode

            [ModifyWorldField("GameMode", 1)]
            [Option("expert", HelpText = "If given, changes the World to Expert mode", Required = false)]
            public bool? SeVExpert { get; set; }

            [ModifyWorldField("GameMode", 0)]
            [Option("normal", HelpText = "If given, changes the World to Normal mode", Required = false)]
            public bool? SeVNormal { get; set; }

            [ModifyWorldField("GameMode", 2)]
            [Option("master", HelpText = "If given, changes the World to Master mode", Required = false)]
            public bool? SeVMaster { get; set; }

            [ModifyWorldField("GameMode", 3)]
            [Option("journey", HelpText = "If given, changes the World to Journey mode", Required = false)]
            public bool? SeVCreative { get; set; }

            #endregion

            [ModifyWorldField("SpawnTileX")]
            [Option("spawntilex", HelpText = "Sets the x coordinate of your spawn", Required = false)]
            public int? SetSpawnTileX { get; set; }

            [ModifyWorldField("SpawnTileY")]
            [Option("spawntiley", HelpText = "Sets the y coordinate of your spawn", Required = false)]
            public int? SetSpawnTileY { get; set; }

            #endregion
        }

        private static object Increment(object o)
        {
            return o switch
            {
                byte b => b + 1,
                sbyte b => b + 1,
                ushort s => s + 1,
                short s => s + 1,
                uint i => i + 1,
                int i => i + 1,
                ulong l => l + 1,
                long l => l + 1,
                _ => null
            };
        }

        private static void ApplyModifications(World w, Options o, int verbosity)
        {
            foreach (PropertyInfo pi in o.GetType().GetProperties())
            {
                if (pi.GetValue(o) == null)
                    continue;
                
                Attribute[] attributes = Attribute.GetCustomAttributes(pi, typeof(ModifyWorldFieldAttribute));
                foreach (Attribute a in attributes)
                {
                    ModifyWorldFieldAttribute mwfa = a as ModifyWorldFieldAttribute;

                    object oldValue = mwfa?.fi.GetValue(w);
                    
                    object valueToSetTo = pi.Name.Substring(0, 3) switch
                    {
                        "SeV" => mwfa?.val,
                        "Set" => pi.GetValue(o),
                        "Tog" => !(bool?) oldValue,
                        "Inc" => Increment(oldValue),
                        _ => null
                    };

                    if (verbosity >= 1)
                        Console.WriteLine(
                            $"{mwfa?.fi.Name} set to {valueToSetTo}");
                    if (verbosity >= 2)
                        Console.WriteLine($"(was {oldValue})");

                    mwfa?.fi.SetValue(w, valueToSetTo);
                }
            }
        }

        private static void WriteWorld(BinaryWriter s, World w)
        {
            WorldWriter.WriteWorld(s, w);
        }

        private static async Task<World> ReadWorld(byte[] b)
        {
            await using MemoryStream ms = new MemoryStream(b);
            using BinaryReader br = new BinaryReader(ms);
            return WorldReader.ReadWorld(br);
        }

        private static async Task<byte[]> ReadStdinToEnd()
        {
            byte[] result = new byte[7_000_000];
            int length;
            await using (Stream stdin = Console.OpenStandardInput())
            {
                length = await stdin.ReadAsync(result, 0, result.Length);
            }

            byte[] result2 = new byte[length];
            Array.Copy(result, 0, result2, 0, length);
            return result2;
        }

        private static async Task Execute(Options o)
        {
            //I tried to do this with streams and consistently ran into buffering problems which i'm not smart
            //enough to fix. TODO
            byte[] inBytes = o.Filepath == null ? await ReadStdinToEnd() : await File.ReadAllBytesAsync(o.Filepath);

            await using BinaryWriter outStream = o.Output == null
                ? new BinaryWriter(Console.OpenStandardOutput())
                : new BinaryWriter(File.OpenWrite(o.Output));

            World w = await ReadWorld(inBytes);
            ApplyModifications(w, o, o.Verbosity);
            WriteWorld(outStream, w);
        }

        private static async Task<int> Main(string[] args)
        {
            // really the only reason this is a new function is that I dislike having that many lines indented
            // by one level, just because the library doesnt support getting the Options without a callback (to my
            // knowledge)
            await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(o => Execute(o));
            //TODO proper exit codes (probably just copy sysexits.h)
            return 0;
        }
    }
}