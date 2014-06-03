using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Media;


namespace sokoban
{
    class SoundEngine
    {
        private SoundPlayer background;
        private SoundPlayer levelComplete;

        private bool playingBg;
        private bool mute;
        public SoundEngine()
        {
            background = new SoundPlayer(sokoban.resources.sound.bacground01);
            levelComplete = new SoundPlayer(sokoban.resources.sound.levelcomplete01);
            mute = false;
        }

        public void playLevelComplete()
        {
            if (mute)
                return;
            stopPlaying();
            levelComplete.Play();
        }

        public void playBackground()
        {
            if (mute || playingBg)
                return;
            stopPlaying();
            background.PlayLooping();
            playingBg = true;
        }

        public void stopPlaying()
        {
            background.Stop();
            levelComplete.Stop();
            playingBg = false;
        }

        public void toggleMute()
        {
            mute = !mute;
            if (mute) {
                stopPlaying();
            } else {
                playBackground();
            }
        }
    }
}
