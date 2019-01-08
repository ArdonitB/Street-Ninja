using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Squared.Tiled;

namespace StreetNinja
{
    class Enemy:Character
    {
        Squared.Tiled.Object MapObject;

        public void Initialize(Vector2 position, bool facing, Game1 p, int a, Squared.Tiled.Object mapObj)
        {
            MapObject = mapObj;
            base.Initialize(position, facing, p, a);

        }

        public void Update(GameTime gameTime, Vector2 PlayerPos)
        {


            Vector2 temp = (PlayerPos - Position) / (Vector2.Distance(PlayerPos, Position)/2);

            Position += temp;

            if (Math.Abs((int)temp.X) == 0 && Math.Abs((int)temp.Y) == 0)
            {
                CurrentAnimation = 0;
                Playing.Active = true;
            }
            else
            {
                CurrentAnimation = 0;
                Playing.Active = true;
            }

            MapObject.X = (int)Position.X;
            MapObject.Y = (int)Position.Y;
            base.Update(gameTime);
        }
    }

    class Character
    {
        Animation[] animations;
        public Vector2 Position;
        Game1 Parent;
        public bool Facing;
        int AnimationsNo;
        int current = 0;
        int count = -1;

        public static Texture2D Flip(Texture2D source, bool vertical, bool horizontal)
        {
            Texture2D flipped = new Texture2D(source.GraphicsDevice, source.Width, source.Height);
            Color[] data = new Color[source.Width * source.Height];
            Color[] flippedData = new Color[data.Length];

            source.GetData(data);

            for (int x = 0; x < source.Width; x++)
                for (int y = 0; y < source.Height; y++)
                {
                    int idx = (horizontal ? source.Width - 1 - x : x) + ((vertical ? source.Height - 1 - y : y) * source.Width);
                    flippedData[x + y * source.Width] = data[idx];
                }

            flipped.SetData(flippedData);

            return flipped;
        }

        public Animation Playing
        {
            get { return animations[current]; }

        }

        public int CurrentAnimation
        {
            get { return current; }
            set { current = value; }
        }

        public Texture2D GetFrameTexture
        {
            get {
                if(Facing)
                    return animations[current].GetCurrentFrame;
                else
                    return Flip(animations[current].GetCurrentFrame,false,true);
            }
        }

        public void Initialize(Vector2 position, bool facing, Game1 p, int a)
        {
            Position = position;
            Parent = p;
            Facing = facing;
            AnimationsNo = a;
            animations = new Animation[a];
        }

        public void AddAnimation(string files, int no, Game1 g)
        {
            count++;
            Animation temp = new Animation();
            temp.Initialize(Position, false, files, no, g);
            animations[count] = temp;
        }

        public void Update(GameTime gameTime)
        {
            if (count != -1)
            {
                if (animations[current].Active)
                    animations[current].Update(gameTime);
            }
                
        }
    }
}
