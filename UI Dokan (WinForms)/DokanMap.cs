using DokanGroups;
using Microsoft.WindowsAPICodePack.Dialogs;
using Syroot.BinaryData;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace UIDokan {
    public partial class DokanMap : Form {
        private string inputDirectory = "";
        private bool isWiiU = true;
        private bool saved = false;

        public DokanMap() {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e) {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, System.EventArgs e) {
            MessageBox.Show("About");
        }

        private void openToolStripMenuItem_Click(object sender, System.EventArgs e) {
            CommonOpenFileDialog openDlg = new() {
                IsFolderPicker = true,
                Title = "Please select your mods folder folder..."
            };

            if(openDlg.ShowDialog().Equals(CommonFileDialogResult.Ok))
                CommonOpen(openDlg.FileName);
        }

        private void dokanMap_DragEnter(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Copy;
        }

        private void dokanMap_DragDrop(object sender, DragEventArgs e) {
            this.Activate();
            CommonOpen(((string[]) e.Data.GetData(DataFormats.FileDrop))[0]);
        }

        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e) {
            Pen pen = new(Color.Black, 3);
            e.Graphics.DrawLine(pen, 100, 50, 100, 150);
            e.Graphics.DrawLine(pen, 50, 100, 150, 100);
        }

        private void CommonOpen(string directory) {
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
                MessageBox.Show("This folder isn't valid, please select a mod folder.",
                    "Invalid",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            pictureBox1.Visible = false;
            pictureBox2.Visible = true;
            portButton.Visible = true;

            if(IntersectionDokan.SZSDecompress(
                new DirectoryInfo(directory).GetDirectories()[0].GetFiles()[0].FullName)
                .endianness == ByteOrder.BigEndian) {
                pictureBox2.Image = AnimationData.Animation1;
                label1.Text = "Ready (Wii U)";
            } else {
                MessageBox.Show("Porting mods from Nintendo Switch version to Wii U is still work in progress.",
                    "Uncompatible", MessageBoxButtons.OK, MessageBoxIcon.Information);

                pictureBox1.Visible = true;
                pictureBox2.Visible = false;
                portButton.Visible = false;
                label1.Text = "Open a mod folder or drag it here.";
                return;
            }

            inputDirectory = directory;
        }

        private void portButton_Click(object sender, System.EventArgs e) {
            CommonOpenFileDialog saveDlg = new() {
                IsFolderPicker = true,
                Title = "Select where you want to save the ported mod..."
            };

            if(saveDlg.ShowDialog().Equals(CommonFileDialogResult.Ok)) {
                BackgroundWorker worker = new();
                BackgroundWorker animator = new();

                portButton.Enabled = false;

                worker.DoWork += (object sender, DoWorkEventArgs e) => {
                    ConsoleDokan.DokanEntrance.Main(new string[] { inputDirectory, saveDlg.FileName, isWiiU.ToString() });
                    saved = true;
                    //label1.Text = "Done.";
                    //portButton.Enabled = true;

                    Process explorer = new();
                        explorer.StartInfo.FileName = "explorer.exe";
                        explorer.StartInfo.Arguments = saveDlg.FileName;
                        explorer.EnableRaisingEvents = true;
                    explorer.Start();
                };

                animator.DoWork += (object sender, DoWorkEventArgs e) => {
                    Thread.Sleep(1000);
                    pictureBox2.Image = AnimationData.Animation2;
                    Thread.Sleep(500);
                    pictureBox2.Image = AnimationData.Animation3;
                    Thread.Sleep(500);
                    pictureBox2.Image = AnimationData.Animation4;
                    Thread.Sleep(500);
                    pictureBox2.Image = AnimationData.Animation5;
                    Thread.Sleep(500);
                    pictureBox2.Image = AnimationData.Animation6;
                    Thread.Sleep(500);
                    pictureBox2.Image = AnimationData.Animation7;
                    Thread.Sleep(500);
                    pictureBox2.Image = AnimationData.Animation8;
                    Thread.Sleep(1000);

                    //Point previousLocation = pictureBox2.Location;

                    //while(!saved) {
                    //    for(byte i = 0; i != 6; i++)
                    //        pictureBox2.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y + 1);
                    //    Thread.Sleep(700);
                    //    for(byte i = 0; i != 6; i++)
                    //        pictureBox2.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y + -1);
                    //}

                    //pictureBox2.Location = previousLocation;
                };

                animator.RunWorkerAsync();
                worker.RunWorkerAsync();
            }
        }
    }
}
