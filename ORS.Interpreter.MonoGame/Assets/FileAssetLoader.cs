using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ORS.Player.Media;
using System.IO;

namespace ORS.Player.Assets
{
    internal class FileAssetLoader : IAssetLoader
    {
        private const string ScriptFileExtension = ".ORS";
        private const string AudioFileExtension = ".OGG";
        private const string VideoFileExtension = ".WMV";
        private const string ImageFileExtension = ".PNG";

        private readonly string _basePath;
        private readonly GraphicsDevice _device;

        public FileAssetLoader(string basePath, GraphicsDevice graphicsDevice)
        {
            _basePath = basePath;
            _device = graphicsDevice;
        }

        public string LoadScript(string path)
        {
            path = GetAssetPath(path, ScriptFileExtension);
            return File.ReadAllText(path);
        }

        public SoundEffectInstance LoadAudio(string path)
        {
            path = GetAssetPath(path, AudioFileExtension);
            if (!File.Exists(path))
                return null;

            var sound = VorbisImporter.Open(path);
            return sound.CreateInstance();
        }

        public Video LoadVideo(string path)
        {
            path = GetAssetPath(path, VideoFileExtension);
            if (!File.Exists(path))
                return null;

            return VideoDecoder.Decode(path, _device);
        }

        public Texture2D LoadImage(string path)
        {
            path = GetAssetPath(path, ImageFileExtension);
            if (!File.Exists(path))
                return null;

            return Texture2D.FromFile(_device, path);
        }

        private string GetAssetPath(string path, string extension)
        {
            return Path.Combine(_basePath, path + extension);
        }
    }
}
