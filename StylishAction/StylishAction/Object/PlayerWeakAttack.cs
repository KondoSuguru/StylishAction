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
    class PlayerWeakAttack : Object
    {
        private Vector2 mVelocity;
        private float mSpeed;
        private CountDownTimer mTimer;

        public PlayerWeakAttack(string name, Vector2 size, Vector2 origin, int dir, CountDownTimer timer) : base(name, size)
        {
            if(dir % 2 == 0)
            {
                mVelocity = new Vector2(0, dir - 1);
            }
            else
            {
                mVelocity = new Vector2(dir - 2, 0);
            }
            SetOrigin(origin + (mVelocity * size));
            mSpeed = 100;
            mTimer = timer;
        }

        public override void Collision(Object other)
        {

        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            Translate(mVelocity * mSpeed * deltaTime);

            mTimer.Update(deltaTime);
            if (mTimer.IsTime())
            {
                mIsDead = true;
            }
        }

        public float GetLimitTime()
        {
            return mTimer.GetLimitTime();
        }
    }
}
