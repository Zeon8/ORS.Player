using Avalonia.Animation;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ORS.Interpreter
{
    public partial class Background : ObservableObject
    {
        [ObservableProperty] private Bitmap? _image;
        [ObservableProperty] private bool _visible;

        [ObservableProperty] private Bitmap? _animatedImage;
        [ObservableProperty] private bool _animated;

        public void SetBackground(Bitmap bitmap)
        {
            Image = bitmap;
            Visible = true;
        }

        public async Task PlayAnimation(Bitmap[] sprites)
        {
            Animated = true;
            int i = 0;
            while (Animated)
            {
                if (i == 2)
                    i = Random.Shared.Next(1, 3);
                else
                    i = Random.Shared.Next(0, 3);

                await SetSprite(sprites[i]);
            }
        }

        private async Task SetSprite(Bitmap sprite)
        {
            AnimatedImage = sprite;
            await Task.Delay(100);
        }

        public void Hide()
        {
            StopAnimation();
            Visible = false;
        }

        internal void StopAnimation() => Animated = false;
    }
}
