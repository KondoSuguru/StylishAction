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
        private Vector2 mVelocity;
        private readonly float mGravity;
        private readonly float mSpeed;
        private readonly float mDashSpeed;
        private int mDashTimer;
        private int mDashCount;
        private bool mIsDash;
        private readonly float mJumpPower;
        private int mJumpCount;
        private readonly int mMaxHitPoint;
        private int mHitPoint;

        private bool mIsInvisible;
        private int mInvisibleTimer;

        private enum MoveState
        {
            OnGround,
            JumpUp,
            JumpDown,
        }
        private MoveState mMoveState;

        private enum Direction
        {
            Up,
            Left,
            Down,
            Right,
        }
        private Direction mCurrentDir; //今のDirection
        private Direction mPreviousDir;//前のDirection

        private enum AttackState
        {
            WeakAttack,
            None,
        }
        private AttackState mAttackState;
        private int mWeakAttackTimer;

        public Player(string name, int size) : base(name, size)
        {
            mSpeed = 5;
            mDashSpeed = 50;
            mJumpPower = 20;
            mGravity = 1;
            mMaxHitPoint = 3;
        }

        public override void Initialize()
        {
            base.Initialize();
            mPosition = Vector2.Zero;
            mVelocity = Vector2.Zero;
            mDashTimer = 0;
            mDashCount = 2;
            mIsDash = false;
            mJumpCount = 0;
            mHitPoint = mMaxHitPoint;
            mIsInvisible = false;
            mInvisibleTimer = 0;
            mWeakAttackTimer = 5;
            mMoveState = MoveState.OnGround;
            mCurrentDir = Direction.Right;
            mPreviousDir = mCurrentDir;
            mAttackState = AttackState.None;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            DirectionUpdate();
            InvisibleUpdate();

            Move();
            Dash();
            Fall();
            Jump();
            WeakAttack();
            if (mAttackState == AttackState.None)
            {
                Translate(mVelocity);
            }

            if (mHitPoint <= 0)
            {
                mIsDead = true;
            }
        }

        private void Move()
        {
            if (mIsDash)
                return;
            mVelocity.X = Input.Velocity().X * mSpeed;
        }

        private void Jump()
        {
            if (mIsDash)
                return;
            if (Input.GetKeyTrigger(Keys.Space) && mJumpCount > 0)
            {
                mMoveState = MoveState.JumpUp;
                mVelocity.Y = -mJumpPower;
                mJumpCount--;

            }
        }

        private void Dash()
        {
            if (!mIsDash && Input.GetKeyTrigger(Keys.X) && mDashCount > 0)
            {
                mIsDash = true;
                mDashCount--;
            }
            if (mIsDash)
            {
                mDashTimer++;
                mVelocity.Y = 0;
                mVelocity.X = ((int)mPreviousDir - 2) * mDashSpeed;
                if(mDashTimer > 5)
                {
                    mIsDash = false;
                    mDashTimer = 0;
                }
            }
        }

        private void Fall()
        {
            if (mIsDash)
                return;

            if (mPosition.Y < Screen.HEIGHT - 64)
            {
                mVelocity.Y += mGravity;
                mVelocity.Y = ((mVelocity.Y >= 100) ? 100 : mVelocity.Y);
                if(mVelocity.Y > 0)
                {
                    mMoveState = MoveState.JumpDown;
                }
            }
            else
            {
                mMoveState = MoveState.OnGround;
                mPosition.Y = Screen.HEIGHT - 64;
                mVelocity.Y = 0;
                mJumpCount = 2;
                mDashCount = 2;
            }
        }

        private void WeakAttack()
        {
            if (mIsDash)
                return;

            if (Input.GetKeyTrigger(Keys.Z) && mAttackState == AttackState.None)
            {
                Vector2 attackOrigin = Vector2.Zero;
                switch (mCurrentDir)
                {
                    case Direction.Up:
                        attackOrigin = new Vector2(mOrigin.X, mOrigin.Y - 32);
                        break;
                    case Direction.Down:
                        attackOrigin = new Vector2(mOrigin.X, mOrigin.Y + 32);
                        break;
                    case Direction.Right:
                        attackOrigin = new Vector2(mOrigin.X + 32, mOrigin.Y);
                        break;
                    case Direction.Left:
                        attackOrigin = new Vector2(mOrigin.X - 32, mOrigin.Y);
                        break;
                }
                new PlayerWeakAttack("player", 32, attackOrigin, mWeakAttackTimer);
                mAttackState = AttackState.WeakAttack;
            }

            if(mAttackState == AttackState.WeakAttack)
            {
                mWeakAttackTimer--;
                if(mWeakAttackTimer <= 0)
                {
                    mAttackState = AttackState.None;
                    mWeakAttackTimer = 5;
                    mVelocity = Vector2.Zero;
                }
            }
        }

        private void DirectionUpdate()
        {
            if (mIsDash)
                return;
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

        private void InvisibleUpdate()
        {
            if (mIsInvisible)
            {
                mInvisibleTimer++;
                if(mInvisibleTimer >= 60)
                {
                    mIsInvisible = false;
                    mInvisibleTimer = 0;
                }
            }
        }

        public Vector2 GetVelocity()
        {
            return mVelocity;
        }

        public override void Draw()
        {
            if (mIsInvisible)
            {
                GameDevice.Instance().GetRenderer().DrawTexture(mName, mPosition, 0.5f);
            }
            else
            {
                base.Draw();
            }
        }

        public override void Collision(Object other)
        {
            if (other is Enemy)
            {
                if (!mIsDash && !mIsInvisible)
                {
                    mIsInvisible = true;
                    mHitPoint--;
                }
            }
        }
    }
}
