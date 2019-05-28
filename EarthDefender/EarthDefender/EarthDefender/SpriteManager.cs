using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EarthDefender
{
    class SpriteManager : DrawableGameComponent
    {
        private float timeElapsed;
        public bool IsLooping = false;
        private float timeToUpdate = 0.05f;
        protected Dictionary<string, Rectangle[]> Animations =
            new Dictionary<string, Rectangle[]>();
        protected int FrameIndex = 0;
        public string Animation;

        protected int Frames;
        private int height;
        private int width;
        SpriteBatch spriteBatch;
        protected Texture2D mTexture;

        public Vector2 mPosition = Vector2.Zero;
        public Color Color = Color.White;
        public Vector2 Origin;
        public float Rotation = 0f;
        public float Scale = 1f;
        public SpriteEffects SpriteEffect;
        protected Rectangle[] recs;

        public float mSpeed = 5f;
        private bool mIsRemoved = false;
        private Rectangle mSpriteRectangle;

        public int FramesPerSecond
        {
            set { timeToUpdate = (1f / value); }
        }

        public SpriteManager(Game game, Texture2D texture, int frames, int animations) 
            : base(game)
        {
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            this.Frames = frames;
            this.mTexture = texture;
            width = mTexture.Width / frames;
            height = mTexture.Height / animations;
            IsLooping = true;

            mSpeed = Game1.mRandom.Next(1, 7);
        }

        public void AddAnimation(string name, int row)
        {
            recs = new Rectangle[Frames];
            for (int i = 0; i < Frames; i++)
            {
                recs[i] = new Rectangle(i * width,
                    (row - 1) * height, width, height);
            }
            Animations.Add(name, recs);
        }

        public void SetFrame(int frame)
        {
            if (frame < recs.Length)
                FrameIndex = frame;
        }

        public override void Update(GameTime gameTime)
        {
            timeElapsed += (float)
                gameTime.ElapsedGameTime.TotalSeconds;

            if (timeElapsed > timeToUpdate)
            {
                timeElapsed -= timeToUpdate;

                if (FrameIndex < recs.Length - 1)
                    FrameIndex++;
                else if (IsLooping)
                    FrameIndex = 0;
            }

            mPosition.Y += mSpeed;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gametime)
        {
            spriteBatch.Draw(mTexture, mPosition, Animations[Animation][FrameIndex],
                Color, Rotation, Origin, Scale, SpriteEffect, 0f);
        }

        public Rectangle getSpriteRectangle()
        {
            return new Rectangle((int)mPosition.X, (int)mPosition.Y, width, height);
        }

        public Boolean isLastFrame()
        {
            return FrameIndex == recs.Length - 1;
        }
    }
}
