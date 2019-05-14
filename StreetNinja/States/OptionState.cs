using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StreetNinja.Controls;


namespace StreetNinja.States
{
    public class OptionState : State
    {// option state is a sub class of parent class State
        private List<Component> _components;

        public OptionState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Regular");

            var newLabel = new Label(buttonFont)
            {
                Position = new Vector2(230, 50),
                Text = "OPTIONS", //Declares title of options with colours
                PenColor = Color.Red,
                Shadow = Color.Yellow
            };
            var newToggleButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(80, 100),
                Text = "Toggle Full Screen",  //allows full screen and adjusts player position to account for change
            };

            newToggleButton.Click += NewToggleButton_Click;

            var loadMenuButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(80, 225),
                Text = "Main Menu",
            };

            loadMenuButton.Click += LoadMenuButton_Click;



            _components = new List<Component>()
            {
                newLabel,
                newToggleButton,
                loadMenuButton,
            };
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);

        }

        private void LoadMenuButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            _game.gamestate = Game1.ScreenState.MainMenu;
        }

        private void NewToggleButton_Click(object sender, EventArgs e)
        {
            _game.ToggleFullScreen();
        }
    }
}