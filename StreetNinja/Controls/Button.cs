﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetNinja.Controls
{
    public class Label : Component
    { // Label is a sub class of parent class component
        #region Fields

        private SpriteFont _font;

        #endregion

        #region Properties

        public Color PenColor { get; set; }

        public Vector2 Position { get; set; }

        public Color Shadow { get; set; } // declares properties of a label

        public string Text { get; set; }
        #endregion

        #region Methods

        public Label(SpriteFont font)
        {
            _font = font;
            PenColor = Color.Black;
        }
        public override void Draw(GameTime gametime, SpriteBatch spriteBatch) // Declares methods for labels
        {
            if (Shadow != null)
                spriteBatch.DrawString(_font, Text, Position+new Vector2(1,1), Shadow);
            spriteBatch.DrawString(_font, Text, Position, PenColor);
        }

        public override void Update(GameTime gameTime)
        {

        }
    }

    public class Button : Component
    { //Declares sub class of button
        #region Fields 

        private MouseState _currentMouse;

        private SpriteFont _font;

        private bool _isHovering; // Declares fields and different states of the button

        private MouseState _previousMouse;

        private Texture2D _texture;

        #endregion

        #region Properties

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color PenColor { get; set; }

        public Vector2 Position { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public string Text { get; set; }
        #endregion

        #region Methods

        public Button(Texture2D texture, SpriteFont font)
        {
            _texture = texture;

            _font = font;
            PenColor = Color.Black;
        }
        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            var colour = Color.White;

            if (_isHovering)
                colour = Color.Gray;

            spriteBatch.Draw(_texture, Rectangle, colour);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColor);
            }
        }
        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }

        }
        #endregion
    }
    #endregion
}