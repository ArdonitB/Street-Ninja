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
    public class MenuState : State
    {
        private List<Component> _components;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Regular");

            var newLabel = new Label(buttonFont)
            {
                Position = new Vector2(230, 50),
                Text = "STREET NINJA",
                PenColor = Color.Red,
                Shadow = Color.Yellow
            };
            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(80, 100),
                Text = "New Game",
            };

            newGameButton.Click += NewGameButton_Click;

            var loadGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(80, 225),
                Text = "Option",
            };

            loadGameButton.Click += LoadGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(80, 350),
                Text = "Quit Game",
            };

            quitGameButton.Click += QuitGameButton_Click; ;

            _components = new List<Component>()
            {
                newLabel,
                newGameButton,
                loadGameButton,
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
        private void LoadGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new OptionState(_game, _graphicsDevice, _content));
            _game.gamestate = Game1.ScreenState.Options;
        }
        private void NewGameButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("New Game");
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            _game.gamestate = Game1.ScreenState.Playing;
        }
    }
}
