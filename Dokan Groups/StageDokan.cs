using SARCExt;
using Syroot.BinaryData;
using System.Collections.Generic;

namespace DokanGroups {
    public class StageDokan {
        public enum StageType {
            Design,
            Map,
            Sound
        }

        public static SarcData Stage2Switch(Dictionary<StageType, SarcData> stageFiles) {
            Dictionary<string, byte[]> portedFiles = new();

            foreach(KeyValuePair<StageType, SarcData> sarc in stageFiles) {
                foreach(KeyValuePair<string, byte[]> file in sarc.Value.Files) {
                    if(file.Key.EndsWith("byml"))
                        portedFiles.Add(file.Key, IntersectionDokan.BYMLPort(file.Value));
                    else if(file.Value[0] == 0x41 && file.Value[1] == 0x41 && file.Value[2] == 0x4D && file.Value[3] == 0x50)
                        portedFiles.Add(file.Key, IntersectionDokan.AAMPPort(file.Value));
                }
            }

            return new SarcData() {
                endianness = ByteOrder.LittleEndian,
                Files = portedFiles
            };
        }
    }
}
