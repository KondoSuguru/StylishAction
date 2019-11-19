using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StylishAction.Device;
using StylishAction.Utility;

namespace StylishAction.Object
{
    class Enemy : Character
    {
        private bool mIsPlayerAttackCollision;
        private CountDownTimer mTimer;

        private enum MoveState
        {
            Stay,
            Walk,
            JumpUp,
            JumpDown,
        }
        private MoveState mMoveState;

        public Enemy(string name, Vector2 size, float speed) : base(name, size)
        {
            mSpeed = speed;
            mGravity = 50;
            mMaxHitPoint = 1;
        }

        public override void Initialize()
        {
            base.Initialize();
            mPosition = new Vector2(500,500);
            mMoveState = MoveState.JumpDown;
            mIsPlayerAttackCollision = false;
            mVelocity = new Vector2(1, 0);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            Fall();
            Translate(new Vector2( mVelocity.X * mSpeed, mVelocity.Y) * deltaTime);

            if(mHitPoint <= 0)
            {
                mIsDead = true;
            }

            if (mIsPlayerAttackCollision)
            {
                mTimer.Update(deltaTime);
                if (mTimer.IsTime())
                {
                    mTimer.Initialize();
                    mIsPlayerAttackCollision = false;
                }
            }
        }

        public override void Translate(Vector2 translation)
        {
            //壁や床にめり込まないように1ピクセルずつ当たり判定を取る
            for (int i = 0; i < Math.Abs(translation.X); i++)
            {
                float x = translation.X / Math.Abs(translation.X);
                if (ObjectManager.Instance().IsStageCollisionX(mOrigin + new Vector2(x, 0), mSize))
                {
                    mVelocity.X *= -1;//反転
                    break;
                }

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
            }
            if (mVelocity.Y > 0)
            {
                mMoveState = MoveState.JumpDown;
            }
        }

        public override void Collision(Object other)
        {
            if(other is PlayerWeakAttack && !mIsPlayerAttackCollision)
            {
                Random r = GameDevice.Instance().GetRandom();
                mColor = new Color(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
                //mHitPoint--;

                //mTimer.SetTime(((PlayerWeakAttack)other).GetLimitTime() + 0.05f);
                mTimer  = new CountDownTimer(((PlayerWeakAttack)other).GetLimitTime() + 0.05f);
                mIsPlayerAttackCollision = true;

                HitStop.mHitStopTime = 0.3f;
                HitStop.mIsHitStop = true;
            }
        }
    }
}
