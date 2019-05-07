using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Squared.Tiled;
using StreetNinja.States;
using System;
using System.Collections.Generic;
using System.Timers;


namespace StreetNinja
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map map;
        Layer bounds;
        Layer pavement;
        Vector2 viewportPosition;
        int tilePixel;
        int deadenemies;
        Squared.Tiled.Object player;
        Character Player1;
        List<Enemy> enemies = new List<Enemy>();
        //Enemy Enemy1;

        //Health Bar
        Texture2D healthTexture;
        Rectangle healthRectangle;

        MouseState PastMouse;

        Timer gameovertimer = new Timer();

        public enum ScreenState
        {
            MainMenu,
            Options,
            Playing,
            GameOver,
            GameComplete
        }

        public ScreenState gamestate = ScreenState.MainMenu;

        private State _currentState;

        private State _nextState;

        public void ChangeState(State state)
        {
            _nextState = state;
        }

        public void ToggleFullScreen()
        {
            graphics.IsFullScreen = !graphics.IsFullScreen;
            graphics.ApplyChanges();
        }
  
        int jumppixels = 0;
        int jumpcount = 0;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 600;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 600;   // set this value to the desired height of your window
            graphics.ApplyChanges();

        }
        

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            spriteBatch = new SpriteBatch(GraphicsDevice);

            _currentState = new MenuState(this, graphics.GraphicsDevice, Content);


            Player1 = new Character(4);
                       
            
            // TODO: use this.Content to load your game content here
            map = Map.Load(Path.Combine(Content.RootDirectory, "(MAP 1).tmx"), Content);
            map.ObjectGroups["5Objects"].Objects["Player1"].Texture = Content.Load<Texture2D>("Player1");
            map.ObjectGroups["5Objects"].Objects["Enemy1"].Texture = Content.Load<Texture2D>("Enemy1");
            map.ObjectGroups["5Objects"].Objects["Flag"].Texture = Content.Load<Texture2D>("Flag");
            
            bounds = map.Layers["0Collision"];

            viewportPosition = new Vector2(map.ObjectGroups["5Objects"].Objects["Player1"].X, map.ObjectGroups["5Objects"].Objects["Player1"].Y);

            Player1.Initialize(viewportPosition, true,  this, 6, 4 );
            Player1.AddAnimation("run0,run1,run2,run3,run4,run5", 6, this);
            Player1.AddAnimation("jump0,jump1,jump2,jump3", 4, this);
            Player1.AddAnimation("standing0", 1, this);
            Player1.AddAnimation("standingattack0,standingattack1,standingattack2,standingattack3,standingattack4,standingattack5,standingattack6," +
                "standingattack7,standingattack8,", 9, this);
            Player1.AddAnimation("death0,death1,death2,death3,death4,death5,death6,death7,death8", 9, this );
            Player1.AddAnimation("standing0,runattack6,runattack7,runattack8,runattack8,runattack9,runattack10,runattack11,runattack12", 9, this);
            Player1.CurrentAnimation = 2;

            for (int i = 1; i <= 9; i++)
            {
                Enemy Enemy1 = new Enemy(2);
                Enemy1.Initialize(new Vector2(map.ObjectGroups["5Objects"].Objects["Enemy"+i].X, map.ObjectGroups["5Objects"].Objects["Enemy"+i].Y), false, this, 5, 3, map.ObjectGroups["5Objects"].Objects["Enemy"+i]);
                Enemy1.AddAnimation("Enemy1", 1, this);
                Enemy1.AddAnimation("erun1,erun2,erun3", 3, this);
                Enemy1.AddAnimation("hit", 1, this);
                Enemy1.AddAnimation("dead1,dead2", 2, this);
                Enemy1.AddAnimation("eattack1,eattack2,eattack3", 3, this);
                Enemy1.CurrentAnimation = 0;
                enemies.Add(Enemy1);
            }
            Enemy boss = new Enemy(8);
            boss.Initialize(new Vector2(map.ObjectGroups["5Objects"].Objects["Enemy10"].X, map.ObjectGroups["5Objects"].Objects["Enemy10"].Y), false, this, 5, 3, map.ObjectGroups["5Objects"].Objects["Enemy10"]);
            boss.AddAnimation("Enemy1", 1, this);
            boss.AddAnimation("erun1,erun2,erun3", 3, this);
            boss.AddAnimation("hit", 1, this);
            boss.AddAnimation("dead1,dead2", 2, this);
            boss.AddAnimation("eattack1,eattack2,eattack3", 3, this);
            boss.CurrentAnimation = 0;
            enemies.Add(boss);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }








        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {


            if (_nextState != null)
            {
                _currentState = _nextState;

                _nextState = null;
            }
            _currentState.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (gamestate == ScreenState.Playing)
            {
                Vector2 playerpos = new Vector2(map.ObjectGroups["5Objects"].Objects["Player1"].X, map.ObjectGroups["5Objects"].Objects["Player1"].Y);

                if (jumppixels <= 0 && Player1.CurrentAnimation == 0)
                    Player1.CurrentAnimation = 2;

                KeyboardState keys = Keyboard.GetState();

                if (keys.IsKeyDown(Keys.U))
                {
                    Player1.Health += (float)0.25;

                }
                if (Player1.Health < 20)
                {
                    Console.WriteLine(Player1.Health);
                    if (keys.IsKeyDown(Keys.W))
                    {
                        map.ObjectGroups["5Objects"].Objects["Player1"].Y -= 2;
                        if (!Player1.Playing.Active)
                        {
                            Player1.CurrentAnimation = 0;
                            Player1.Playing.Active = true;
                        }

                    }

                    if (keys.IsKeyDown(Keys.S))
                    {
                        map.ObjectGroups["5Objects"].Objects["Player1"].Y += 2;
                        if (!Player1.Playing.Active)
                        {
                            Player1.CurrentAnimation = 0;
                            Player1.Playing.Active = true;
                        }

                    }

                    if (keys.IsKeyDown(Keys.D))
                    {
                        map.ObjectGroups["5Objects"].Objects["Player1"].X += 5;

                        Player1.Facing = true;
                        if (!Player1.Playing.Active)
                        {
                            Player1.CurrentAnimation = 0;
                            Player1.Playing.Active = true;
                        }

                    }

                    if (keys.IsKeyDown(Keys.A))
                    {
                        map.ObjectGroups["5Objects"].Objects["Player1"].X -= 5;
                        Player1.Facing = false;
                        if (!Player1.Playing.Active)
                        {
                            Player1.CurrentAnimation = 0;
                            Player1.Playing.Active = true;
                        }
                    }
                    if (keys.IsKeyDown(Keys.Space))
                    {
                        if (jumpcount < 2)
                        {
                            jumpcount++;
                            jumppixels += 30;
                            Player1.CurrentAnimation = 1;
                        }
                    }
                    if (keys.IsKeyDown(Keys.J))
                    {
                        if (Player1.CurrentAnimation == 2 || Player1.CurrentAnimation == 3)
                        {
                            Player1.CurrentAnimation = 3;
                            Player1.Playing.Active = true;
                        }
                        else if (Player1.CurrentAnimation == 0)
                        {
                            Player1.CurrentAnimation = 5;
                            Player1.Playing.Active = true;
                        }

                    }
                }
                else
                {
                    if (!Player1.Playing.Active && Player1.CurrentAnimation != 4)
                    {
                        Player1.CurrentAnimation = 4;
                        Player1.Playing.Active = true;
                        if (!gameovertimer.Enabled)
                        {
                            gameovertimer.Interval = 1000;
                            gameovertimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                            gameovertimer.Enabled = true;
                        }
                    }
                }
                Rectangle p = Player1.punchbox;
                int j = 1;
                deadenemies = 0;
                foreach (Enemy Enemy1 in enemies)
                {
                    if (Enemy1.CurrentAnimation != 3)
                    {
                        Enemy1.Update(gameTime, new Vector2(map.ObjectGroups["5Objects"].Objects["Player1"].X, map.ObjectGroups["5Objects"].Objects["Player1"].Y), Player1);
                        map.ObjectGroups["5Objects"].Objects["Enemy" + j].Texture = Enemy1.GetFrameTexture;
                        Rectangle e = Enemy1.hitbox;

                        Rectangle b = new Rectangle(0, 0, 0, 0);
                        bool intersects = false;
                        //Console.WriteLine(p + " - " + e);
                        if (p != b)
                        {
                            intersects = e.Intersects(p);

                            if (intersects)
                            {
                                Console.WriteLine("hit enemy: " + Enemy1.Hitable);
                            }
                        }
                        if (intersects && Enemy1.Hitable)
                        {

                            Enemy1.CurrentAnimation = 2;
                            Enemy1.Playing.Active = true;
                            Enemy1.Health -= 1;
                            Console.WriteLine("hit enemy & hitable: " + Enemy1.Hitable);
                            Console.WriteLine(Player1.Position.Y + " " + Enemy1.Position.Y);
                            Enemy1.Hitable = false;
                        }
                    }
                    else
                        deadenemies++;
                    j++;
                }

                if (keys.IsKeyDown(Keys.Y))
                {
                    Player1.CurrentAnimation = 4;
                }


                if (CheckBounds(map.ObjectGroups["5Objects"].Objects["Player1"]))
                {
                    map.ObjectGroups["5Objects"].Objects["Player1"].Y = (int)playerpos.Y;
                    map.ObjectGroups["5Objects"].Objects["Player1"].X = (int)playerpos.X;


                }

                viewportPosition = new Vector2(map.ObjectGroups["5Objects"].Objects["Player1"].X + map.ObjectGroups["5Objects"].Objects["Player1"].Width / 2 - 300, map.ObjectGroups["5Objects"].Objects["Player1"].Y - 240);
                //if (playerpos.X < 500)
                //    viewportPosition = new Vector2(0, map.ObjectGroups["5Objects"].Objects["Player1"].Y - 240);
                //else if (playerpos.X < 1000)
                //    viewportPosition = new Vector2(500, map.ObjectGroups["5Objects"].Objects["Player1"].Y - 240);
                //else if (playerpos.X < 1500)
                //    viewportPosition = new Vector2(1000, map.ObjectGroups["5Objects"].Objects["Player1"].Y - 240);
                //else if(playerpos.X < 2000)
                //    viewportPosition = new Vector2(1500, map.ObjectGroups["5Objects"].Objects["Player1"].Y - 240);
                //else if (playerpos.X < 2500)
                //    viewportPosition = new Vector2(2000, map.ObjectGroups["5Objects"].Objects["Player1"].Y - 240);
                //else if (playerpos.X < 3000)
                //    viewportPosition = new Vector2(2500, map.ObjectGroups["5Objects"].Objects["Player1"].Y - 240);
                //else if (playerpos.X < 3500)
                //    viewportPosition = new Vector2(3000, map.ObjectGroups["5Objects"].Objects["Player1"].Y - 240);
                //else if (playerpos.X < 4000)
                ///    viewportPosition = new Vector2(3500, map.ObjectGroups["5Objects"].Objects["Player1"].Y - 240);
                //else if (playerpos.X < 4500)
                //   viewportPosition = new Vector2(4000, map.ObjectGroups["5Objects"].Objects["Player1"].Y - 240);
                //else if (playerpos.X < 5000)
                //  viewportPosition = new Vector2(4500, map.ObjectGroups["5Objects"].Objects["Player1"].Y - 240);
                // TODO: Add your update logic here

                if (jumppixels > 0)
                    jumppixels -= 4;
                else
                    jumpcount = 0;



                Player1.Update(gameTime);

                map.ObjectGroups["5Objects"].Objects["Player1"].Texture = Player1.GetFrameTexture;
                
            }
            _currentState.PostUpdate(gameTime);
            base.Update(gameTime);
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            gameovertimer.Enabled = false;
            _currentState = new GameOverState(this, graphics.GraphicsDevice, Content);
            gamestate = ScreenState.GameOver;
        }

        public bool CheckBounds(Squared.Tiled.Object obj)
        {
            bool check = false;
            
            Rectangle playrec = new Rectangle(
                obj.X,
                obj.Y + (obj.Height-10),
                obj.Width,
                10
                );

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    if (bounds.GetTile(x, y) != 0)
                    {
                        Rectangle tile = new Rectangle(
                            (int)x * map.TileWidth,
                            (int)y * map.TileHeight,
                            map.TileWidth,
                        map.TileHeight
                        );

                        if (playrec.Intersects(tile))
                            check = true;
                    }
                }
            }

            Squared.Tiled.Object flag = map.ObjectGroups["5Objects"].Objects["Flag"];
            Rectangle flagrec = new Rectangle(flag.X,flag.Y,flag.Width,flag.Height);
            if (playrec.Intersects(flagrec) && deadenemies>9)
            {
                Console.WriteLine("test");

                _currentState = new GameWinState(this, graphics.GraphicsDevice, Content);
                gamestate = ScreenState.GameComplete;
            }

            return check;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Color test = new Color(45, 45, 45);
            GraphicsDevice.Clear(test);

           
            // TODO: Add your drawing code here

            if (gamestate == ScreenState.Playing)
            {
                spriteBatch.Begin();
                int playerpos = map.ObjectGroups["5Objects"].Objects["Player1"].Y;
                map.ObjectGroups["5Objects"].Objects["Player1"].Y -= jumppixels;
                int j = 1;
                foreach (Enemy Enemy1 in enemies)
                {
                    if (Enemy1.CurrentAnimation == 3)
                    {
                        map.ObjectGroups["5Objects"].Objects["Enemy"+j].Width = 120;
                    }
                    Enemy1.Draw(spriteBatch, viewportPosition);
                    j++;
                }
                map.Draw(spriteBatch, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), viewportPosition);
                map.ObjectGroups["5Objects"].Objects["Player1"].Y = playerpos;
                Player1.Draw(spriteBatch, new Vector2(map.ObjectGroups["5Objects"].Objects["Player1"].X, map.ObjectGroups["5Objects"].Objects["Player1"].Y)-viewportPosition);
                foreach (Enemy Enemy1 in enemies)
                {
                    Enemy1.Draw(spriteBatch, viewportPosition);
                }
                spriteBatch.End();
            }
            else if (gamestate == ScreenState.MainMenu)
            {
                _currentState.Draw(gameTime, spriteBatch);
            }
            else if (gamestate == ScreenState.Options)
            {
                _currentState.Draw(gameTime, spriteBatch);
            }
            else if (gamestate == ScreenState.GameOver)
            {
                _currentState.Draw(gameTime, spriteBatch);
            }
            else if (gamestate == ScreenState.GameComplete)
            {
                _currentState.Draw(gameTime, spriteBatch);
            }



            base.Draw(gameTime);
        }
    }
}
