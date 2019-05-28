using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EarthDefender
{
    class MainScene : Scene
    {
        SpriteFont mFont;
        KeyboardState mKbState;
        SpriteBatch mSpriteBatch;
        Game mGame;

        Texture2D mOnePlayerButton;
        Vector2 mOnePlayerButtonPosition;

        Texture2D mTwoPlayerButton;
        Vector2 mTwoPlayerButtonPosition;

        Texture2D mBackground;
        Rectangle mMainFrame;

        public String mInstructionMessage = "Controls & Instructions:\n" +
                                     "Player 1 uses arrow buttons left/right to move + Right CTRL to shoot.\n" +
                                     "Player 2 uses A/D to move + SPACE to shoot.\n" +
                                     "The goal is to protect earth from asteroids, you have 3 lives, try to score as much as you can!\n" +
                                     "ESC to exit/finish game\n";

        public MainScene(Game game) : base(game)
        {
            this.mGame = game;
            mSpriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            mOnePlayerButton = mGame.Content.Load<Texture2D>("singlePlayer");
            mTwoPlayerButton = mGame.Content.Load<Texture2D>("multiplayer");

            mOnePlayerButtonPosition = new Vector2(mGame.GraphicsDevice.Viewport.Width / 2 - mOnePlayerButton.Width / 2, mGame.GraphicsDevice.Viewport.Height / 10);
            mTwoPlayerButtonPosition = new Vector2(mGame.GraphicsDevice.Viewport.Width / 2 - mTwoPlayerButton.Width / 2, 3 * mGame.GraphicsDevice.Viewport.Height / 10);


            mBackground = mGame.Content.Load<Texture2D>("background_opt2");

            // set main screen size
            mMainFrame = new Rectangle(0, 0, mGame.GraphicsDevice.Viewport.Width, mGame.GraphicsDevice.Viewport.Height);

            mFont = (SpriteFont)game.Services.GetService(typeof(SpriteFont));
        }

        //protected override void Dispose(bool disposing) { dice.Dispose();  base.Dispose(disposing); }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void _initialize() { }
        public override void Draw(GameTime gameTime)
        {
            mSpriteBatch.Draw(mBackground, mMainFrame, Color.White);

            mSpriteBatch.DrawString(mFont, "Earth Defender", new Vector2(50, 20), Color.White);

            // write game instructions
            mSpriteBatch.DrawString(mFont, mInstructionMessage, new Vector2(50, 4 * mGame.GraphicsDevice.Viewport.Height / 10), Color.Black);

            mSpriteBatch.DrawString(mFont, "Mark Lurie", new Vector2(mGame.GraphicsDevice.Viewport.Width - 130, Game.GraphicsDevice.Viewport.Height - 30), Color.LawnGreen);

            mSpriteBatch.Draw(mOnePlayerButton, mOnePlayerButtonPosition, Color.White);
            mSpriteBatch.Draw(mTwoPlayerButton, mTwoPlayerButtonPosition, Color.White);
            base.Draw(gameTime);
        }

        public Rectangle getOnePlayerRect()
        {
            return new Rectangle((int)mOnePlayerButtonPosition.X, (int)mOnePlayerButtonPosition.Y, mOnePlayerButton.Width, mOnePlayerButton.Height);
        }

        public Rectangle getTwoPlayerRect()
        {
            return new Rectangle((int)mTwoPlayerButtonPosition.X, (int)mTwoPlayerButtonPosition.Y, mTwoPlayerButton.Width, mTwoPlayerButton.Height);
        }

    }
}
