using Avalonia.Media.Imaging;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORS.Interpreter
{
    internal interface IAssetLoader
    {
        Media LoadMediaAsset(string path, string format);
        string LoadScript(string path);
        Bitmap? LoadImage(string path);
        bool TryLoadImage(string path, [NotNullWhen(returnValue: true)] out Bitmap? image);
    }
}
