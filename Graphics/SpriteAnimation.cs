using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrexRunner.Graphics
{
    public class SpriteAnimation
    {
        private List<SpriteAnimationFrame> _frames = new List<SpriteAnimationFrame>();

        // This definition allows for shorthanding "anim[1]" instead of "anim.GetFrame(1);"
        public SpriteAnimationFrame this[int index]
        {
            get
            {
                return GetFrame(index);
            }
        }

        public SpriteAnimationFrame CurrentFrame
        {
            get
            {
                return _frames
                    .Where(f => f.TimeStamp <= PlaybackProgress) // Query the frames that have a timestamp value less than the Playback progress
                    .OrderBy(f => f.TimeStamp)  // Order the frames, based on their timestamp, in ascending order
                    .LastOrDefault(); // If there are no frames in this animation, return (null)
            }
        }

        public float Duration
        {
            get
            {
                // Return 0 if there are no frames in the animation
                if (!_frames.Any())
                    return 0;

                // Determine and return the maximum number of frames in this animation
                return _frames.Max(f => f.TimeStamp);
            }
        }
        public bool IsPlaying { get; private set; }
        public float PlaybackProgress { get; private set; }

        public bool ShouldLoop { get; set; } = true;

        public void AddFrame(Sprite sprite, float timeStamp)
        {
            SpriteAnimationFrame frame = new SpriteAnimationFrame(sprite, timeStamp);

            _frames.Add(frame);
        }

        public void Update(GameTime gameTime)
        {
            if (IsPlaying)
            {
                PlaybackProgress += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (PlaybackProgress > Duration)
                {
                    if (ShouldLoop)
                        PlaybackProgress -= Duration;
                    else
                        Stop(); // We're not looping the anim, and we've also reached/exceeded the anim frame max. Stop the animation here
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            // Look up the current animation frame to be drawn
            SpriteAnimationFrame frame = CurrentFrame;

            // Draw that current animation frame to the screen, if it exists
            if (frame != null)
                frame.Sprite.Draw(spriteBatch, position);
        }
        public void Play()
        {
            IsPlaying = true;
        }
        public void Stop()
        {
            IsPlaying = false;
            PlaybackProgress = 0; // Reset the animation
        }

        public SpriteAnimationFrame GetFrame(int index)
        {
            if (index < 0 || index >= _frames.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "A frame with index " + index + " does not exist in this animation.");

            return _frames[index];
        }

        public void Clear()
        {
            Stop();
            _frames.Clear();
        }
    }
}
