﻿using System;
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
        private readonly int mMaxHitPoint;

        private enum Direction
        {
            Up,
            Down,
            Left,
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
            mJumpPower = 20;
            mGravity = 1;
            mMaxHitPoint = 3;
            mCurrentDir = Direction.Right;
            mPreviousDir = mCurrentDir;
            mAttackState = AttackState.None;
        }

        public override void Initialize()
        {
            base.Initialize();
            mPosition = Vector2.Zero;
            mVelocity = Vector2.Zero;
            mJumpCount = 0;
            mHitPoint = mMaxHitPoint;
            mWeakAttackTimer = 5;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Move();
            Fall();
            Jump();
            WeakAttack();
            DirectionUpdate();
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
            if (mPosition.Y < Screen.HEIGHT - 64)
            {
                mVelocity.Y += mGravity;
                mVelocity.Y = ((mVelocity.Y >= 100) ? 100 : mVelocity.Y);
            }
            else
            {
                mPosition.Y = Screen.HEIGHT - 64;
                mVelocity.Y = 0;
                mJumpCount = 2;
            }
        }

        private void WeakAttack()
        {
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
            if (Input.GetKeyTrigger(Keys.Right))
            {
                mCurrentDir = Direction.Right;
                mPreviousDir = mCurrentDir;
            }
            if (Input.GetKeyTrigger(Keys.Left))
            {
                mCurrentDir = Direction.Left;
                mPreviousDir = mCurrentDir;
            }

            if (Input.GetKeyTrigger(Keys.Up))
            {
                mCurrentDir = Direction.Up;
            }
            if(Input.GetKeyUp(Keys.Up))
            {
                mCurrentDir = mPreviousDir;
            }

            if (Input.GetKeyTrigger(Keys.Down))
            {
                mCurrentDir = Direction.Down;
            }
            if(Input.GetKeyUp(Keys.Down))
            {
                mCurrentDir = mPreviousDir;
            }
        }

        public Vector2 GetVelocity()
        {
            return mVelocity;
        }

        public override void Collision(Object other)
        {
            if (other is Enemy)
            {
                mHitPoint--;
            }
        }
    }
}
