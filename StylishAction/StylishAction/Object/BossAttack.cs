using Microsoft.Xna.Framework;
using StylishAction.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylishAction.Object
{
    class BossAttack : Object
    {
        private Vector2 mVelocity;
        private float mSpeed;
        private CountDownTimer mTimer;

        public BossAttack(string name, Vector2 size, Vector2 origin, Vector2 velocity) : base(name, size)
        {
            SetOrigin( origin);
            mVelocity = Vector2.Normalize(velocity);
            mSpeed = 800;
            mTimer = new CountDownTimer(0.8f);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            Translate(mVelocity * mSpeed * deltaTime);
            mAlpha -= deltaTime ;

            mTimer.Update(deltaTime);
            if (mTimer.IsTime())
            {
                mIsDead = true;
            }
        }

        public override void Collision(Object other)
        {
            
        }
    }
}
