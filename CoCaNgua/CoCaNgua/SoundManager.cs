using System;
using System.IO;
using System.Media;
using WMPLib;

namespace CoCaNgua
{
    public static class SoundManager
    {
        public static bool Muted = false;

        static SoundPlayer dice, move, kick, spawn;
        static WindowsMediaPlayer bgm;

        static string SoundPath(string file)
            => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sound", file);

        public static void Init()
        {
            // ===== SFX (WAV) =====
            dice = new SoundPlayer(SoundPath("dice.wav"));
            move = new SoundPlayer(SoundPath("move.wav"));
            kick = new SoundPlayer(SoundPath("kick.wav"));
            spawn = new SoundPlayer(SoundPath("spawn.wav"));

            dice.LoadAsync();
            move.LoadAsync();
            kick.LoadAsync();
            spawn.LoadAsync();

            // ===== BGM (MP3) =====
            bgm = new WindowsMediaPlayer();
            bgm.settings.setMode("loop", true);
            bgm.settings.volume = 35;
        }

        // ===== SFX =====
        static void Play(SoundPlayer sp)
        {
            if (Muted || sp == null) return;
            try { sp.Play(); } catch { }
        }

        public static void Dice() => Play(dice);
        public static void Move() => Play(move);
        public static void Kick() => Play(kick);
        public static void Spawn() => Play(spawn);

        // ===== BGM =====
        public static void StartBgm()
        {
            if (bgm == null) return;

            if (Muted)
                bgm.controls.stop();
            else
            {
                bgm.URL = SoundPath("bgm.mp3");
                bgm.controls.play();
            }
        }

        public static void ToggleMute()
        {
            Muted = !Muted;

            if (Muted)
                bgm?.controls.stop();
            else
            {
                bgm.URL = SoundPath("bgm.mp3");
                bgm.controls.play();
            }
        }

        public static void StopBgm()
        {
            try { bgm?.controls.stop(); } catch { }
        }
    }
}
