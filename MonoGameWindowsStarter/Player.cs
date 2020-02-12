using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    enum State { South = 0, East = 1, West = 2, North = 3, Idle = 4 }

    public class Player
    {
        const int FRAME_RATE = 124,
                  FRAME_WIDTH = 32,
                  FRAME_HEIGHT = 38;

        Game1 game;
        State state;
        TimeSpan timer;
        int frame;
        Texture2D texture;
        public BoundingRectangle Bounds;

        public Player(Game1 game)
        {
            this.game = game;
            this.state = State.Idle;
            this.timer = new TimeSpan(0);
        }

        public void Initialize()
        {
            Bounds.Width = FRAME_WIDTH;
            Bounds.Height = FRAME_HEIGHT;
            Bounds.X = game.GraphicsDevice.Viewport.Width / 2 - Bounds.Width / 2;
            Bounds.Y = game.GraphicsDevice.Viewport.Height / 2 - Bounds.Height / 2;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("spriteSheet");
        }

        public void Update(GameTime gameTime)
        {
            var keyBoardState = Keyboard.GetState();
            var speed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // move player according to arrow presses
            if (keyBoardState.IsKeyDown(Keys.Up))
            {
                state = State.North;
                Bounds.Y -= speed;
            }
            else if (keyBoardState.IsKeyDown(Keys.Down))
            {
                state = State.South;
                Bounds.Y += speed;
            }
            else if (keyBoardState.IsKeyDown(Keys.Left))
            {
                state = State.West;
                Bounds.X -= speed;
            }
            else if (keyBoardState.IsKeyDown(Keys.Right))
            {
                state = State.East;
                Bounds.X += speed;
            }
            else
            {
                state = State.Idle;
            }

            if (state != State.Idle)
                timer += gameTime.ElapsedGameTime;

            while (timer.TotalMilliseconds > FRAME_RATE)
            {
                frame++;
                timer -= new TimeSpan(0, 0, 0, 0, FRAME_RATE);
            }
            frame %= 4;

            // check for wall collisions
            if (Bounds.Y < 0)
                Bounds.Y = 0;
            if (Bounds.Y > game.GraphicsDevice.Viewport.Height - Bounds.Height)
                Bounds.Y = game.GraphicsDevice.Viewport.Height - Bounds.Height;
            if (Bounds.X < 0)
                Bounds.X = 0;
            if (Bounds.X > game.GraphicsDevice.Viewport.Width - Bounds.Width)
                Bounds.X = game.GraphicsDevice.Viewport.Width - Bounds.Width;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var source = new Rectangle(
                frame * FRAME_WIDTH,
                (int)state % 4 * FRAME_HEIGHT,
                FRAME_WIDTH,
                FRAME_HEIGHT
                );
            
            spriteBatch.Draw(texture, Bounds, source, Color.White);
        }
    }
}
