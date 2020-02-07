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
    public class Ship
    {
        Game1 game;
        Texture2D texture;
        SoundEffect soundEffect;
        public BoundingRectangle Bounds;

        public Ship(Game1 game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            Bounds.Width = 100;
            Bounds.Height = 50;
            Bounds.X = game.GraphicsDevice.Viewport.Width / 2 - Bounds.Width / 2;
            Bounds.Y = game.GraphicsDevice.Viewport.Height / 2 - Bounds.Height /2;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("ship");
            soundEffect = content.Load<SoundEffect>("move");
        }

        public void Update(GameTime gameTime)
        {
            var keyBoardState = Keyboard.GetState();

            if (keyBoardState.IsKeyDown(Keys.Up))
            {
                Bounds.Y -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (keyBoardState.IsKeyDown(Keys.Down))
            {
                Bounds.Y += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (keyBoardState.IsKeyDown(Keys.Left))
            {
                Bounds.X -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (keyBoardState.IsKeyDown(Keys.Right))
            {
                Bounds.X += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            if (Bounds.Y < 0)
            {
                Bounds.Y = 0;
            }
            if (Bounds.Y > game.GraphicsDevice.Viewport.Height - Bounds.Height)
            {
                Bounds.Y = game.GraphicsDevice.Viewport.Height - Bounds.Height;
            }
            if (Bounds.X < 0)
            {
                Bounds.X = 0;
            }
            if (Bounds.X > game.GraphicsDevice.Viewport.Width - Bounds.Width)
            {
                Bounds.X = game.GraphicsDevice.Viewport.Width - Bounds.Width;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Bounds, Color.White);
        }
    }
}
