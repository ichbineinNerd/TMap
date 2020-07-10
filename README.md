# TMap - Terraria World File Parser and Editor

TMap provides an easy way to parse Terraria world files (their name usually ends in `.wld`).

To Read a World, simply create a `BinaryReader` from the contents of the save, and then call `WorldReader.ReadWorld`:

    World w = WorldReader.ReadWorld(new BinaryReader(new MemoryStream(yourByteArray)));

This `World` Object contains all the info that was stored in the file, such as Gamemode, World Evil, Rescued NPCs, The World's name and much more.
