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

        private void playList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
