using BfresLibrary;
using BfresLibrary.PlatformConverters;
using BYAML;
using Byml.cs.lib.Ext;
using EveryFileExplorer;
using KclLibrary;
using Syroot.BinaryData;
using System;
using System.IO;
using SARCExt;
using AampLibraryCSharp;

namespace DokanGroups {
    public class IntersectionDokan {
        public static SarcData SZSDecompress(string file) {
            return SARC.UnpackRamN(YAZ0.Decompress(file));
        }

        public static void SZSCompress(SarcData sarc, string file) {
            File.WriteAllBytes(file, YAZ0.Compress(SARC.PackN(sarc).Item2, 3, (uint)SARC.PackN(sarc).Item1));
        }

        public static byte[] BYMLPort(byte[] file, ByteOrder byteOrder = ByteOrder.BigEndian) {
            MemoryStream stream = new(file);
            BymlFileData byml = ByamlFile.LoadN(stream, false, byteOrder);
            stream.Close();

            if(byteOrder.Equals(ByteOrder.BigEndian)) byml.Version = 2;
            else byml.Version = 1;

            return ByamlFile.FastSaveN(BYMLExtensions.SwitchEndianness(byml));
        }

        public static byte[] BFRESPort(byte[] file) {
            MemoryStream stream = new(file);

            ResFile bfres = new(stream);
            stream.Close();

            if(bfres.ByteOrder.Equals(ByteOrder.BigEndian)) {
                bfres.ChangePlatform(true, 4096, 0, 9, 0, 0, ConverterHandle.SM3DW);
                bfres.Alignment = 0x0C;
            } else {
                Console.WriteLine("Wii U not supported");
                return null;
            }

            MemoryStream saveStream = new();

            bfres.Save(saveStream, true);

            byte[] value = saveStream.ToArray();
            saveStream.Close();

            return value;
        }

        public static byte[] KCLPort(byte[] file, ByteOrder byteOrder = ByteOrder.LittleEndian) {
            MemoryStream stream = new(file);
            KCLFile kcl = new(stream);
            stream.Close();

            MemoryStream saveStream = new();
            kcl.ByteOrder = byteOrder;
            kcl.Save(saveStream);

            byte[] result = saveStream.ToArray();
            saveStream.Close();

            return result;
        }

        public static byte[] AAMPPort(byte[] file, byte version = 1) {
            MemoryStream stream = new(file);
            AampFile aamp = AampFile.LoadFile(stream);
            stream.Close();

            if(version == 1) aamp.ConvertToVersion2();
            else aamp.ConvertToVersion1();

            MemoryStream saveStream = new();
            aamp.Save(saveStream);
            byte[] result = stream.ToArray();
            saveStream.Close();

            return result;
        }
    }
}
