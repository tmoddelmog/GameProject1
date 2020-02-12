using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    public class Rock
    {
        Game1 game;
        Texture2D texture;
        public SoundEffect SoundEffect;
        public BoundingCircle Bounds;
        public Vector2 Velocity;

        public Rock(Game1 game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            Bounds.Radius = 100;
            Bounds.X = 0;
            Bounds.Y = 0;
            Velocity = new Vector2(
                (float)game.Random.NextDouble(),
                (float)game.Random.NextDouble()
            );
            Velocity.Normalize();
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("rock");
            SoundEffect = content.Load<SoundEffect>("hit");
        }

        public void Update(GameTime gameTime)
        {
            var viewport = game.GraphicsDevice.Viewport;

            Bounds.Center += 0.5f * (float)gameTime.ElapsedGameTime.TotalMilliseconds * Velocity;

            // check for wall collisions and bounce
            if (Bounds.Center.Y < Bounds.Radius)
            {
                Velocity.Y *= -1;
                float delta = Bounds.Radius - Bounds.Y;
                Bounds.Y += 2 * delta;
            }
            if (Bounds.Center.Y > viewport.Height - Bounds.Radius)
            {
                Velocity.Y *= -1;
                float delta = viewport.Height - Bounds.Radius - Bounds.Y;
                Bounds.Y += 2 * delta;
            }
            if (Bounds.Center.X < Bounds.Radius)
            {
                Velocity.X *= -1;
                float delta = Bounds.Radius - Bounds.X;
                Bounds.X += 2 * delta;
            }
            if (Bounds.X > viewport.Width - Bounds.Radius)
            {
                Velocity.X *= -1;
                float delta = viewport.Width - Bounds.Radius - Bounds.X;
                Bounds.X += 2 * delta;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Bounds, Color.White);
        }
    }
}
