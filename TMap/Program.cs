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
        internal class ModifyWorldFieldAttribute : Attribute
        {
            public enum Actions
            {
                Increment,
                SetToValue,
                SetToPropertyValue,
                Toggle
            }

            public FieldInfo fi;
            public object val;
            public Actions action;

            public ModifyWorldFieldAttribute(string fi, object val, Actions action)
            {
                this.fi = typeof(World).GetField(fi);
                this.val = val;
                this.action = action;
            }

            public ModifyWorldFieldAttribute(string fi)
            {
                this.fi = typeof(World).GetField(fi);
                this.val = null;
                this.action = Actions.SetToPropertyValue;
            }
        }

        private static object Increment(object o) => o switch
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

        private static void ApplyModifications(World w, Options o, int verbosity)
        {
            foreach (PropertyInfo pi in o.GetType().GetProperties())
            {
                Attribute[] attributes = Attribute.GetCustomAttributes(pi, typeof(ModifyWorldFieldAttribute));
                foreach (Attribute a in attributes)
                {
                    ModifyWorldFieldAttribute mwfa = a as ModifyWorldFieldAttribute;

                    object oldValue = mwfa?.fi.GetValue(w);

                    object valueToSetTo = mwfa?.action switch
                    {
                        ModifyWorldFieldAttribute.Actions.SetToValue => mwfa.val,
                        ModifyWorldFieldAttribute.Actions.SetToPropertyValue => pi.GetValue(o),
                        ModifyWorldFieldAttribute.Actions.Increment => Increment(mwfa.fi.GetValue(w)),
                        // ReSharper disable once PossibleNullReferenceException
                        ModifyWorldFieldAttribute.Actions.Toggle => !(bool)mwfa.fi.GetValue(w),
                        _ => null
                    };

                    if (mwfa?.action == ModifyWorldFieldAttribute.Actions.SetToPropertyValue && valueToSetTo == null)
                        continue;
                    
                    if (mwfa?.action != ModifyWorldFieldAttribute.Actions.SetToPropertyValue && !(bool)pi.GetValue(o))
                        continue;
                    
                    if (verbosity >= 1)
                        Console.WriteLine(
                            $"{mwfa.fi.Name} set to {valueToSetTo}");
                    if (verbosity >= 2)
                        Console.WriteLine($"(was {oldValue})");

                    mwfa.fi.SetValue(w, valueToSetTo);
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
            await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(Execute);
            //TODO proper exit codes (probably just copy sysexits.h)
            return 0;
        }
    }
}