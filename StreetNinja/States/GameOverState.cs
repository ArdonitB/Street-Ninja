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
    class GameOverState: State
    {
        private List<Component> _components;

        public GameOverState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Regular");


            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(80, 50),
                Text = "Main Menu",
            };

            newGameButton.Click += NewGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(80, 300),
                Text = "Quit Game",
            };

            quitGameButton.Click += QuitGameButton_Click; ;

            _components = new List<Component>()
            {
                newGameButton,
                quitGameButton,
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
