using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        const int SCREEN_WIDTH = 1042,
                  SCREEN_HEIGHT = 700;

        const String GAME_OVER = "GAME OVER";
        int STRING_LEN = GAME_OVER.Length;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Random Random = new Random();

        Texture2D space;
        SpriteFont spriteFont;
        bool isGameOver;

        Player player;
        Rock rock;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            player = new Player(this);
            rock = new Rock(this);
            isGameOver = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.ApplyChanges();

            player.Initialize();
            rock.Initialize();

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

            // TODO: use this.Content to load your game content here
            space = Content.Load<Texture2D>("space");
            spriteFont = Content.Load<SpriteFont>("defaultFont");

            player.LoadContent(Content);
            rock.LoadContent(Content);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            player.Update(gameTime);
            rock.Update(gameTime);

            // handle rock and player collision
            if (player.Bounds.CollidesWith(rock.Bounds))
            {
                rock.SoundEffect.Play();

                // set player position to center
                player.Bounds.X = GraphicsDevice.Viewport.Width / 2 - player.Bounds.Width / 2;
                player.Bounds.Y = GraphicsDevice.Viewport.Height / 2 - player.Bounds.Height / 2;

                // stop rock and set to top left corner
                rock.Velocity = Vector2.Zero;
                rock.Bounds.X = rock.Bounds.Radius;
                rock.Bounds.Y = rock.Bounds.Radius;

                isGameOver = true;
            }

            // restart game when space is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Space)
                && isGameOver)
            {
                rock.Velocity = new Vector2(
                    (float)Random.NextDouble(),
                    (float)Random.NextDouble()
                );
                rock.Velocity.Normalize();
                isGameOver = false;
            }

            // speed up rock when left/right shift is pressed
            if ((Keyboard.GetState().IsKeyDown(Keys.RightShift)
                || Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                && !isGameOver)
                rock.Velocity += new Vector2(1, 1);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            // draw background
            spriteBatch.Draw(space, 
                new Rectangle(0,0, SCREEN_WIDTH, SCREEN_HEIGHT), 
                Color.White);

            player.Draw(spriteBatch);
            rock.Draw(spriteBatch);

            // draw player X and Y in right corner
            spriteBatch.DrawString(spriteFont,
                $"X:{player.Bounds.X} Y:{player.Bounds.Y}",
                new Vector2(SCREEN_WIDTH - 505, 0),
                Color.White);

            // drawString game over if game is over
            if (isGameOver)
            {
                spriteBatch.DrawString(spriteFont, 
                    GAME_OVER, 
                    new Vector2((GraphicsDevice.Viewport.Width / 2) - 20*STRING_LEN, GraphicsDevice.Viewport.Height / 2 ),
                    Color.ForestGreen);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
