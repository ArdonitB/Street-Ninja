using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace StreetNinja
{



    class Animation
    {
        // The time since we last updated the frame
        int elapsedTime;

        // The number of frames that the animation contains
        int frameCount;

        // The index of the current frame we are displaying
        int currentFrame;

        // The color of the frame we will be displaying
        Color color;

        // The area of the image strip we want to display
        Rectangle sourceRect = new Rectangle();

        // The area where we want to display the image strip in the game
        Rectangle destinationRect = new Rectangle();

        // This is the spritesheet for the animations
        Texture2D[] PlayerAnimation;

        // The current position of the animation
        Vector2 Position;

        // Is the animation active, if so it will be updated
        public bool Active;

        // Flip will be used to allow one set of animations work facing left or right
        bool flip;

        // Parent game for animation
        Game1 parent;

        public Texture2D GetCurrentFrame
        {
            get { return PlayerAnimation[currentFrame]; }
        }

        public void Initialize(Vector2 position, bool facing, string frames, int fc, Game1 p)
        {
            parent = p;
            // Set starting direction
            flip = facing;

            // Set the spritesheet texture
            //"run0,run1,run2,run3,run4,run5"
            string[] names = frames.Split(',');

            frameCount = fc;
            PlayerAnimation = new Texture2D[frameCount];

            for (int i = 0; i < frameCount; i++)
            {
                PlayerAnimation[i] = parent.Content.Load<Texture2D>(names[i]);
            }

            // Set position of the animation
            Position = position;

            // Set the player to be active
            Active = true;

            // Set the color
            this.color = Color.White;
            
            // Set the time to zero
            elapsedTime = 0;

            // Set current frame to frame 0
            currentFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (Active)
            {
                elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

                // If the elapsed time is larger than the frame time
                // we need to switch frames
                if (elapsedTime > 100)
                {
                    // Move to the next frame
                    currentFrame++;

                    // If the currentFrame is equal to frameCount reset currentFrame to zero
                    if (currentFrame == frameCount)
                    {
                        currentFrame = 0;

                        // the current animation has finished
                        Active = false;
                    }

                    // Reset the elapsed time to zero
                    elapsedTime = 0;
                }
            }
            // Grab the correct frame in the image strip by multiplying the currentFrame index by the Frame width
            // Each animation is in a different row of the spritesheet, so multiplying the frame height by animation will
            // choose the animation to show
            //sourceRect = new Rectangle(currentFrame * FrameWidth, animation * FrameHeight, FrameWidth, FrameHeight);

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            destinationRect = new Rectangle((int)Position.X,
                (int)Position.Y, (int)(PlayerAnimation[currentFrame].Width), (int)(PlayerAnimation[currentFrame].Height));
        }

    }

}

