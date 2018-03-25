using Microsoft.Xna.Framework;
using MonoGameBreakout;
using MonoGameLibrary.Util;
using System.Collections.Generic;

namespace BreakoutTest
{
    class BlockManager : DrawableGameComponent
    {
        public List<Block> Blocks { get; private set; } //List of Blocks the are managed by Block Manager

        // Dependancy on Ball
        Ball ball;

        GameConsole console;

        public int levelNumber;
        
        List<Block> blocksToRemove; //list of block to remove probably because they were hit

        /// <summary>
        /// BlockManager hold a list of blocks and handles updating, drawing a block collision
        /// </summary>
        /// <param name="game">Reference to Game</param>
        /// <param name="ball">Refernce to Ball for collision</param>
        public BlockManager(Game game, Ball b) : base(game)
        {
            this.Blocks = new List<Block>();
            this.blocksToRemove = new List<Block>();
            console = (GameConsole)this.Game.Services.GetService(typeof(IGameConsole));
            this.ball = b;
        }

        public override void Initialize()
        {
            levelNumber = 1;
            LoadLevel(levelNumber);
            base.Initialize();
        }

        /// <summary>
        /// Replacable Method to Load a Level by filling the Blocks List with Blocks
        /// </summary>
        protected virtual void LoadLevel(int Level)
        {
            if(Level == 1)
            {
                CreateBlockArrayByWidthAndHeight(6, 1, 25);
            }
            if (Level == 2)
            {
                CreateBlockArrayByWidthAndHeight(6, 3, 10);
            }
            if (Level == 3)
            {
                CreateBlockArrayByWidthAndHeight(8, 5, 10);
            }
        }

        /// <summary>
        /// Simple Level lays out multiple levels of blocks
        /// </summary>
        /// <param name="width">Number of blocks wide</param>
        /// <param name="height">Number of blocks high</param>
        /// <param name="margin">space between blocks</param>
        private void CreateBlockArrayByWidthAndHeight(int width, int height, int margin)
        {
            Block b;
            //Create Block Array based on with and hieght
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    b = new Block(this.Game);
                    b.Initialize();
                    b.Location = new Vector2((4 * width * width)  + (w * b.SpriteTexture.Width + (w * margin)), 100 + (h * b.SpriteTexture.Height + (h * margin)));
                    Blocks.Add(b);
                }
            }
        }
        
        bool reflected; //the ball should only reflect once even if it hits two bricks
        public override void Update(GameTime gameTime)
        {
            this.reflected = false; //only reflect once per update
            UpdateCheckBlocksForCollision(gameTime);
            UpdateRemoveDisabledBlocks();
            updateLevel();
            UpdateGameState();

            base.Update(gameTime);
        }

        /// <summary>
        /// Removes disabled blocks from list
        /// </summary>
        private void UpdateRemoveDisabledBlocks()
        {
            // Remove disabled blocks
            foreach (var block in blocksToRemove)
            {
                Blocks.Remove(block);
                ScoreManager.Score++;
            }
            blocksToRemove.Clear();
        }

        private void UpdateCheckBlocksForCollision(GameTime gameTime)
        {
            foreach (Block b in Blocks)
            {
                if (b.Enabled) //Only chack active blocks
                {
                    b.Update(gameTime); //Update Block
                    //Ball Collision
                    if (b.Intersects(ball)) //chek rectagle collision between ball and current block 
                    {
                        //hit
                        b.HitByBall(ball);

                        blocksToRemove.Add(b);  //Ball is hit add it to remove list
                        if (!reflected) //only reflect once
                        {
                            ball.Reflect(b);
                            this.reflected = true;
                        }
                    }
                }
            }
        }

        private void updateLevel()
        {
            if(Blocks.Count == 0)
            {
                levelNumber++;
                ball.resetBall();
                ScoreManager.Level += 1;
                ScoreManager.Lives = 3;
                LoadLevel(levelNumber);
            }
            else if(levelNumber > 3)
            {
                levelNumber = 1;
                ball.resetBall();
                ScoreManager.Level = 1;
                ScoreManager.Lives = 3;
                LoadLevel(levelNumber);
            }
        }

        private void UpdateGameState()
        {
            if (ScoreManager.Lives == 0)
            {
                this.console.GameConsoleWrite("You Lost");
                ball.resetBall();
                ScoreManager.Level = 1;
                ScoreManager.Lives = 3;
                ScoreManager.Score = 0;
                LoadLevel(1);
            }
        }

        private void PlayerWonGame()
        {
            if(ScoreManager.Level == 3 && Blocks.Count == 0)
            {
                this.console.GameConsoleWrite("You Won!");
            }
        }
        
        public override void Draw(GameTime gameTime)
        {
            foreach (var block in this.Blocks)
            {
                block.Draw(gameTime);
            }
            base.Draw(gameTime);
        }
    }
}
