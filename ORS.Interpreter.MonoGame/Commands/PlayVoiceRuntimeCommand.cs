using Microsoft.Xna.Framework.Audio;
using ORS.Player.Components;
using System;

namespace ORS.Player.Commands
{
    public class PlayVoiceRuntimeCommand : PlaySoundRuntimeCommand
    {
        public PlayVoiceRuntimeCommand(TimeSpan beginTime, TimeSpan endTime, 
            SoundEffectInstance sound, SoundManager soundManager) 
            : base(beginTime, endTime, sound, soundManager)
        {
        }
    }
}
