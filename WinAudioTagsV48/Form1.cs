using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        private const char SeparatorChar = ';';

        private TagLibWrapper.TagLibWrapper _tagLibWrapper = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
#if DEBUG

            this.textBox1.Text = Directory.GetCurrentDirectory() + @"\..\..\..\MP3\05 - Altaelva.mp3";
            //05 - Altaelva
            //03 - Trouble(With a Capital T)_VBR_LAME.mp3
#endif

            //this.textBox1.Text = @"E:\Torrents\DownloadsCompleted\Circa - Valley of the Windmill (2016)\01. Silent Resolve.mp3";
            //this.textBox1.Text = @"E:\Torrents\DownloadsCompleted\Circa - Valley of the Windmill (2016)\02 - Dystopian Overture.flac";

            //this.textBox1.Text = @"E:\Ikarus {2022} [Plasma] @MP3\01 - Tritium.mp3";
        }

        private void buttonSaveInfos_Click(object sender, EventArgs e)
        {
            if (_tagLibWrapper == null)
                return;

            //roles
            textBoxArtists.Text = textBoxAlbumArtist.Text;
            textBoxPerformers.Text = textBoxAlbumArtist.Text;

            //set fields
            _tagLibWrapper.Artists = StringToStringArray(textBoxArtists.Text);
            _tagLibWrapper.AlbumArtists = StringToStringArray(textBoxAlbumArtist.Text);
            _tagLibWrapper.Album = textBoxAlbum.Text;
            _tagLibWrapper.Title = textBoxTitle.Text;
            _tagLibWrapper.Year = StringToUint(textBoxYear.Text);
            _tagLibWrapper.Genres = StringToStringArray(textBoxGenres.Text);
            _tagLibWrapper.Track = StringToUint(textBoxTrack.Text);
            _tagLibWrapper.TrackCount = StringToUint(textBoxTrackCount.Text);
            _tagLibWrapper.Disk = StringToUint(textBoxDisk.Text);
            _tagLibWrapper.DiskCount = StringToUint(textBoxDiskCount.Text);
            _tagLibWrapper.Comment = textBoxComment.Text;
            _tagLibWrapper.Description = textBoxDescription.Text;
            _tagLibWrapper.Performers = StringToStringArray(textBoxPerformers.Text);
            _tagLibWrapper.Composers = StringToStringArray(textBoxComposers.Text);

            _tagLibWrapper.Cover = pictureBox1.Image;

            _tagLibWrapper.SaveAudioFile(this.textBox1.Text, true, true);
        }

        private void buttonReadInfos_Click(object sender, EventArgs e)
        {
        //https://learn.microsoft.com/pt-br/dotnet/desktop/winforms/controls/layout?view=netdesktop-6.0

            _tagLibWrapper = new TagLibWrapper.TagLibWrapper();
            _tagLibWrapper.ReadAudioFile(this.textBox1.Text, true);

            textBoxMimeType.Text = _tagLibWrapper.MimeType;
            textBoxTagTypesOnMemory.Text = _tagLibWrapper.TagTypesOnMemory;
            textBoxTagTypesOnDisk.Text = _tagLibWrapper.TagTypesOnDisk;
            textBoxPossiblyCorrupt.Text = _tagLibWrapper.PossiblyCorrupt.ToString();
            textBoxArtists.Text = StringArrayToString(_tagLibWrapper.Artists);
            textBoxAlbumArtist.Text = StringArrayToString(_tagLibWrapper.AlbumArtists);
            textBoxAlbum.Text = _tagLibWrapper.Album;
            textBoxTitle.Text = _tagLibWrapper.Title;
            textBoxYear.Text = _tagLibWrapper.Year.ToString();
            textBoxGenres.Text = StringArrayToString(_tagLibWrapper.Genres);
            textBoxTrack.Text = _tagLibWrapper.Track.ToString();
            textBoxTrackCount.Text = _tagLibWrapper.TrackCount.ToString();
            textBoxDisk.Text = _tagLibWrapper.Disk.ToString();
            textBoxDiskCount.Text = _tagLibWrapper.DiskCount.ToString();
            textBoxComment.Text = _tagLibWrapper.Comment;
            textBoxPerformers.Text = StringArrayToString(_tagLibWrapper.Performers);
            textBoxComposers.Text = StringArrayToString(_tagLibWrapper.Composers);
            textBoxDescription.Text = _tagLibWrapper.Description;
            textBoxCodecDescription.Text = _tagLibWrapper.CodecDescription;
            textBoxDuration.Text = _tagLibWrapper.Duration.ToString();
            textBoxAudioBitrate.Text = _tagLibWrapper.AudioBitrate.ToString();
            textBoxAudioChannels.Text = _tagLibWrapper.AudioChannels.ToString();
            textBoxAudioSampleRate.Text = _tagLibWrapper.AudioSampleRate.ToString();
            textBoxBitsPerSample.Text = _tagLibWrapper.BitsPerSample.ToString();
            textBoxMediaTypes.Text = _tagLibWrapper.MediaTypes;
            textBoxChannelMode.Text = _tagLibWrapper.ChannelMode;
            textBoxIsVBR.Text = _tagLibWrapper.IsVBR.ToString();
            textBoxAudioVersion.Text = _tagLibWrapper.AudioVersion;
            textBoxAudioDescription.Text = _tagLibWrapper.AudioDescription;

            if (_tagLibWrapper.Cover == null)
            {
                pictureBox1.Image = null;
                textBoxPictureHeight.Text = "";
                textBoxPictureWidth.Text = "";
            }
            else
            {
                pictureBox1.Image = _tagLibWrapper.Cover;
                textBoxPictureHeight.Text = _tagLibWrapper.Cover.Height.ToString();
                textBoxPictureWidth.Text = _tagLibWrapper.Cover.Width.ToString();
            }
        }

        private void buttonReadImage_Click(object sender, EventArgs e)
        {
            string fileName = Directory.GetCurrentDirectory() + @"\..\..\..\MP3\Front.jpg";

            Image image1 = Image.FromFile(fileName);

            this.pictureBox1.Image = image1;

            //FileInfo fileInfo = new FileInfo(fileName);

            //// The byte[] to save the data in
            //byte[] data = new byte[fileInfo.Length];

            //// Load a filestream and put its content into the byte[]
            //using (FileStream fs = fileInfo.OpenRead())
            //{
            //    fs.Read(data, 0, data.Length);
            //}

            //// Delete the temporary file
            //fileInfo.Delete();
        }

        private string StringArrayToString(string[] data)
        {
            StringBuilder stringBuilder = new StringBuilder();

            bool addSparator = false;
            foreach (string item in data)
            {
                if (addSparator)
                    stringBuilder.Append(SeparatorChar);
                else
                    addSparator = true;

                stringBuilder.Append(item);
            }

            return stringBuilder.ToString();
        }
  
        private string[] StringToStringArray(string data)
        {
            return data.Split(SeparatorChar);
        }

        private uint StringToUint(string data)
        {
            uint.TryParse(data, out uint i);
            return i;
        }
    }
}
