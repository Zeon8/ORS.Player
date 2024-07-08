using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended.Framework.Media;
using NAudio.Wave;

namespace ORS.Interpreter
{
    public interface IAssetLoader
    {
        Video LoadVideo(string path, string format);

        WaveStream LoadSound(string path);

        string LoadScript(string path);
        //Bitmap? LoadImage(string path);
        //bool TryLoadImage(string path, [NotNullWhen(returnValue: true)] out Bitmap? image);
    }
}
