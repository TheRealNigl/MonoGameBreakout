using MonoGameLibrary.Util;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Sprite;
using Microsoft.Xna.Framework.Graphics;
using BreakoutTest;

namespace MonoGameBreakout
{
    enum BallState { OnPaddleStart, Playing }

    class Ball : DrawableSprite
    {
        public BallState State { get; private set; }

        //GameConsole console;

        public Ball(Game game) : base(game)
        {
            this.State = BallState.OnPaddleStart;

            //console = (GameConsole)this.Game.Services.GetService(typeof(IGameConsole));
            //if (console == null) //ohh no no console
            //{
            //    console = new GameConsole(this.Game);
            //    this.Game.Components.Add(console);  //add a new game console to Game
            //}
        }

        public void SetInitialLocation()
        {
            this.Location = new Vector2(200, 300);
        }

        public void LaunchBall(GameTime gameTime)
        {
            this.Speed = 190;
            this.Direction = new Vector2(1, -1);
            this.State = BallState.Playing;
            //this.console.GameConsoleWrite("Ball Launched " + gameTime.TotalGameTime.ToString());
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("ballSmall");
            SetInitialLocation();
            base.LoadContent();
        }

        public void resetBall()
        {
            // potential arguement Gametime gametime
            this.Speed = 0;
            this.State = BallState.OnPaddleStart;
            //this.console.GameConsoleWrite("Ball Reset " + gameTime.TotalGameTime.ToString());
        }

        public override void Update(GameTime gameTime)
        {
            switch (this.State)
            {
                case BallState.OnPaddleStart:
                    break;

                case BallState.Playing:
                    UpdateBall(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        private void UpdateBall(GameTime gameTime)
        {
            this.Location += this.Direction * (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);

            //bounce off wall
            //Left and Right
            if ((this.Location.X + this.spriteTexture.Width > this.Game.GraphicsDevice.Viewport.Width)
                ||
                (this.Location.X < 0))
            {
                this.Direction.X *= -1;
            }
            //bottom Miss
            if (this.Location.Y + this.spriteTexture.Height > this.Game.GraphicsDevice.Viewport.Height)
            {
                this.Direction.Y *= -1;
                this.State = BallState.OnPaddleStart;
                ScoreManager.Lives -= 1;
                //console.GameConsoleWrite("Should lose life here!!!");
            }

            //Top
            if (this.Location.Y < 0)
            {
                this.Direction.Y *= -1;
            }
        }

        public void Reflect(Block block)
        {
            if (this.Intersects(block))
            {
                this.Direction.Y *= -1;
                this.Rotate = .15f;
            }
        }
        
    }
}
