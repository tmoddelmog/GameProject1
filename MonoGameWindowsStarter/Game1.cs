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

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Random Random = new Random();

        Texture2D space, gameOver;
        bool isGameOver;

        Ship ship;
        Rock rock;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ship = new Ship(this);
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

            ship.Initialize();
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
            gameOver = Content.Load<Texture2D>("game over");

            ship.LoadContent(Content);
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
            ship.Update(gameTime);
            rock.Update(gameTime);

            if (ship.Bounds.CollidesWith(rock.Bounds))
            {
                rock.SoundEffect.Play();
                isGameOver = true;
                rock.Velocity = Vector2.Zero;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space)
                && isGameOver)
            {
                isGameOver = false;
                rock.Velocity = new Vector2(
                    (float)Random.NextDouble(),
                    (float)Random.NextDouble()
                );
                rock.Velocity.Normalize();
            }

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

            ship.Draw(spriteBatch);
            rock.Draw(spriteBatch);

            // draw game over if game is over
            if (isGameOver)
            {
                spriteBatch.Draw(gameOver,
                    new Rectangle((int)SCREEN_WIDTH/2 - 100,
                                    (int)SCREEN_HEIGHT/2 - 100,
                                    200,
                                    200),
                    Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
