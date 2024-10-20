using Microsoft.Xna.Framework.Graphics;
using Sdcb.FFmpeg.Codecs;
using Sdcb.FFmpeg.Raw;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ORS.Player
{
    public unsafe class VideoDecoder
    {
        private AVFormatContext* _formatContext;
        private AVCodecContext* _codecContext;
        private AVFrame* _frame;
        private AVFrame* _rgbFrame;
        private AVPacket* _packet;
        private SwsContext* _swsContext;
        private int _videoStreamIndex;
        private bool _disposed;

        public VideoDecoder(string filePath)
        {
            Load(filePath);
        }

        ~VideoDecoder() => Dispose(false);

        private void Load(string filePath)
        {
            ffmpeg.avdevice_register_all();

            _formatContext = ffmpeg.avformat_alloc_context();
            AVDictionary* options = null;
            if (ffmpeg.avformat_open_input(ref _formatContext, filePath, null, ref options) != 0)
            {
                throw new ApplicationException("Could not open video file.");
            }

            if (ffmpeg.avformat_find_stream_info(_formatContext, null) != 0)
            {
                throw new ApplicationException("Could not find stream info.");
            }

            for (int i = 0; i < _formatContext->nb_streams; i++)
            {
                if (_formatContext->streams[i]->codecpar->codec_type == AVMediaType.Video)
                {
                    _videoStreamIndex = i;
                    break;
                }
            }

            if (_videoStreamIndex == -1)
            {
                throw new ApplicationException("Could not find a video stream.");
            }

            AVCodecParameters* codecParameters = _formatContext->streams[_videoStreamIndex]->codecpar;
            AVCodec* codec = ffmpeg.avcodec_find_decoder(codecParameters->codec_id);
            if (codec == null)
            {
                throw new ApplicationException("Unsupported codec.");
            }

            _codecContext = ffmpeg.avcodec_alloc_context3(codec);

            if (ffmpeg.avcodec_parameters_to_context(_codecContext, codecParameters) < 0)
            {
                throw new ApplicationException("Could not initialize codec codecContext.");
            }

            if (ffmpeg.avcodec_open2(_codecContext, codec, null) < 0)
            {
                throw new ApplicationException("Could not open codec.");
            }

            _frame = ffmpeg.av_frame_alloc();
            _rgbFrame = ffmpeg.av_frame_alloc();
            _packet = ffmpeg.av_packet_alloc();

            _swsContext = ffmpeg.sws_getContext(_codecContext->width, _codecContext->height, _codecContext->pix_fmt,
                                                _codecContext->width, _codecContext->height, AVPixelFormat.Rgba,
                                                (int)SWS.Bilinear, null, null, null);
        }

        public Texture2D GetNextFrame(GraphicsDevice graphicsDevice)
        {
            while (ffmpeg.av_read_frame(_formatContext, _packet) >= 0)
            {
                if (_packet->stream_index == _videoStreamIndex)
                {
                    if (ffmpeg.avcodec_send_packet(_codecContext, _packet) < 0)
                    {
                        continue;
                    }

                    if (ffmpeg.avcodec_receive_frame(_codecContext, _frame) == 0)
                    {
                        var texture = new Texture2D(graphicsDevice, _frame->width, _frame->height, false, SurfaceFormat.Color);
                        byte[] data = new byte[_frame->width * _frame->height * 4];

                        IntPtr dataPtr = _frame->data[0];
                        int lineSize = _frame->linesize[0];

                        fixed (byte* ptr = &data[0])
                        {
                            byte*[] srcData = { ptr, null, null, null };
                            int[] srcLinesize = { _frame->width * 4, 0, 0, 0 };
                            // convert video frame to the RGB data
                            ffmpeg.sws_scale(_swsContext, _frame->data.ToRawArray(), _frame->linesize.ToArray(), 0, _frame->height, srcData, srcLinesize);
                        }
                        texture.SetData(data);
                        ffmpeg.av_packet_unref(_packet);
                        return texture;
                    }
                    else
                    {
                        ffmpeg.av_packet_unref(_packet);
                        continue;
                    }
                }
                ffmpeg.av_packet_unref(_packet);
            }

            return null;
        }

        public void Dispose() => Dispose(true);

        private void Dispose(bool disposing)
        {
            if(!_disposed)
            {
                ffmpeg.av_frame_free(ref _frame);
                ffmpeg.av_packet_free(ref _packet);
                ffmpeg.avcodec_free_context(ref _codecContext);
                ffmpeg.avformat_close_input(ref _formatContext);

                _disposed = true;
            }
        }
    }

}
