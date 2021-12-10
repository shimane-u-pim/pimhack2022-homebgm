﻿using System;
using NAudio;
using NAudio.Wave;
using System.Text.Json;
using System.Text;

namespace AudioPlayerHost
{
    public class Program
    {
        const double AUDIO_TIMING = 45;

        const string AUDIO1 = @"audios/audio1.flac";
        const string AUDIO2 = @"audios/audio2.flac";

        private static AudioPlayer player;

        static void Main(string[] args)
        {
            HttpServer server = new();

            server.IncomingHttpRequest += Server_IncomingHttpRequest;
            server.Start();

            string audioFile = AUDIO1;

            var outputs = new Dictionary<string, int> {
                { "Main", 0 },
                { "Sub", 1 },
            };

            player = new(outputs);

            LoadFile(audioFile);

            for (; ; )
            {
                char kchar = Console.ReadKey().KeyChar;
                switch (kchar)
                {
                    case 'q':
                        return;
                }
            }
        }

        private static void LoadFile(string file)
        { 
            player.LoadFile(file, AUDIO_TIMING);
            player.Play();
        }

        private static async void Server_IncomingHttpRequest(object? sender, IncomingHttpRequestEventArgs e)
        {
            if (e.Request.HttpMethod != "POST") goto Close;

            if (!e.Request.HasEntityBody) goto Close;

            if (e.Request.ContentEncoding != Encoding.UTF8) goto Close;
            ControlApi? control = await JsonSerializer.DeserializeAsync<ControlApi>(e.Request.InputStream);

            if (control == null) goto Close;

            if (control.type == null) goto Close;

            switch (control.type)
            {
                case "file":
                    if (control.file == null || !File.Exists(control.file)) goto Close;
                    LoadFile(control.file);
                    break;

                case "vol":
                    if (control.target == null || control.volume < 0 || control.volume > 1)
                        goto Close;
                    player.SetVolume(control.target, control.volume);
                    break;

                default:
                    goto Close;
            }

            Console.WriteLine($"!{control.type}");
        Close:
            e.Response.Close();
        }
    }
}
