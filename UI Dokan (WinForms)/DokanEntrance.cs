using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DokanGroups;

namespace UIDokan {
    static class DokanEntrance {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            // Enables Shift-JIS encoding
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("Shift-JIS");

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DokanMap());

            //string path = @"M:\Modding\3DW\temp\octoomba mania\content\";
            //string output = @"M:\Modding\3DW\temp\octoomba mania switch\";

            //Directory.CreateDirectory(Path.Join(output, "ObjectData"));

            //// ObjectData
            //foreach(FileInfo file in new DirectoryInfo(Path.Join(path, "ObjectData")).GetFiles()) {
            //    IntersectionDokan.SZSCompress(
            //        ObjectDokan.Obj2Switch(IntersectionDokan.SZSDecompress(file.FullName)),
            //        Path.Join(output, "ObjectData/", file.Name));
            //}

            //// StageData
            //foreach(FileInfo file in new DirectoryInfo(Path.Join(path, "StageData")).GetFiles()) {
            //    IntersectionDokan.SZSCompress(
            //        StageDokan.Stage2Switch(IntersectionDokan.SZSDecompress(file.FullName)),
            //        Path.Join(output, "ObjectData/", file.Name));
            //}
        }
    }
}
