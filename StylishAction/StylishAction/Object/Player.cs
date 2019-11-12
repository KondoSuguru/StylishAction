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
        private readonly float mSpeed;
        private readonly float mJumpPower;
        private readonly float mGravity;
        private Vector2 mVelocity;
        private int mJumpCount;
        private int mHitPoint;
        private int mMaxHitPoint;

        public Player(string name, int size) : base(name, size)
        {
            mSpeed = 5;
            mJumpPower = 20;
            mGravity = 1;
            mMaxHitPoint = 1;
        }

        public override void Initialize()
        {
            base.Initialize();
            mPosition = Vector2.Zero;
            mVelocity = Vector2.Zero;
            mJumpCount = 0;
            mHitPoint = mMaxHitPoint;
        }

        public override void Update(GameTime gameTime)
        {
            Move();
            Fall();
            Jump();
            Translate(mVelocity);

            if(mHitPoint <= 0)
            {
                mIsDead = true;
            }
        }

        private void Move()
        {
            mVelocity.X = Input.Velocity().X * mSpeed;
        }

        private void Jump()
        {
            if (Input.GetKeyTrigger(Keys.Space) && mJumpCount > 0)
            {
                mVelocity.Y = -mJumpPower;
                mJumpCount--;
            }
        }

        private void Fall()
        {
            if (mPosition.Y < Screen.HEIGHT - 32)
            {
                mVelocity.Y += mGravity;
                mVelocity.Y = ((mVelocity.Y >= 100) ? 100 : mVelocity.Y);
            }
            else
            {
                mPosition.Y = Screen.HEIGHT - 32;
                mVelocity.Y = 0;
                mJumpCount = 2;
            }
        }

        public override void Collision(Object other)
        {
            if(other is Enemy)
            {
                mHitPoint--;
            }
        }
    }
}
