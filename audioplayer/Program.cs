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

            using (AudioPlayUnit apu1 = new(0))
            using (AudioPlayUnit apu2 = new(1))
            {
            LoadAudio:
                apu1.LoadFile(audioFile);
                apu2.LoadFile(audioFile);
                apu2.CurrentTime.Add(TimeSpan.FromMilliseconds(AUDIO_TIMING));

                apu1.Play();
                apu2.Play();

                for (; ; )
                {
                    char kchar = Console.ReadKey().KeyChar;

                    switch (kchar)
                    {
                        case 'q':
                            break;

                        case '1':
                            apu1.Volume = 0;
                            break;

                        case '2':
                            apu1.Volume = 1;
                            break;

                        case '3':
                            apu2.Volume = 0;
                            break;

                        case '4':
                            apu2.Volume = 1;
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
