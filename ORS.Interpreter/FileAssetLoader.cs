using Avalonia.Media.Imaging;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORS.Interpreter
{
    internal class FileAssetLoader : IAssetLoader
    {
        private readonly string _basePath;
        private readonly LibVLC _libVlc;

        public FileAssetLoader(LibVLC libVlc, string basePath)
        {
            _libVlc = libVlc;
            _basePath = basePath;
        }

        public Media LoadMediaAsset(string path, string format)
        {
            path = Path.Join(_basePath, path.ToUpper() + format);
            return new Media(_libVlc, path, FromType.FromPath);
        }

        public string LoadScript(string path)
        {
            path = Path.Join(_basePath, "Script", path);
            return File.ReadAllText(path);
        }

        public Bitmap? LoadImage(string path)
        {
            path = Path.Join(_basePath, path.ToUpper());
            if (!File.Exists(path))
                return null;

            return new Bitmap(path);
        }

        public bool TryLoadImage(string path, out Bitmap? bitmap)
        {
            bitmap = LoadImage(path);
            return bitmap is not null;
        }
    }
}
