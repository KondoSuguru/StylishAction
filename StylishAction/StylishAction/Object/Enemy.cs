using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StylishAction.Device;
using StylishAction.Utility;
using StylishAction.Effect;

namespace StylishAction.Object
{
    class Enemy : Character
    {
        private bool mIsPlayerAttackCollision;
        private CountDownTimer mIsCollisionTimer;
        private CountDownTimer mStayTimer;
        private CountDownTimer mDashTimer;
        private float mDashSpeed;
        private CountDownTimer mJumpTimer;
        private float mJumpPower;
        private CountDownTimer mAttackTimer;
        private float mHitStopTimer;

        private enum MoveState
        {
            Stay,
            Walk,
            JumpUp,
            JumpDown,
        }
        private MoveState mMoveState;

        private enum AttackState
        {
            Stay,
            Dash,
            Jump,
            Attack,
        }
        private AttackState mAttackState;

        public Enemy(string name, Vector2 size) : base(name, size)
        {
            mSpeed = 300;
            mDashSpeed = 1500;
            mJumpPower = 1500;
            mGravity = 50;
            mMaxHitPoint = 10;
            mStayTimer = new CountDownTimer(0.5f);
            mDashTimer = new CountDownTimer(1f);
            mJumpTimer = new CountDownTimer(1.2f);
            mAttackTimer = new CountDownTimer(0.5f);
            mHitStopTimer = 0.1f;
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            mPosition = new Vector2(1000, 600);
            mOrigin = new Vector2(mPosition.X + (mSize.X / 2), mPosition.Y + (mSize.Y / 2));
            mMoveState = MoveState.JumpDown;
            mIsPlayerAttackCollision = false;
            mVelocity = new Vector2(1, 0);
            mAttackState = AttackState.Stay;
        }

        public override void Update(float deltaTime)
        {
            if (HitStop.mIsHitStop)
                return;
            base.Update(deltaTime);

            switch (mAttackState)
            {
                case AttackState.Stay:
                    StayUpdate(deltaTime);
                    break;
                case AttackState.Dash:
                    DashUpdate(deltaTime);
                    break;
                case AttackState.Jump:
                    JumpUpdate(deltaTime);
                    break;
                case AttackState.Attack:
                    AttackUpdate(deltaTime);
                    break;
            }
            Fall();
            Translate(mVelocity * deltaTime);

            if (mIsPlayerAttackCollision)
            {
                mIsCollisionTimer.Update(deltaTime);

                mHitStopTimer -= deltaTime;

                if(mHitStopTimer <= 0)
                {
                    mHitStopTimer = 0.1f;
                    HitStop.mHitStopScale = 1.6f;
                    HitStop.mHitStopTime = 0.3f;
                    HitStop.mIsHitStop = true;
                }
                if (mIsCollisionTimer.IsTime())
                {
                    mIsCollisionTimer.Initialize();
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

            mVelocity.Y += mGravity;
            if (mVelocity.Y > 0)
            {
                mMoveState = MoveState.JumpDown;
            }
        }

        public override void Collision(Object other)
        {
            if (other is PlayerWeakAttack && !mIsPlayerAttackCollision)
            {
                GameDevice.Instance().GetSound().PlaySE("damageSE");
                CreateDamageEffect();
                mHitPoint--;

                mIsCollisionTimer = new CountDownTimer(((PlayerWeakAttack)other).GetLimitTime() + 0.01f);
                mIsPlayerAttackCollision = true;

                //HitStop.mHitStopScale = 1.5f;
                //HitStop.mHitStopTime = 0.3f;
                //HitStop.mIsHitStop = true;
            }
        }

        private void StayUpdate(float deltaTime)
        {
            mStayTimer.Update(deltaTime);
            mVelocity.X = 0;

            if (ObjectManager.Instance().GetPlayer() != null)
            {
                float playerDir = ObjectManager.Instance().GetPlayer().GetOrigin().X - mOrigin.X;
                if (playerDir > 0)
                {
                    mCurrentDir = Direction.Right;
                }
                else
                {
                    mCurrentDir = Direction.Left;
                }
            }

            if (mCurrentDir == Direction.Left)
            {
                mName = "boss_left";
            }
            if (mCurrentDir == Direction.Right)
            {
                mName = "boss_right";
            }

            if (mStayTimer.IsTime())
            {
                mStayTimer.Initialize();
                if (ObjectManager.Instance().GetPlayer() != null)
                {
                    if (Vector2.Distance(ObjectManager.Instance().GetPlayer().GetOrigin(), mOrigin) >= Screen.WIDTH / 3)
                    {
                        mAttackState = (AttackState)GameDevice.Instance().GetRandom().Next((int)AttackState.Dash, (int)AttackState.Jump + 1);
                        //mAttackState = AttackState.Dash;
                    }
                    else
                    {
                        mAttackState = (AttackState)GameDevice.Instance().GetRandom().Next((int)AttackState.Jump, (int)AttackState.Attack + 1);
                    }
                }
            }
        }

        private void DashUpdate(float deltaTime)
        {
            mDashTimer.Update(deltaTime);

            if (mDashTimer.Rate() <= 0.3f)
            {
                mVelocity.X = GameDevice.Instance().GetRandom().Next(-1, 2) * mSpeed;
            }
            else
            {
                mVelocity.Y = 0;
                mVelocity.X = ((int)mCurrentDir - 2) * (mDashSpeed - (mDashTimer.Rate() * 500));
                if(mHitPoint <= mMaxHitPoint / 2)
                {
                    mVelocity.X = ((int)mCurrentDir - 2) * (mDashSpeed * 1.2f - (mDashTimer.Rate() * 500));
                }
            }

            if (mDashTimer.IsTime())
            {
                mDashTimer.Initialize();
                mAttackState = AttackState.Stay;
                if (mHitPoint <= mMaxHitPoint / 3)
                    mAttackState = AttackState.Attack;
            }
        }

        private void AttackUpdate(float deltaTime)
        {
            mAttackTimer.Update(deltaTime);
            if (mAttackTimer.IsTime())
            {
                mAttackTimer.Initialize();
                int min = 0;
                int max = 0;
                if(mCurrentDir == Direction.Right)
                {
                    min = 0;
                    max = 2;
                }
                else
                {
                    min = -1;
                    max = 1;
                }
                
                if (mHitPoint <= mMaxHitPoint / 2)
                {
                    if (mCurrentDir == Direction.Right)
                    {
                        min = 0;
                        max = 3;
                    }
                    else
                    {
                        min = -2;
                        max = 1;
                    }
                }

                for (int i = min; i < max; i++)
                {
                    for (int j =  -1; j < 2; j++)
                    {
                        new BossAttack("boss", new Vector2(64, 64), mOrigin, new Vector2(i, j));
                    }
                }
                GameDevice.Instance().GetSound().PlaySE("shotSE");
                mAttackState = AttackState.Stay;
            }
            else
            {
                mVelocity.X = GameDevice.Instance().GetRandom().Next(-1, 2) * mSpeed;
            }
        }

        private void JumpUpdate(float deltaTime)
        {
            mJumpTimer.Update(deltaTime);
            if (mJumpTimer.Rate() <= 0.1f)
            {
                mVelocity.Y = -mJumpPower;
                if (mHitPoint <= mMaxHitPoint / 2)
                {
                    mVelocity.Y = -mJumpPower * 1.2f;
                }
            }
            mVelocity.X = ((int)mCurrentDir - 2) * mSpeed;
            if(mHitPoint <= mMaxHitPoint / 2)
            {
                mVelocity.X = ((int)mCurrentDir - 2) * mSpeed * 1.2f;
            }

            if (mJumpTimer.IsTime())
            {
                mJumpTimer.Initialize();
                mAttackState = AttackState.Stay;
            }
        }

        private void CreateDamageEffect()
        {
            Random rand = GameDevice.Instance().GetRandom();
            for (int i = 0; i < 10; i++)
            {
                new DamageEffect("sikaku", new Vector2(64, 64), mOrigin, new Vector2(rand.Next(-1,2), rand.Next(-1, 2)));
            }
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
