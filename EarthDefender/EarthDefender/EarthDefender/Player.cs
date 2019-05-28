using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EarthDefender
{
    class Player : Sprite
    {        
        public Input mInput;

        public List<Bullet> mBullets;

        private Texture2D mBulletSprite;

        private float mTimer;

        public Player(Texture2D texture, Vector2 position, Texture2D bulletSprite)
            : base(texture, position)
        {
            mBullets = new List<Bullet>();
            mBulletSprite = bulletSprite;
            mScale = 0.5f;
        }

        public override void update(GameTime gameTime)
        {
            mPreviousKey = mCurrentKey;
            mCurrentKey = Keyboard.GetState();

            if (mInput == null)
                return;

            mTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            move();
            checkAndFire();

            for (int i = 0; i < mBullets.Count; i++)
            {
                Bullet bullet = mBullets[i];
                if (bullet.mIsRemoved)
                {
                    mBullets.RemoveAt(i);
                    i--;
                }
                mBullets.ElementAt(i).update(gameTime);
            }

            base.update(gameTime);
        }

        public void move()
        {
            KeyboardState kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(mInput.Left))
            {
                mPosition.X -= mSpeed;
            }

            if (kbState.IsKeyDown(mInput.Right))
            {
                mPosition.X += mSpeed;
            }
        }

        public void checkAndFire()
        {
            KeyboardState kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(mInput.Fire) && mPreviousKey.IsKeyUp(mInput.Fire))
            {
                if (mTimer > 0.5f)
                {
                    mTimer = 0f;
                    fireBullet();
                }
            }
        }

        public void fireBullet()
        {
            Bullet bullet = new Bullet(mBulletSprite, new Vector2(mPosition.X + 0.5f * mTexture.Width / 2, mPosition.Y));
            mBullets.Add(bullet);
            
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            //base.draw(spriteBatch);
            spriteBatch.Draw(mTexture, mPosition, null, Color.White, 0, Vector2.Zero, mScale, SpriteEffects.None, 0f);
            for (int i = 0; i < mBullets.Count; i++)
            {
                mBullets.ElementAt(i).draw(spriteBatch);
            }
        }

        /**
         *  asteroidRectangle - checks if any bullet hits the specific asteroid 
         *  RETURN - Vector2 of the bullet which hit the asteroid
         *  IF no bullet hit - returns a zero vector
        */
        public Vector2 hasBulletHitAsteroid(Rectangle asteroidRectangle)
        {
            //asteroidRectangle = new Rectangle(0, 0, 1500, 1500);
            for (int i=0; i < mBullets.Count; i++)
            {
                Bullet currentBullet = mBullets[i];
                Rectangle bulletRect = currentBullet.mSpriteRectangle;
                if (bulletRect.Intersects(asteroidRectangle))
                { 
                    mBullets.RemoveAt(i);
                    return new Vector2(currentBullet.mPosition.X, currentBullet.mPosition.Y);
                }
            }

            return Vector2.Zero;
        }
    }
}
