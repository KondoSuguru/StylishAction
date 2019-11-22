using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylishAction.Utility;

namespace StylishAction.Object
{
    class PlayerDashEffect : Object
    {
        private CountDownTimer mDeleteTimer;

        public PlayerDashEffect(string name, Vector2 size, Vector2 position) : base(name, size)
        {
            mPosition = position;
            mDeleteTimer = new CountDownTimer(1f);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            mDeleteTimer.Update(deltaTime);
            //SetAlpha(mDeleteTimer.Rate());
            if (mDeleteTimer.IsTime())
            {
                mIsDead = true;
            }
        }

        public override void Collision(Object other)
        {
            
        }
    }
}
