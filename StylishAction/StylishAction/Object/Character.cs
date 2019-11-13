using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylishAction.Object
{
    public abstract class Character : Object
    {
        protected Vector2 mVelocity;
        protected float mGravity;
        protected float mSpeed;
        protected int mMaxHitPoint;
        protected int mHitPoint;

        protected enum Direction
        {
            Up,
            Left,
            Down,
            Right,
        }
        protected Direction mCurrentDir; //今のDirection
        protected Direction mPreviousDir;//前のDirection


        public Character(string name, Vector2 size) :base(name, size)
        {

        }
        public override void Initialize()
        {
            base.Initialize();
            mVelocity = Vector2.Zero;
            mHitPoint = mMaxHitPoint;
            mCurrentDir = Direction.Right;
            mPreviousDir = mCurrentDir;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (mHitPoint <= 0)
            {
                Dead();
            }
        }

        protected virtual void Dead()
        {
            //死ぬときの処理
            mIsDead = true;
        }

        public override void Collision(Object other)
        {
            
        }
    }
}
