using DokanGroups;
using SARCExt;
using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleDokan {
    public class DokanEntrance {
        public static void Main(string[] args) {
            // Enables Shift-JIS encoding
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("Shift-JIS");

            string directory;
            string output = string.Empty;
            bool isWiiU = false;
            bool isPrepasedVersion = false;

            switch(args.Length) {
                case 0:
                    Console.WriteLine("* || Route Dokan Passport - SM3DW mod porter by Jenrikku || * \n\n" +
                        "Usage: RouteDokanPassport.exe [modFolder] [output]");
                    return;
                case 1:
                    directory = args[0];
                    if(output == string.Empty) output = Path.Join(directory, "out/");
                    break;
                case 2:
                    output = args[1];
                    goto case 1;
                default:
                    isPrepasedVersion = true;
                    isWiiU = bool.Parse(args[2]);
                    goto case 2;
            }

            if(Directory.Exists(Path.Join(directory, "content")))
                directory = Path.Join(directory, "content");

            if(Directory.Exists(Path.Join(directory, "romfs")))
                directory = Path.Join(directory, "romfs");

            if(!(Directory.Exists(Path.Join(directory, "CubeMapTextureData")) ||
               Directory.Exists(Path.Join(directory, "EffectData")) ||
               Directory.Exists(Path.Join(directory, "LayoutData")) ||
               Directory.Exists(Path.Join(directory, "LocalizedData")) ||
               Directory.Exists(Path.Join(directory, "LuigiBros")) ||
               Directory.Exists(Path.Join(directory, "ObjectData")) ||
               Directory.Exists(Path.Join(directory, "ShaderData")) ||
               Directory.Exists(Path.Join(directory, "SoundData")) ||
               Directory.Exists(Path.Join(directory, "StageData")) ||
               Directory.Exists(Path.Join(directory, "SystemData")))) {
                Console.WriteLine("This folder isn't valid, please select a mod folder.");
                return;
            }

            if(!isPrepasedVersion)
                isWiiU = IntersectionDokan.SZSDecompress(
                    new DirectoryInfo(directory).GetDirectories()[0].GetFiles()[0].FullName)
                    .endianness == ByteOrder.BigEndian;

            if(!isWiiU) {
                Console.WriteLine("Porting mods from Nintendo Switch version to Wii U is still work in progress.");
                return;
            }

            if(Directory.Exists(Path.Join(directory, "ObjectData"))) {
                Directory.CreateDirectory(Path.Join(output, "ObjectData"));

                foreach(FileInfo file in new DirectoryInfo(Path.Join(directory, "ObjectData")).GetFiles()) {
                    IntersectionDokan.SZSCompress(
                        ObjectDokan.Obj2Switch(IntersectionDokan.SZSDecompress(file.FullName)),
                        Path.Join(output, "ObjectData/", file.Name));
                }
            }

            if(Directory.Exists(Path.Join(directory, "StageData"))) {
                Directory.CreateDirectory(Path.Join(output, "StageData"));

                List<string> names = new();
                foreach(FileInfo file in new DirectoryInfo(Path.Join(directory, "StageData")).GetFiles()) {
                    if(file.Extension != ".szs")
                        continue;

                    string current = RemoveStringEnd(file.FullName);
                    if(!names.Contains(current)) names.Add(current);
                }

                foreach(string stage in names) {
                    Dictionary<StageDokan.StageType, SarcData> stageData = new();

                    if(File.Exists(stage + "Design1.szs"))
                        stageData.Add(StageDokan.StageType.Design, 
                            IntersectionDokan.SZSDecompress(stage + "Design1.szs"));

                    if(File.Exists(stage + "Map1.szs"))
                        stageData.Add(StageDokan.StageType.Map,
                            IntersectionDokan.SZSDecompress(stage + "Map1.szs"));

                    if(File.Exists(stage + "Sound1.szs"))
                        stageData.Add(StageDokan.StageType.Sound,
                            IntersectionDokan.SZSDecompress(stage + "Sound1.szs"));

                    IntersectionDokan.SZSCompress(StageDokan.Stage2Switch(stageData), 
                        Path.Join(output, "StageData/", new FileInfo(stage).Name + ".szs"));
                }
            }
        }

        static string RemoveStringEnd(string raw) {
            if(raw.EndsWith("Map1.szs"))
                return raw.Remove(raw.Length - 8);
            if(raw.EndsWith("Design1.szs"))
                return raw.Remove(raw.Length - 11);
            if(raw.EndsWith("Sound1.szs"))
                return raw.Remove(raw.Length - 10);
            return raw;
        }
    }
}
