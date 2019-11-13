using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StylishAction.Device;

namespace StylishAction.Object
{
    class Enemy : Character
    {
        private enum MoveState
        {
            Stay,
            Walk,
            JumpUp,
            JumpDown,
        }
        private MoveState mMoveState;

        public Enemy(string name, Vector2 size) : base(name, size)
        {
            mGravity = 1;
            mMaxHitPoint = 1;
        }

        public override void Initialize()
        {
            base.Initialize();
            mPosition = new Vector2(500,500);
            mMoveState = MoveState.JumpDown;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Fall();
            Translate(mVelocity);

            if(mHitPoint <= 0)
            {
                mIsDead = true;
            }
        }

        public override void Translate(Vector2 translation)
        {
            //壁や床にめり込まないように1ピクセルずつ当たり判定を取る
            for (int i = 0; i < Math.Abs(translation.X); i++)
            {
                float x = translation.X / Math.Abs(translation.X);
                if (ObjectManager.Instance().IsStageCollisionX(mOrigin + new Vector2(x, 0), mSize))
                    break;

                mPosition.X += x;
                mOrigin.X += x;
            }

            for (int i = 0; i < Math.Abs(translation.Y); i++)
            {
                float y = translation.Y / Math.Abs(translation.Y);
                if (ObjectManager.Instance().IsStageCollisionY(mOrigin + new Vector2(0, y), mSize))
                {
                    if (y > 0)
                    {
                        Landing();
                    }
                    else
                    {
                        mVelocity.Y = 0;
                    }
                    break;
                }

                mPosition.Y += y;
                mOrigin.Y += y;
            }
        }

        private void Landing()
        {
            mVelocity.Y = 0;
            mMoveState = MoveState.Stay;
        }

        private void Fall()
        {
            if (mMoveState != MoveState.Stay)
            {
                mVelocity.Y += mGravity;
                mVelocity.Y = ((mVelocity.Y >= 100) ? 100 : mVelocity.Y);
            }
            if (mVelocity.Y > 0)
            {
                mMoveState = MoveState.JumpDown;
            }
        }

        public override void Collision(Object other)
        {
            if(other is PlayerWeakAttack)
            {
                mHitPoint--;
            }
        }
    }
}
