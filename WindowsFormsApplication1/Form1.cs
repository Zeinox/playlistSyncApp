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

        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form_DragEnter);
            this.DragDrop += new DragEventHandler(Form_DragDrop);
        }

        void Form_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void Form_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            string fileExt= Path.GetExtension(FileList[0]);
         
            
            if (validFileExtensions.Contains(fileExt))
            {
                playList.Items.Add(FileList[0]);
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
        
    }
}
