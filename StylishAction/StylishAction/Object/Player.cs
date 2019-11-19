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
    class Player : Character
    {
        private readonly float mDashSpeed;
        private CountDownTimer mDashTimer;
        private int mDashCount;
        private readonly float mJumpPower;
        private int mJumpCount;
        private bool mIsInvisible;
        private CountDownTimer mInvisibleTimer;

        private enum MoveState
        {
            Stay,
            Walk,
            Dash,
            JumpUp,
            JumpDown,
        }
        private MoveState mMoveState;

        private enum AttackState
        {
            WeakAttack,
            StrongAttack,
            None,
        }
        private AttackState mAttackState;
        private CountDownTimer mWeakAttackTimer;

        public Player(string name, Vector2 size) : base(name, size)
        {
            mSpeed = 300;
            mDashSpeed = 1500;
            mJumpPower = 1200;
            mGravity = 50;
            mMaxHitPoint = 3;
        }

        public override void Initialize()
        {
            base.Initialize();
            mPosition = new Vector2(100,100);
            mDashTimer = new CountDownTimer(0.2f);
            mDashCount = 2;
            mJumpCount = 0;
            mIsInvisible = false;
            mInvisibleTimer = new CountDownTimer(1);
            mWeakAttackTimer = new CountDownTimer(0.1f);
            mMoveState = MoveState.JumpDown;
            mAttackState = AttackState.None;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            InvisibleUpdate(deltaTime);

            if (mMoveState != MoveState.Dash)
            {
                DirectionUpdate();
                InputX();
                Fall();
                Jump();
                WeakAttack(deltaTime);
            }
            Dash(deltaTime);

            if (mAttackState == AttackState.None)
            {
                Translate(mVelocity * deltaTime);
            }
            MoveStateUpdate();
        }

        public override void Translate(Vector2 translation)
        {
            //壁や床にめり込まないように1ピクセルずつ当たり判定を取る
            for(int i = 0; i < Math.Abs(translation.X); i++)
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
                    if(y > 0)
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
            mJumpCount = 2;
            mDashCount = 2;
        }

        private void InputX()
        {
            mVelocity.X = Input.Velocity().X * mSpeed;
        }

        private void Jump()
        {
            if (Input.GetKeyTrigger(Keys.Space) && mJumpCount > 0)
            {
                mMoveState = MoveState.JumpUp;
                mVelocity.Y = -mJumpPower;
                mJumpCount--;
            }
        }

        private void Dash(float deltaTime)
        {
            if (mMoveState != MoveState.Dash && Input.GetKeyTrigger(Keys.X) && mDashCount > 0)
            {
                mMoveState = MoveState.Dash;
                mDashCount--;
            }
            if (mMoveState == MoveState.Dash)
            {
                mDashTimer.Update(deltaTime);
                mVelocity.Y = 0;
                mVelocity.X = ((int)mPreviousDir - 2) * mDashSpeed;

                    //ダッシュ状態中、先行入力受付
                Input.SetBufferKey(Keys.Space);
                Input.SetBufferKey(Keys.Z);
                //Input.SetBufferKey(Keys.X);
                
                if (mDashTimer.IsTime())
                {
                    mMoveState = MoveState.JumpDown;
                    mDashTimer.Initialize();

                    Input.BufferInput();
                }
            }
        }

        private void Fall()
        {
            mVelocity.Y += mGravity;
            //mVelocity.Y = ((mVelocity.Y >= 100) ? 100 : mVelocity.Y);
        }

        private void WeakAttack(float deltaTime)
        {
            if (Input.GetKeyTrigger(Keys.Z) && mAttackState == AttackState.None)
            {
                new PlayerWeakAttack("enemy", new Vector2(32, 32), mOrigin, (int)mCurrentDir, new CountDownTimer(0.1f));
                mAttackState = AttackState.WeakAttack;
            }

            if(mAttackState == AttackState.WeakAttack)
            {
                mVelocity = Vector2.Zero;
                mWeakAttackTimer.Update(deltaTime);
                if(mWeakAttackTimer.IsTime())
                {
                    mWeakAttackTimer.Initialize();
                    mAttackState = AttackState.None;
                }
            }
        }

        private void DirectionUpdate()
        {
            if (Input.GetKeyState(Keys.Right))
            {
                mCurrentDir = Direction.Right;
                mPreviousDir = mCurrentDir;
            }
            if (Input.GetKeyState(Keys.Left))
            {
                mCurrentDir = Direction.Left;
                mPreviousDir = mCurrentDir;
            }

            if (Input.GetKeyState(Keys.Up))
            {
                mCurrentDir = Direction.Up;
            }
            if(Input.GetKeyUp(Keys.Up))
            {
                mCurrentDir = mPreviousDir;
            }

            if (Input.GetKeyState(Keys.Down))
            {
                mCurrentDir = Direction.Down;
            }
            if(Input.GetKeyUp(Keys.Down))
            {
                mCurrentDir = mPreviousDir;
            }
        }

        private void InvisibleUpdate(float deltaTime)
        {
            if (mIsInvisible)
            {
                SetAlpha(0.1f);
                mInvisibleTimer.Update(deltaTime);
                if(mInvisibleTimer.IsTime())
                {
                    SetAlpha(1.0f);
                    mIsInvisible = false;
                    mInvisibleTimer.Initialize();
                }
            }
        }

        private void MoveStateUpdate()
        {
            if(mMoveState == MoveState.Dash)
            {
                return;
            }
            if(mVelocity == Vector2.Zero)
            {
                mMoveState = MoveState.Stay;
            }
            if(mVelocity.X != 0)
            {
                mMoveState = MoveState.Walk;
            }
            if(mVelocity.Y < 0)
            {
                mMoveState = MoveState.JumpUp;
            }
            if(mVelocity.Y > 0)
            {
                mMoveState = MoveState.JumpDown;
            }
        }

        public override void Draw()
        {
            //stateで色を変える（テスト）
            switch (mMoveState)
            {
                case MoveState.Stay:
                    SetColor(new Color(255,255,255));
                    break;
                case MoveState.Walk:
                    SetColor(new Color(255, 0, 0));
                    break;
                case MoveState.JumpUp:
                    SetColor(new Color(0, 255, 0));
                    break;
                case MoveState.JumpDown:
                    SetColor(new Color(0, 0, 255));
                    break;
                case MoveState.Dash:
                    SetColor(new Color(255, 0, 255));
                    break;
            }
            base.Draw();
        }

        public override void Collision(Object other)
        {
            if (other is Enemy)
            {
                if (mMoveState != MoveState.Dash && !mIsInvisible)
                {
                    mIsInvisible = true;
                    mHitPoint--;
                }
            }
        }
    }
}
