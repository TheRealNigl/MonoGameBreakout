using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameLibrary.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameBreakout;

namespace BreakoutTest
{
    enum BlockState { Normal, Broken, Red, Yellow };

    class Block : DrawableSprite
    {
        protected BlockState state;            //Private instance datamenber for block
        public BlockState State
        {
            get { return state; }
            protected set
            {
                if (this.state != value)       //Change state if it is different than previous state                
                {
                    this.state = value;
                }
            }
        }

        public Block(Game game): base(game)
        {
            this.state = BlockState.Normal;
            this.scale = 0.75f;
        }

        protected override void LoadContent()
        {
            switch (this.state)
            {
                case BlockState.Normal:
                    this.spriteTexture = this.Game.Content.Load<Texture2D>("block_blue");
                    break;
                case BlockState.Red:
                    this.spriteTexture = this.Game.Content.Load<Texture2D>("block_red");
                    break;
                case BlockState.Yellow:
                    this.spriteTexture = this.Game.Content.Load<Texture2D>("block_yellow");
                    break;
                default:
                    break;
            }
            base.LoadContent();
        }

        /// <summary>
        /// Checks if ball is hit by block
        /// </summary>
        /// <param name="ball"></param>
        internal void HitByBall(Ball ball)
        {
            this.Enabled = false;
            this.Visible = false;
            this.State = BlockState.Broken;
        }
    }
}