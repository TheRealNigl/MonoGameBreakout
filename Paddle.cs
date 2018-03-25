using MonoGameLibrary.Util;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Sprite;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameBreakout
{
    class Paddle : DrawableSprite
    {
        // Service Dependencies
        //GameConsole console;

        // Dependencies
        PaddleController controller;
        Ball ball;      //Need reference to ball for collision

        public Paddle(Game game, Ball b) : base(game)
        {
            this.Speed = 200;
            this.ball = b;
            controller = new PaddleController(game, ball);

            //console = (GameConsole)this.Game.Services.GetService(typeof(IGameConsole));
            //if (console == null) //ohh no no console
            //{
            //    console = new GameConsole(this.Game);
            //    this.Game.Components.Add(console);  //add a new game console to Game
            //}
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("paddleSmall");
            SetInitialLocation();
            base.LoadContent();
        }

        public void SetInitialLocation()
        {
            this.Location = new Vector2(this.Game.GraphicsDevice.Viewport.Width / 2, 
                this.Game.GraphicsDevice.Viewport.Height - this.spriteTexture.Height);
        }

        Rectangle collisionRectangle;  //Rectangle for paddle collision uses just the top of the paddle

        public override void Update(GameTime gameTime)
        {
            //Update Collision Rect
            collisionRectangle = new Rectangle((int)this.Location.X, 
                (int)this.Location.Y, this.spriteTexture.Width, 1);

            //Deal with ball
            switch (ball.State)
            {
                case BallState.OnPaddleStart:
                    //Move the ball with the paddle until launch
                    UpdateMoveBallWithPaddle();
                    break;
                case BallState.Playing:
                    UpdateCheckBallCollision();
                    break;
            }

            //Movement from controller
            controller.HandleInput(gameTime);
            this.Direction = controller.Direction;
            this.Location += this.Direction * (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);

            KeepPaddleOnScreen();
            base.Update(gameTime);
        }

        private void UpdateMoveBallWithPaddle()
        {
            ball.Speed = 0;
            ball.Direction = Vector2.Zero;
            ball.Location = new Vector2(this.Location.X + (this.LocationRect.Width / 2 - ball.spriteTexture.Width / 2), this.Location.Y - ball.spriteTexture.Height);
        }

        private void UpdateCheckBallCollision()
        {
            //Ball Collsion
            if (this.collisionRectangle.Intersects(ball.LocationRect))
            {
                if (PerPixelCollision(this))
                {
                    ball.Direction += new Vector2(0, -1);
                    //console.GameConsoleWrite("You saved the ball!");
                }
            }
        }
        
        private void KeepPaddleOnScreen()
        {
            this.Location.X = MathHelper.Clamp(this.Location.X, 0, this.Game.GraphicsDevice.Viewport.Width - this.spriteTexture.Width);
        }

        private void Reflect()
        {
            if (this.collisionRectangle.Intersects(ball.LocationRect))
            {
                this.ball.Direction.Y *= -1;
            }
        }
    }
}
