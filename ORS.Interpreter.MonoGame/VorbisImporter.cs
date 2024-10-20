using Microsoft.Xna.Framework.Audio;
using NVorbis;
using System;
using System.IO;

namespace ORS.Player
{
    public class VorbisImporter
    {
        public static SoundEffect Open(string path)
        {
            byte[] buffer;
            int sampleRate;
            int channels;

            using (var vorbis = new VorbisReader(path))
            {
                sampleRate = vorbis.SampleRate;
                channels = vorbis.Channels;

                using var stream = new MemoryStream();
                Span<float> readBuffer = new float[4096];
                int samples;

                while ((samples = vorbis.ReadSamples(readBuffer)) > 0)
                {
                    for (int i = 0; i < samples; i++)
                    {
                        short sample = (short)(readBuffer[i] * short.MaxValue);
                        stream.WriteByte((byte)(sample & 0xff));
                        stream.WriteByte((byte)((sample >> 8) & 0xff));
                    }
                }

                buffer = stream.ToArray();
            }

            // Create a SoundEffect from the decoded PCM data
            return new SoundEffect(buffer, sampleRate, (AudioChannels)channels);
        }
    }
}
