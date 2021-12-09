using System;
using NAudio;
using NAudio.Wave;

namespace AudioPlayerHost
{
    public class Program
    {
        const double AUDIO_TIMING = 45;

        const string AUDIO1 = @"audios/audio1.flac";
        const string AUDIO2 = @"audios/audio2.flac";

        static void Main(string[] args)
        {
            string audioFile = AUDIO1;

            var outputs = new Dictionary<string, int> {
                { "Main", 0 },
                { "Sub", 1 },
            };

            using (AudioPlayer ap = new(outputs))
            {
            LoadAudio:
                ap.LoadFile(audioFile, AUDIO_TIMING);

                ap.Play();

                for (; ; )
                {
                    char kchar = Console.ReadKey().KeyChar;

                    switch (kchar)
                    {
                        case 'q':
                            break;

                        case '1':
                            ap.SetVolume(outputs.Keys.ToArray()[0], 0);
                            break;

                        case '2':
                            ap.SetVolume(outputs.Keys.ToArray()[0], 1);
                            break;

                        case '3':
                            ap.SetVolume(outputs.Keys.ToArray()[1], 0);
                            break;

                        case '4':
                            ap.SetVolume(outputs.Keys.ToArray()[1], 1);
                            break;

                        case 'a':
                            audioFile = AUDIO1;
                            goto LoadAudio;

                        case 'b':
                            audioFile = AUDIO2;
                            goto LoadAudio;
                    }
                }
            }
        }
    }
}
