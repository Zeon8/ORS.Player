using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ORS.Player;

namespace ORS.Interpreter
{
    public interface IAssetLoader
    {
        VideoDecoder LoadVideo(string path);

        SoundEffectInstance LoadAudio(string path);

        string LoadScript(string path);

        Texture2D LoadImage(string path);
    }
}
