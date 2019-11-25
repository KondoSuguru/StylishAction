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
        private CountDownTimer mDamageMovetimer;

        private enum MoveState
        {
            Stay,
            Walk,
            Dash,
            JumpUp,
            JumpDown,
            Damage,
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
            mDashSpeed = 1300;
            mJumpPower = 1200;
            mGravity = 50;
            mMaxHitPoint = 5;
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            mPosition = new Vector2(100,Screen.HEIGHT - 100);
            mOrigin = new Vector2(mPosition.X + (mSize.X / 2), mPosition.Y + (mSize.Y / 2));
            mDashTimer = new CountDownTimer(0.2f);
            mDashCount = 2;
            mJumpCount = 0;
            mIsInvisible = false;
            mInvisibleTimer = new CountDownTimer(1);
            mWeakAttackTimer = new CountDownTimer(0.1f);
            mDamageMovetimer = new CountDownTimer(0.5f);
            mMoveState = MoveState.JumpDown;
            mAttackState = AttackState.None;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            InvisibleUpdate(deltaTime);

            if (mMoveState != MoveState.Dash)
            {
                if(mMoveState != MoveState.Damage)
                {
                    DirectionUpdate();
                    InputX();
                    Jump();
                    WeakAttack(deltaTime);
                }                
                Fall();               
                DamageMove(deltaTime);           
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
            mJumpCount = 2;
            mDashCount = 2;
            if (mMoveState == MoveState.Damage)
                return;

            mMoveState = MoveState.Stay;
        }

        private void InputX()
        {
            mVelocity.X = Input.Velocity().X * mSpeed;
        }

        private void Jump()
        {
            if (Input.GetKeyTrigger(Keys.Space) && mJumpCount > 0)
            {
                GameDevice.Instance().GetSound().PlaySE("shotSE");
                mMoveState = MoveState.JumpUp;
                mVelocity.Y = -mJumpPower;
                mJumpCount--;
            }
        }

        private void Dash(float deltaTime)
        {
            if (mMoveState != MoveState.Dash && Input.GetKeyTrigger(Keys.X) && mDashCount > 0)
            {
                GameDevice.Instance().GetSound().PlaySE("shotSE");
                mMoveState = MoveState.Dash;
                SetAlpha(0.5f);
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
                    SetAlpha(1.0f);

                    Input.BufferInput();
                }
            }
        }

        private void DamageMove(float deltaTime)
        {
            if(mMoveState == MoveState.Damage)
            {
                mDamageMovetimer.Update(deltaTime);
                if (mDamageMovetimer.IsTime())
                {
                    mDamageMovetimer.Initialize();
                    mMoveState = MoveState.JumpDown;
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
                GameDevice.Instance().GetSound().PlaySE("shotSE");
                new PlayerWeakAttack("sikaku", new Vector2(32, 32), mOrigin, (int)mCurrentDir, new CountDownTimer(0.1f));
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
            switch (mCurrentDir)
            {
                case Direction.Right:
                    mName = "player_right";
                    break;
                case Direction.Left:
                    mName = "player_left";
                    break;
                case Direction.Up:
                    mName = "player_up";
                    break;
                case Direction.Down:
                    mName = "player_down";
                    break;
            }
        }

        private void InvisibleUpdate(float deltaTime)
        {
            if (mIsInvisible)
            {
                SetAlpha(0.5f);
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
            if(mMoveState == MoveState.Dash || mMoveState == MoveState.Damage)
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

        private void CreateDamageEffect()
        {
            Random rand = GameDevice.Instance().GetRandom();
            for (int i = 0; i < 10; i++)
            {
                new DamageEffect("sikaku", new Vector2(64, 64), mOrigin, new Vector2(rand.Next(-1, 2), rand.Next(-1, 2)));
            }
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void Collision(Object other)
        {
            if (other is Enemy)
            {
                if (mMoveState != MoveState.Dash && !mIsInvisible)
                {
                    GameDevice.Instance().GetSound().PlaySE("damageSE");
                    CreateDamageEffect();
                    HitStop.mHitStopScale = 1.05f;
                    HitStop.mHitStopTime = 0.1f;
                    HitStop.mIsHitStop = true;

                    mIsInvisible = true;
                    mHitPoint--;

                    mVelocity.X = Vector2.Normalize(mOrigin - other.GetOrigin()).X * mSpeed;
                    mVelocity.Y = -mJumpPower * 0.8f;

                    mMoveState = MoveState.Damage;
                }
            }

            if(other is BossAttack)
            {
                if (mMoveState != MoveState.Dash && !mIsInvisible)
                {
                    GameDevice.Instance().GetSound().PlaySE("damageSE");
                    CreateDamageEffect();
                    HitStop.mHitStopScale = 1.2f;
                    HitStop.mHitStopTime = 0.2f;
                    HitStop.mIsHitStop = true;

                    mIsInvisible = true;
                    mHitPoint--;

                    mVelocity.X = Vector2.Normalize(mOrigin - other.GetOrigin()).X * mSpeed;
                    mVelocity.Y = -mJumpPower * 0.8f;

                    mMoveState = MoveState.Damage;
                }
            }
        }
    }
}
