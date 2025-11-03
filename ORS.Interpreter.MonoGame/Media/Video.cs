using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ORS.Player.Media;

public class Video : IDisposable
{
    public IReadOnlyList<Texture2D> Frames => _frames;

    private bool _disposedValue;

    private readonly List<Texture2D> _frames;

    public Video(List<Texture2D> frames)
    {
        _frames = frames;
    }

    public void Dispose()
    {
        if (_disposedValue)
            return;

        _frames.ForEach(f => f.Dispose());
        GC.SuppressFinalize(this);

        _disposedValue = true;
    }
}
