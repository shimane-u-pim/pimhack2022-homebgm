using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayerHost
{
    internal class AudioPlayer : IDisposable
    {
        private Dictionary<string, AudioPlayUnit> deviceIdentityAndPlayUnits;

        public AudioPlayer(IDictionary<string, int> deviceIdentityAndDeviceIds)
        {
            deviceIdentityAndPlayUnits = new();
            foreach (var i in deviceIdentityAndDeviceIds)
            {
                deviceIdentityAndPlayUnits.Add(i.Key, new(i.Value));
            }
        }

        public void LoadFile(string audioFile, double delay)
        {
            int count = 0;
            foreach (var i in deviceIdentityAndPlayUnits)
            {
                i.Value.LoadFile(audioFile);
                i.Value.CurrentTime.Add(TimeSpan.FromMilliseconds(delay * count));
                count++;
            }
        }

        public void SetVolume(string id, float volume)
        {
            if (!deviceIdentityAndPlayUnits.ContainsKey(id))
                return;

            deviceIdentityAndPlayUnits[id].Volume = volume;
        }

        public void Play()
        {
            foreach (var i in deviceIdentityAndPlayUnits)
                i.Value.Play();
        }

        public void Stop()
        { 
            foreach (var i in deviceIdentityAndPlayUnits)
                i.Value.Stop();
        }

        public void Pause()
        {
            foreach (var i in deviceIdentityAndPlayUnits)
                i.Value.Pause();
        }

        public void Dispose()
        {
            foreach(var i in deviceIdentityAndPlayUnits)
                i.Value.Dispose();
        }
    }
}
