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
        Animation[] animations;
        public Vector2 Position;
        Game1 Parent;
        public bool Facing;
        int AnimationsNo;
        int current = 0;
        int count = -1;
        Squared.Tiled.Object MapObject;

        public Enemy(int newhealth):base(newhealth)
        {

        }

        public void Initialize(Vector2 position, bool facing, Game1 p, int a, Squared.Tiled.Object mapObj)
        {
            MapObject = mapObj;
            Position = position;
            Parent = p;
            Facing = facing;
            AnimationsNo = a;
            animations = new Animation[a];
            base.Initialize(position, facing, p, a);


        }


        public void Update(GameTime gameTime, Vector2 PlayerPos)
        {


            Vector2 temp = (PlayerPos - Position) / (Vector2.Distance(PlayerPos, Position)/2);

            Position += temp;

            if(temp.X<0)
            {
                Facing = false;
            }
            else
            {
                Facing = true;
            }

            if (Math.Abs((int)temp.X) == 0 && Math.Abs((int)temp.Y) == 0)
            {
                if (CurrentAnimation != 0)
                {
                    CurrentAnimation = 0;
                    Playing.Active = true;
                }
            }
            else
            {
                if (!Playing.Active)
                {
                    CurrentAnimation = 1;
                    Playing.Active = true;
                }
            }

            if (count != -1)
            {
                if (animations[current].Active)
                    animations[current].Update(gameTime);
            }

            MapObject.X = (int)Position.X;
            MapObject.Y = (int)Position.Y;
            base.Update(gameTime);
        }

        public new static Texture2D Flip(Texture2D source, bool vertical, bool horizontal)
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
        public new Animation Playing => animations[current];

        public new int CurrentAnimation
        {
            get { return current; }
            set { current = value; }
        }
        public new Texture2D GetFrameTexture
        {
            get
            {
                if (Facing)
                    return animations[current].GetCurrentFrame;
                else
                    return Flip(animations[current].GetCurrentFrame, false, true);
            }
        }
        public new void Initialize(Vector2 position, bool facing, Game1 p, int a)
        {
            Position = position;
            Parent = p;
            Facing = facing;
            AnimationsNo = a;
            animations = new Animation[a];
        }
        public new void AddAnimation(string files, int no, Game1 g)
        {
            count++;
            Animation temp = new Animation();
            temp.Initialize(Position, false, files, no, g);
            animations[count] = temp;
        }


        public void Draw(SpriteBatch spriteBatch, Vector2 vp)
        {
            // Make a 1x1 texture named pixel.  
            Texture2D pixel = new Texture2D(Parent.GraphicsDevice, 1, 1);

            // Create a 1D array of color data to fill the pixel texture with.  
            Color[] colorData = {
                        Color.White,
                    };

            // Set the texture data with our color information.  
            pixel.SetData<Color>(colorData);

            rectangle = new Rectangle((int)Position.X - (int)vp.X, (int)Position.Y - (int)vp.Y- 13, 80 * (int)(health / starthealth), 10);

            if (health > 0)
                spriteBatch.Draw(pixel, rectangle, Color.Red);
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
        public Rectangle rectangle;
        Texture2D texture;

        public float health;

        public float starthealth;

        public Character(float newHealth)
        {
            health = newHealth;
            starthealth = health;
        }

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
            {
               
            }
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 pos)
        {
            // Make a 1x1 texture named pixel.  
            Texture2D pixel = new Texture2D(Parent.GraphicsDevice, 1, 1);

            // Create a 1D array of color data to fill the pixel texture with.  
            Color[] colorData = {
                        Color.White,
                    };

            // Set the texture data with our color information.  
            pixel.SetData<Color>(colorData);

            rectangle = new Rectangle((int)pos.X, (int)pos.Y -13, 80 * (int)(health / starthealth), 10);

            if (health > 0)
                spriteBatch.Draw(pixel, rectangle, Color.Red);
        }
    }
}
