using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EarthDefender
{
    class Asteroid : Sprite
    {
        public Asteroid(Texture2D texture)
            : base(texture, new Vector2(Game1.mRandom.Next(0, Game1.ScreenWidth - texture.Width), -texture.Height))
        {
            mSpeed = Game1.mRandom.Next(3, 8);
        }

        public override void update(GameTime gameTime)
        {
            mPosition.Y += mSpeed;

            // if hit bottom of window 
            if (mSpriteRectangle.Bottom >= Game1.ScreenHeight)
            {
                mIsRemoved = true;
                // need to add here score or something?
            }
            base.update(gameTime);
        }
    }
}
