using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended.Framework.Media;
using MonoGame.Extended.VideoPlayback;
using NAudio.Vorbis;
using NAudio.Wave;
using System.IO;

namespace ORS.Interpreter
{
    internal class FileAssetLoader : IAssetLoader
    {
        private readonly string _basePath;

        public FileAssetLoader(string basePath)
        {
            _basePath = basePath;
        }

        public string LoadScript(string path)
        {
            path = Path.Combine(_basePath, "Script", path + ".ORS");
            return File.ReadAllText(path);
        }

        public WaveStream LoadSound(string path)
        {
            path = Path.Combine(_basePath, path.ToUpper() + ".OGG");
            return new VorbisWaveReader(path);
        }

        public Video LoadVideo(string path, string format)
        {
            path = Path.Combine(_basePath, path.ToUpper() + format);
            return VideoHelper.LoadFromFile(path);
        }

        //public Bitmap? LoadImage(string path)
        //{
        //    path = Path.Join(_basePath, path.ToUpper());
        //    if (!File.Exists(path))
        //        return null;

        //    return new (path);
        //}

        //public bool TryLoadImage(string path, [NotNullWhen(returnValue: true)] out Bitmap? bitmap)
        //{
        //    bitmap = LoadImage(path);
        //    return bitmap is not null;
        //}
    }
}
