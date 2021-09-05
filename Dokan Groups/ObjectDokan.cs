using System.Collections.Generic;
using SARCExt;
using Syroot.BinaryData;

namespace DokanGroups {
    public class ObjectDokan {
        public static SarcData Obj2Switch(SarcData sarc) {
            SarcData portedSarc = new SarcData() {
                endianness = ByteOrder.LittleEndian,
                Files = new Dictionary<string, byte[]>()
            };

            foreach(KeyValuePair<string, byte[]> file in sarc.Files) {
                switch(file.Key.Substring(file.Key.LastIndexOf('.') + 1)) {
                    case "byml":
                        portedSarc.Files.Add(file.Key, IntersectionDokan.BYMLPort(file.Value));
                        break;
                    case "bfres":
                        portedSarc.Files.Add(file.Key, IntersectionDokan.BFRESPort(file.Value));
                        break;
                    case "kcl":
                        portedSarc.Files.Add(file.Key, IntersectionDokan.KCLPort(file.Value));
                        break;
                    default:
                        portedSarc.Files.Add(file.Key, file.Value);
                        break;
                }

                if(file.Key.Equals("PlayerConst.byml"))
                    portedSarc.Files.Add("PlayerConstOld.byml", file.Value);
            }

            return portedSarc;
        }
    }
}
