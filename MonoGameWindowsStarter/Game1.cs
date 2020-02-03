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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const int SCREEN_WIDTH = 1042,
                  SCREEN_HEIGHT = 700,
                  SHIP_SIZE = 200,
                  ROCK_SIZE = 100;

        Texture2D space, gameOver;
        bool isGameOver = false;
        
        Texture2D ship;
        Vector2 shipPosition;
        int shipSpeed;
        BoundingRectangle shipBounds;
        
        Texture2D rock;
        Vector2 rockPosition;
        Vector2 rockVelocity;
        BoundingRectangle rockBounds;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            shipPosition = new Vector2(SCREEN_WIDTH/2 - SHIP_SIZE/2, SCREEN_HEIGHT);
            rockPosition = new Vector2(0, 0);
            rockVelocity = new Vector2((float)1, (float)1);

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
            ship = Content.Load<Texture2D>("ship");
            space = Content.Load<Texture2D>("space");
            rock = Content.Load<Texture2D>("rock");
            gameOver = Content.Load<Texture2D>("game over");
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
            shipSpeed = (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            shipBounds = new BoundingRectangle(shipPosition.X,
                                                shipPosition.Y,
                                                65,
                                                65);

            rockBounds = new BoundingRectangle(rockPosition.X,
                                                rockPosition.Y,
                                                50,
                                                50);

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                shipPosition.Y -= shipSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                shipPosition.Y += shipSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                shipPosition.X -= shipSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                shipPosition.X += shipSpeed;

            // ship check for wall collisions
            // top
            if (shipPosition.Y < -75)
                shipPosition.Y = -75;
            // bottom
            if (shipPosition.Y > graphics.PreferredBackBufferHeight - 125)
                shipPosition.Y = graphics.PreferredBackBufferHeight - 125;
            // left
            if (shipPosition.X < -70)
                shipPosition.X = -70;
            // right
            if (shipPosition.X > graphics.PreferredBackBufferWidth - 130)
                shipPosition.X = graphics.PreferredBackBufferWidth - 130;

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                rockVelocity.Y += 1;
                rockVelocity.X += 1;
                if (isGameOver) isGameOver = false;
            }

            rockPosition += rockVelocity;

            // rock check for wall collisions
            // top
            if (rockPosition.Y < 0)
            {
                rockVelocity.Y *= -1;
                float delta = 0 - rockPosition.Y;
                rockPosition.Y += 2 * delta;
            }
            // bottom
            if (rockPosition.Y > graphics.PreferredBackBufferHeight - ROCK_SIZE)
            {
                rockVelocity.Y *= -1;
                float delta = graphics.PreferredBackBufferHeight - ROCK_SIZE - rockPosition.Y;
                rockPosition.Y += 2 * delta;
            }
            // left
            if (rockPosition.X < 0)
            {
                rockVelocity.X *= -1;
                float delta = 0 - rockPosition.X;
                rockPosition.X += 2 * delta;
            }
            // right
            if (rockPosition.X > graphics.PreferredBackBufferWidth - ROCK_SIZE)
            {
                rockVelocity.X *= -1;
                float delta = graphics.PreferredBackBufferWidth - ROCK_SIZE - rockPosition.X;
                rockPosition.X += 2 * delta;
            }

            if (shipBounds.CollidesWith(rockBounds))
            {
                rockVelocity.X = 0;
                rockVelocity.Y = 0;
                isGameOver = true;
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

            // draw ship
            spriteBatch.Draw(ship, 
                new Rectangle((int)shipPosition.X,
                                (int)shipPosition.Y, 
                                SHIP_SIZE, 
                                SHIP_SIZE),
                Color.White);

            // draw rock
            spriteBatch.Draw(rock,
                new Rectangle((int)rockPosition.X,
                                (int)rockPosition.Y,
                                ROCK_SIZE,
                                ROCK_SIZE),
                Color.White);

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
