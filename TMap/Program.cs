using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using TMap.Data;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper Complains about the setters not being used in Options, even though they are (from the Attribute)

namespace TMap
{
    static class Program
    {
        private class Options
        {
            [Option('f', "file", HelpText = "Path of the world file to load", Required = true)]
            public string Filepath { get; set; }

            [Option('o', "out", HelpText = "Where to write the modified world file", Required = false)]
            public string Output { get; set; }
        }

        private static void ApplyModifications(World w, Options o)
        {
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
            ApplyModifications(w, o);
            WriteWorld(outStream, w);
        }

        private static async Task<int> Main(string[] args)
        {
            // really the only reason this is a new function is that I dislike having that many lines indented
            // by one level, just because the library doesnt support getting the Options without a callback (to my
            // knowledge)
            await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(Execute);
            //TODO proper exit codes (probably just copy sysexits.h)
            return 0;
        }
    }
}
