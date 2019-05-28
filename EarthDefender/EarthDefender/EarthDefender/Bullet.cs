using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EarthDefender
{
    class Bullet : Sprite
    {
        private float mTimer;

        public Bullet(Texture2D texture, Vector2 pos) 
            : base(texture, pos)
        {
            mSpeed = 2f;
            mScale = 0.1f;
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);

            mPosition.Y -= mSpeed; // because bullet goes up the screen.
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, mPosition, null, Color.White, 0, Vector2.Zero, mScale, SpriteEffects.None, 0f);
        }
    }
}
