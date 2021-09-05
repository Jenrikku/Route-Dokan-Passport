using DokanGroups;
using EveryFileExplorer;
using System.IO;
using System.Text;

namespace TestingDokan {
    class DokanEntrance {
        static void Main(string[] args) {
            // Enables Shift-JIS encoding
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("Shift-JIS");

            //ObjectDokan.Obj2Switch(new ResFile(@"M:\Modding\3DW\temp\ArrowStepStraightLong.bfres"))
            //    .Save(new FileStream(@"M:\Modding\3DW\temp\ArrowStepStraightLong_Switch.bfres", FileMode.Create));

            //IntersectionDokan.SZSCompress(
            //    IntersectionDokan.SZSDecompress(@"M:\Modding\3DW\resources\romfs (nSwitch)\ObjectData\Gabon.szs"),
            //    @"M:\Modding\3DW\temp\Gabon.szs");

            //ObjectDokan.Obj2Switch(
            //    IntersectionDokan.SZSDecompress(
            //        @"G:\Console Games\Wii U\Super Mario 3D World (E) Decrypted [0005000010145d00]\content\ObjectData\Gabon.szs"));

            File.WriteAllBytes(
                @"M:\Modding\3DW\temp\LuigiRaccoon Switch\ObjectData\LuigiRaccoonDog.sarc",
                YAZ0.Decompress(File.ReadAllBytes(@"M:\Modding\3DW\temp\LuigiRaccoon Switch\ObjectData\LuigiRaccoonDog.szs")));
            //File.WriteAllBytes(
            //    @"M:\Modding\3DW\temp\LuigiRaccoon Switch\ObjectData\LuigiRaccoonDog_fixed.sarc",
            //    YAZ0.Decompress(File.ReadAllBytes(@"C:\Users\Jenrikku\AppData\Roaming\yuzu\load\010028600EBDA000\Porter Testing\romfs\ObjectData\LuigiRaccoonDog.szs")).ToArray());

            //YAZ0.Compress(@"M:\Modding\3DW\temp\LuigiRaccoon Switch\ObjectData\LuigiRaccoonDog.sarc");
        }
    }
}
