
//https://github.com/mono/taglib-sharp
//https://taglib.org/


using System.Collections.Generic;
using System;
using TagLib;
using TagLib.Mpeg;

namespace TagLibWrapper
{
    public class TagLibWrapper
    {
        #region Fields

        private string _lastFileName = "";
        private TagLib.Mpeg.AudioFile? _file = null;

        private string _mimeType = "";
        private string _tagTypes = "";
        private string _tagTypesOnDisk = "";
        private bool _possiblyCorrupt;

        private string[] _artists = System.Array.Empty<string>();
        private string[] _albumArtists = System.Array.Empty<string>();
        private string _album = "";
        private uint _year;
        private string _title = "";
        private string[] _genres = System.Array.Empty<string>();
        private uint _track;
        private uint _trackCount;
        private uint _disk;
        private uint _diskCount;
        private string _comment = "";
        private string _subTitle = "";
        private string _description = "";
        private string[] _performers = System.Array.Empty<string>();
        private string[] _composers = System.Array.Empty<string>();

        private System.Drawing.Image? _cover;

        private string _codecDescription = "";
        private System.TimeSpan _duration = TimeSpan.Zero;
        private int _audioBitrate;
        private int _audioChannels;
        private int _audioSampleRate;
        private int _bitsPerSample;
        private string _mediaTypes = "";
        private string _channelMode = "";

        private bool _isVBR;
        private string _audioVersion = "";
        private string _audioDescription = "";

        #endregion

        #region Properties Get

        public string MimeType
        {
            get { return _mimeType; }
        }

        public string TagTypesOnMemory
        {
            get { return _tagTypes; }
        }

        public string TagTypesOnDisk
        {
            get { return _tagTypesOnDisk; }
        }

        public bool PossiblyCorrupt
        {
            get { return _possiblyCorrupt; }
        }

        //public string SubTitle
        //{
        //    get { return _subTitle; }
        //}

        public System.TimeSpan Duration
        {
            get { return _duration; }
        }

        public string CodecDescription
        {
            get { return _codecDescription; }
        }

        public int AudioBitrate
        {
            get { return _audioBitrate; }
        }

        public int AudioChannels
        {
            get { return _audioChannels; }
        }

        public int AudioSampleRate
        {
            get { return _audioSampleRate; }
        }

        public int BitsPerSample
        {
            get { return _bitsPerSample; }
        }

        public string MediaTypes
        {
            get { return _mediaTypes; }
        }

        public string ChannelMode
        {
            get { return _channelMode; }
        }

        public bool IsVBR
        {
            get { return _isVBR; }
        }

        public string AudioVersion
        {
            get { return _audioVersion; }
        }

        public string AudioDescription
        {
            get { return _audioDescription; }
        }

        #endregion

        #region Properties Get and Set

        public string[] Artists
        {
            get { return _artists; }
            set { _artists = value; }
        }

        public string[] AlbumArtists
        {
            get { return _albumArtists; }
            set { _albumArtists = value; }
        }

        public string Album
        {
            get { return _album; }
            set { _album = value; }
        }

        public uint Year
        {
            get { return _year; }
            set { _year = value; }
        }

        public uint Track
        {
            get { return _track; }
            set { _track = value; }
        }

        public uint TrackCount
        {
            get { return _trackCount; }
            set { _trackCount = value; }
        }
        public uint Disk
        {
            get { return _disk; }
            set { _disk = value; }
        }

        public uint DiskCount
        {
            get { return _diskCount; }
            set { _diskCount = value; }
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string[] Genres
        {
            get { return _genres; }
            set { _genres = value; }
        }

        public string[] Performers
        {
            get { return _performers; }
            set { _performers = value; }
        }

        public string[] Composers
        {
            get { return _composers; }
            set { _composers = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public System.Drawing.Image? Cover
        {
            get { return _cover; }
            set { _cover = value; }
        }

        #endregion

        #region Constructors

        #endregion

        #region Public Methods

        public void ReadAudioFile(string fullFileName, bool loadCover)
        {
            Create(fullFileName);

            ReadAudio(loadCover);

            _lastFileName = fullFileName;
        }

        public void SaveAudioFile(string fullFileName, bool clearFieldsFirst, bool maintainOriginalCover)
        {
            System.Drawing.Image? coverTemp = _cover;

            if ((_file == null) || (fullFileName != _lastFileName))
            {
                Create(fullFileName);
                if (maintainOriginalCover)
                    coverTemp = GetTagCoverToImage();
            }

            if (_file == null)
                throw new Exception("TagLib.Mpeg.AudioFile is null");

            if (clearFieldsFirst)
            {
                _file.Tag.Clear();
                //_file.RemoveTags(TagTypes.AllTags); ///problem here 
                //_file.GetTag(TagTypes.Id3v2, true); //solution
                _file.RemoveTags(TagTypes.Id3v1);
                _file.RemoveTags(TagTypes.Ape);
            }

            //Set Tags to v2.3 version
            TagLib.Id3v2.Tag.DefaultVersion = 3;
            TagLib.Id3v2.Tag.ForceDefaultVersion = true;

            //set fields
            //obsolete _file.Tag.Artists = _artists;
            _file.Tag.AlbumArtists = _albumArtists;
            _file.Tag.Album = _album;
            _file.Tag.Year = _year;
            _file.Tag.Track = _track;
            _file.Tag.TrackCount = _trackCount;
            _file.Tag.Disc = _disk;
            _file.Tag.DiscCount = _trackCount;
            _file.Tag.Title = _title;
            _file.Tag.Genres = _genres;
            _file.Tag.Performers = _performers;
            _file.Tag.Composers = _composers;
            _file.Tag.Comment = _comment;
            _file.Tag.Description = _description;

            SetImageToTagCover(coverTemp);

            _file.Save();
        }

        #endregion

        #region Private Methods

        private void Create(string fullFileame)
        {
            TagLib.File file = TagLib.File.Create(fullFileame);

            if (file.GetType() != typeof(TagLib.Mpeg.AudioFile))
                throw new Exception("File is not an 'Mpeg Audio'");

            _file = file as TagLib.Mpeg.AudioFile;
        }

        private void ClearFields()
        {
            _lastFileName = "";

            _possiblyCorrupt = false;
            _mimeType = "";
            _tagTypes = "";
            _tagTypesOnDisk = "";

            _artists = System.Array.Empty<string>();
            _albumArtists = System.Array.Empty<string>();
            _album = "";
            _title = "";
            _year = 0;
            _track = 0;
            _trackCount = 0;
            _disk = 0;
            _diskCount = 0;
            _comment = "";
            _subTitle = "";
            _description = "";
            _genres = System.Array.Empty<string>();
            _performers = System.Array.Empty<string>();

            _duration = TimeSpan.Zero;
            _codecDescription = "";
            _audioBitrate = 0;
            _audioChannels = 0;
            _audioSampleRate = 0;
            _bitsPerSample = 0;
            _mediaTypes = "";
            _channelMode = "";
            _isVBR = false;
            _audioVersion = "";
            _audioDescription = "";

            _cover = null;
        }

        private void ReadAudio(bool loadCover)
        {
            //private string _description; /// <summary>
            if (_file == null)
                throw new Exception("TagLib.Mpeg.AudioFile is null");

            ClearFields();

            //from AudioFile
            _mimeType = _file.MimeType;
            TagTypes tagTypes = _file.TagTypes;
            TagTypes tagTypesOnDisk = _file.TagTypesOnDisk;
            _tagTypes = tagTypes.ToString();
            _tagTypesOnDisk = tagTypesOnDisk.ToString();
            _possiblyCorrupt = _file.PossiblyCorrupt;

            //Warning CS0618  'Tag.Artists' is obsolete:
            //'For album artists use AlbumArtists. For track artists, use Performers'

            //from AudioFile.Tag
            if ((_file.Tag.Performers != null) && (_file.Tag.Performers.Length > 0))
            {
                _performers = _file.Tag.Performers;
                _artists = _performers;
            }

            if ((_file.Tag.AlbumArtists != null) && (_file.Tag.AlbumArtists.Length > 0))
                _albumArtists = _file.Tag.AlbumArtists;

            if (_file.Tag.Title != null)
                _title = _file.Tag.Title;

            if (_file.Tag.Album != null)
                _album = _file.Tag.Album;

            _year = _file.Tag.Year;
            _track = _file.Tag.Track;
            _trackCount = _file.Tag.TrackCount;
            _disk = _file.Tag.Disc;
            _diskCount = _file.Tag.DiscCount;
            _comment = _file.Tag.Comment;
            _subTitle = _file.Tag.Subtitle;
            _description = _file.Tag.Description;

            if ((_file.Tag.Genres != null) && (_file.Tag.Genres.Length > 0))
                _genres = _file.Tag.Genres;


            //from File.Properties
            _duration = _file.Properties.Duration;

            if (_file.Properties.Description != null)
                _codecDescription = _file.Properties.Description;

            MediaTypes mediaTypes = _file.Properties.MediaTypes;
            _mediaTypes = mediaTypes.ToString();

            _audioBitrate = _file.Properties.AudioBitrate;
            _audioChannels = _file.Properties.AudioChannels;
            _audioSampleRate = _file.Properties.AudioSampleRate;
            _bitsPerSample = _file.Properties.BitsPerSample;

            //from File.Properties.ICodec[]
            GetAudioCodecInfo();

            if (loadCover)
                _cover = GetTagCoverToImage();
        }

        private void GetAudioCodecInfo()
        {
            if (_file == null)
                throw new Exception("TagLib.Mpeg.AudioFile is null");

            if (_file.Properties.Codecs == null)
                return;

            TagLib.Mpeg.AudioHeader audioHeader = default;

            IEnumerable<ICodec> codecs = _file.Properties.Codecs;

            //https://www.youtube.com/watch?v=LUa6RSLWJLI

            //v1
            IEnumerator<ICodec> enumerator = codecs.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ICodec first = enumerator.Current;
                if (first.GetType() == typeof(TagLib.Mpeg.AudioHeader))
                    audioHeader = (TagLib.Mpeg.AudioHeader)first;
                else
                    return;
            }

            //V2
            //List<ICodec> codecs4 = _file.Properties.Codecs.ToList();
            //if (codecs4.Count > 0)
            //{
            //    if (codecs4[0].GetType() == typeof(TagLib.Mpeg.AudioHeader))
            //        audioHeader = (TagLib.Mpeg.AudioHeader)codecs4[0];
            //    else
            //        return;
            //    }
            //}

            //V3
            //if (codecs.ElementAt(0) == null) //System.ArgumentOutOfRangeException: 'Index was out of range. Must be non-negative and less than the size of the collection.                Parameter name: index'
            //    return;
            //ICodec first2 = codecs.ElementAt(0);
            //audioHeader = (TagLib.Mpeg.AudioHeader)first2;

            //V4
            //bool found = false;
            //foreach (var codec in codecs)
            //{ 
            //    if (codec.GetType() == typeof(TagLib.Mpeg.AudioHeader))
            //    {
            //        audioHeader = (TagLib.Mpeg.AudioHeader)codec;
            //        found = true;
            //        break;
            //    }
            //}

            //if (!found)
            //    return;

            ///////////

            //get infos
            ChannelMode channelMode = audioHeader.ChannelMode;
            _channelMode = channelMode.ToString();

            bool bVBRIHeader = audioHeader.VBRIHeader.Present;
            bool bXingHeader = audioHeader.XingHeader.Present;

            _isVBR = bVBRIHeader || bXingHeader;
            _audioDescription = audioHeader.Description;
            _audioVersion = audioHeader.Version.ToString();
        }

        private System.Drawing.Image? GetTagCoverToImage()
        {
            if (_file == null)
                throw new Exception("TagLib.Mpeg.AudioFile is null");

            if (_file.Tag.Pictures == null)
                return null;

            if (_file.Tag.Pictures.Length == 0)
                return null;

            TagLib.IPicture pic = _file.Tag.Pictures[0];

            byte[] bytes = (byte[])(pic.Data.Data);

            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            System.Drawing.Image? image = System.Drawing.Image.FromStream(ms);

            return image;
        }

        private void SetImageToTagCover(System.Drawing.Image? image)
        {
            if (_file == null)
                return;

            if (image == null)
                return;

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] bytes = ms.ToArray();

            TagLib.ByteVector bv = new TagLib.ByteVector(bytes);
            TagLib.IPicture pic = new TagLib.Picture(bv);

            TagLib.Id3v2.AttachmentFrame frame = new TagLib.Id3v2.AttachmentFrame();
            //frame.FrameId = "APIC";
            frame.MimeType = "image/jpg";
            frame.Type = PictureType.FrontCover;

            IPicture[] pictures = new IPicture[1];
            pictures[0] = pic;

            _file.Tag.Pictures = pictures;
        }

        private System.Drawing.Image ScaleImage(System.Drawing.Image image, int maxWidth, int maxHeight)
        {
            double ratioX = (double)(maxWidth / image.Width);
            double ratioY = (double)(maxHeight / image.Height);
            double ratio = System.Math.Min(ratioX, ratioY);

            int newWidth = (int)(image.Width * ratio);
            int newHeight = (int)(image.Height * ratio);

            System.Drawing.Bitmap newImage = new System.Drawing.Bitmap(newWidth, newHeight);

            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(newImage);
            graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        #endregion
    }
}

//remembers
//ICodec codecs3 = _file.Properties.Codecs.FirstOrDefault();
//TagLib.Mpeg.AudioHeader audioHeader1 = (TagLib.Mpeg.AudioHeader)codecs3;


////var codecs1 = _file.Properties.Codecs;
//IEnumerable<ICodec> codecs2 = _file.Properties.Codecs;

//object obj = codecs2.ElementAt(0);

//IEnumerator enumerator = codecs2.GetEnumerator();
//enumerator.MoveNext();
//object first = enumerator.Current;
//TagLib.Mpeg.AudioHeader audioHeader = (TagLib.Mpeg.AudioHeader) first;


//TagLib.Mpeg.AudioHeader 
//      var zzz = codecs1.FirstOrDefault<ICodec>;


//  var codecsx = codecs[0];