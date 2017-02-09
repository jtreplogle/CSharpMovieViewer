using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpBasicMediaPlayer
{
    public partial class Form1 : Form
    {
        WMPLib.IWMPPlaylist playlist;
        public Form1()
        {
            InitializeComponent();
            playlist = axWindowsMediaPlayer1.playlistCollection.newPlaylist("myplaylist");
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog();
            //Multiselect ~ If the user wants to play an entire set/list/album.
            od.Multiselect = true;
            //Filter types of media files:
            od.Filter = "(mp3, wav, mp4, mov, wmv, 3gp, mpg, flv) | *.mp3; *.wav; *.mp4; *.mov; *.wmv; *.3pg; *.mpg; *.flv | all files | *.*";
            if(od.ShowDialog() == DialogResult.OK)
            {
                if(listBox1.Items.Count > 0)
                {
                    listBox1.Items.Clear();
                    axWindowsMediaPlayer1.currentPlaylist.clear();
                }
                for(int i = 0; i < od.SafeFileNames.Length; i++)
                {
                    listBox1.Items.Add(od.SafeFileNames[i]);
                    WMPLib.IWMPMedia media = axWindowsMediaPlayer1.newMedia(od.FileNames[i]);
                    playlist.appendItem(media);
                }
            }
            axWindowsMediaPlayer1.currentPlaylist = playlist;
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        //Listbox ~ Visible meida file q.
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.Items.Count > 0)
            {
                //Create new media from listbox.
                WMPLib.IWMPMedia media = axWindowsMediaPlayer1.currentPlaylist.get_Item(listBox1.SelectedIndex);
                //Play media.
                axWindowsMediaPlayer1.Ctlcontrols.playItem(media);
            }
        }

        //(+)Button functionality:  Allows user to Add songs to Listbox via local files.
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog();
            //Multiselect ~ If the user wants to play an entire set/list/album.
            od.Multiselect = true;
            od.Filter = "(mp3, wav, mp4, mov, wmv, 3gp, mpg, flv) | *.mp3; *.wav; *.mp4; *.mov; *.wmv; *.3pg; *.mpg; *.flv | all files | *.*";
            if (od.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < od.SafeFileNames.Length; i++)
                {
                    listBox1.Items.Add(od.SafeFileNames[i]);
                    WMPLib.IWMPMedia media = axWindowsMediaPlayer1.newMedia(od.FileNames[i]);
                    axWindowsMediaPlayer1.currentPlaylist.appendItem(media);
                }
            }
        }
        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (axWindowsMediaPlayer1.currentMedia != null || listBox1.Items.Count <= 0)
                return;
            for (int i = 0; i < axWindowsMediaPlayer1.currentPlaylist.count; i++)
            {
                if (axWindowsMediaPlayer1.currentMedia.get_isIdentical(axWindowsMediaPlayer1.currentPlaylist.Item[i]))
                    listBox1.SelectedIndex = i;
            }
        }

        //(-)Remove button method.
        private void button2_Click(object sender, EventArgs e)
        {
            //Stored selected itme from Listbox.
            int selected = listBox1.SelectedIndex;
            //Get media.
            WMPLib.IWMPMedia media = axWindowsMediaPlayer1.currentPlaylist.get_Item(selected);
            //Remove item.
            axWindowsMediaPlayer1.currentPlaylist.removeItem(media);
            //Stop the player.
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            //Remove selected item.
            listBox1.Items.RemoveAt(selected);
        }
    }
}
