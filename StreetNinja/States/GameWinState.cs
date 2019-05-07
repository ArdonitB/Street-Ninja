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
    class GameWinState : State
    {
        private List<Component> _components;

        public GameWinState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Regular");


            var newLabel = new Label(buttonFont)
            {
                Position = new Vector2(230, 50),
                Text = "YOU WIN",
                PenColor = Color.Green,
                Shadow = Color.White
            };

            var newLabel1 = new Label(buttonFont)
            {
                Position = new Vector2(150, 100),
                Text = "You have completed this level.\n\nClick below to continue",
                PenColor = Color.White,
            };

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(80, 300),
                Text = "Main Menu",
            };

            newGameButton.Click += NewGameButton_Click;


            _components = new List<Component>()
            {
                newLabel,
                newLabel1,
                newGameButton
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
        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("New Game");
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            _game.gamestate = Game1.ScreenState.MainMenu;
        }
    }
}