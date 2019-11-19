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
        private CountDownTimer mTimer;

        public PlayerWeakAttack(string name, Vector2 size, Vector2 origin, CountDownTimer timer) : base(name, size)
        {
            SetOrigin(origin);
            mTimer = timer;
        }

        public override void Collision(Object other)
        {

        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

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
