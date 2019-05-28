using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EarthDefender
{
    class Sprite : ICloneable
    {
        protected Texture2D mTexture;

        protected KeyboardState mCurrentKey, mPreviousKey;

        public Vector2 mPosition;

        public float mSpeed = 10f;

        public int mDirection = 1;

        public float mScale = 1f;

        public bool mIsRemoved = false;
        
        public Sprite(Texture2D texture, Vector2 pos)
        {
            mTexture = texture;
            mPosition = pos;
        }

        public virtual void update(GameTime gameTime)
        {

        }

        public virtual void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, mPosition, Color.White);
        }

        public Rectangle mSpriteRectangle
        {
            get
            { 
                return new Rectangle((int)mPosition.X + 10, (int)mPosition.Y + 10, (int)(mTexture.Width * mScale) - 20, (int)(mTexture.Height * mScale) - 20);
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
