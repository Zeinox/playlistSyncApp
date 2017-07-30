using AxWMPLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;






namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public string[] validFileExtensions = { ".mp3", ".wav", ".aa", ".aiff", ".flac", ".wma" };
        public Boolean newSong = false;
        public AxWindowsMediaPlayer Player;
        public WMPLib.IWMPPlaylist playlist;
        public string dir;

        public Form1()
        {
            InitializeComponent();
            
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form_DragEnter);
            this.DragDrop += new DragEventHandler(Form_DragDrop);
            Player = new AxWindowsMediaPlayer();
            Player.CreateControl();
            dir = "C:\\0";
            //axMDocView1.CreateControl()
        }

        void Form_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void Form_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            for (int i = 0; i < FileList.Length; ++i)
            {
                string fileExt = Path.GetExtension(FileList[i]);


                if (validFileExtensions.Contains(fileExt))
                {
                    playList.Items.Add(FileList[i]);
                }
            }
            //more processing
        }

        //Thanks to stackoverflow: https://stackoverflow.com/questions/805165/reorder-a-winforms-listbox-using-drag-and-drop
        //
        private void playList_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.playList.SelectedItem == null) return;
            this.playList.DoDragDrop(this.playList.SelectedItem, DragDropEffects.Move);
        }

        private void playList_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void playList_DragDrop(object sender, DragEventArgs e)
        {
            if(newSong)
            {
                Form_DragDrop(sender, e);
                newSong = false;
                return;
            }

            Point point = playList.PointToClient(new Point(e.X, e.Y));
            int index = this.playList.IndexFromPoint(point);
            if (index < 0) index = this.playList.Items.Count - 1;
            object data = e.Data.GetData(typeof(string));
            this.playList.Items.Remove(data);
            this.playList.Items.Insert(index, data);
        }

        //

        void playList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
                newSong = true;
            }
        }

        private void playList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] songsArray = playList.Items.OfType<string>().ToArray();
            try
            {
                savePlaylist.InitialDirectory = Directory.GetCurrentDirectory();
            }
            catch(Exception ex)
            {
                Console.WriteLine("The process faile: {0}", ex.ToString());
            }

            savePlaylist.Title = "Save Playlist";
            savePlaylist.FileName = "";
            savePlaylist.Filter = "Playlist|*.m3u|All Files|*.*";

            if(savePlaylist.ShowDialog()!= DialogResult.Cancel)
            {
              
                System.IO.File.WriteAllLines(Path.GetFullPath(savePlaylist.FileName), songsArray);
                
                //System.IO.File.WriteAllLines()
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (playList.Items.Count > 0)
            {
                string[] songsArray = playList.Items.OfType<string>().ToArray();

                playlist = Player.playlistCollection.newPlaylist(namePlaylist.Text);

                foreach (string song in songsArray)
                {
                    playlist.appendItem(Player.newMedia(song));
                }
            }
            
          

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (playList.SelectedIndex > -1)
            {
                playList.Items.RemoveAt(playList.SelectedIndex);
            }
        }

        private void setMusicDir_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                dir = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
