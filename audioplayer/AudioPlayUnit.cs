using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayerHost
{
    internal class AudioPlayUnit : IDisposable
    {
        private WaveOut waveOut;

        private AudioFileReader? reader = null;

        public AudioPlayUnit(int deviceId)
        {
            waveOut = new WaveOut() {
                DeviceNumber = deviceId,
            };
        }

        public void LoadFile(string file)
        {
            if (waveOut.PlaybackState != PlaybackState.Stopped)
                waveOut.Stop();

            if (reader != null)
            { 
                reader.Dispose();
            }

            reader = new AudioFileReader(file);
            waveOut.Init(reader);
        }

        public void Play() => waveOut.Play();

        public void Pause() => waveOut.Pause();

        public void Stop() => waveOut.Stop();

        public void Dispose()
        {
            waveOut.Dispose();
            if (reader != null)
                reader.Dispose();
        }

        public long Position { get => reader == null ? 0 : reader.Position; }

        public TimeSpan CurrentTime { get => reader == null ? TimeSpan.Zero : reader.CurrentTime; }

        public PlaybackState PlaybackState { get => waveOut.PlaybackState; }

        public float Volume { get => waveOut.Volume; set => waveOut.Volume = value; }
    }
}
