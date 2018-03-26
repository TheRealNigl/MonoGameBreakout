using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameLibrary.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;


namespace BreakoutTest
{
    class ScoreManager : DrawableGameComponent
    {
        SpriteFont font;
        public static int Lives;
        public static int Level;
        public static int Score;

        Texture2D paddle;

        SpriteBatch sb;
        Vector2 scoreLoc, livesLoc, levelLoc;
        
        public ScoreManager(Game game) : base(game)
        {
            SetupNewGame();
        }

        private static void SetupNewGame()
        {
            Lives = 3;
            Level = 1;
            Score = 0;
        }

        protected override void LoadContent()
        {
            sb = new SpriteBatch(this.Game.GraphicsDevice);
            font = this.Game.Content.Load<SpriteFont>("Arial");
            paddle = this.Game.Content.Load<Texture2D>("paddleSmall");
            livesLoc = new Vector2(10,
                this.Game.GraphicsDevice.Viewport.Height - 25);
            levelLoc = new Vector2(70,
                this.Game.GraphicsDevice.Viewport.Height - 25);
            scoreLoc = new Vector2(130,
                this.Game.GraphicsDevice.Viewport.Height - 25);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();

            for (int i = 0; i < Lives; i++)
            {
                sb.Draw(paddle, new Rectangle(10 + (paddle.Width * i),
                    this.Game.GraphicsDevice.Viewport.Height - 50, paddle.Width / 2, 
                    paddle.Height / 2), Color.White);
            }

            sb.DrawString(font, "Lives: " + Lives, livesLoc, Color.White);
            sb.DrawString(font, "Score: " + Score, scoreLoc, Color.White);
            sb.DrawString(font, "Level: " + Level, levelLoc, Color.White);
            sb.End();

            base.Draw(gameTime);
        }
    }
}
