using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StylishAction.Device;
using StylishAction.Utility;

namespace StylishAction.Object
{
    class Player : Object
    {
        private float mSpeed;
        private float mVelocityY;
        private float mJumpPower;
        private float mGravity;
        private int mJumpCount;

        public Player(string name) : base(name)
        {
            mSpeed = 5;
            mJumpPower = 20;
            mGravity = 1;
        }

        public override void Initialize()
        {
            base.Initialize();
            mPosition = Vector2.Zero;
            mVelocityY = 0;
            mJumpCount = 0;
        }

        public override void Update(GameTime gameTime)
        {
            Fall();
            Jump();
            Translate(new Vector2(Input.Velocity().X * mSpeed, mVelocityY));
        }

        private void Jump()
        {
            if (Input.GetKeyTrigger(Keys.Space) && mJumpCount > 0)
            {
                mVelocityY = -mJumpPower;
                mJumpCount--;
            }
        }

        private void Fall()
        {
            if (mPosition.Y < Screen.HEIGHT - 32)
            {
                mVelocityY += mGravity;
                mVelocityY = ((mVelocityY >= 100) ? 100 : mVelocityY);
            }
            else
            {
                mPosition.Y = Screen.HEIGHT - 32;
                mVelocityY = 0;
                mJumpCount = 2;
            }
        }
    }
}
