using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylishAction.Object
{
    class DamageEffect : Object
    {
        private Vector2 mVelocity;
        private float mSpeed;

        public DamageEffect(string name, Vector2 size, Vector2 origin, Vector2 velocity) :base(name, size)
        {
            SetOrigin(origin);
            mVelocity = Vector2.Normalize(velocity);
            mSpeed = 300;
            SetAlpha(0.8f);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            mAlpha -= deltaTime;

            Translate(mVelocity * mSpeed * deltaTime);

            if(mAlpha <= 0)
            {
                mIsDead = true;
            }
        }

        public override void Collision(Object other)
        {
            
        }
    }
}
