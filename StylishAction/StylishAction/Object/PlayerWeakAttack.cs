using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StylishAction.Device;

namespace StylishAction.Object
{
    class PlayerWeakAttack : Object
    {
        private int mTimer;
        private Player mPlayer;

        public PlayerWeakAttack(string name, Vector2 size, Vector2 origin, int timer) : base(name, size)
        {
            SetOrigin(origin);
            mTimer = timer;
            
        }

        public override void Collision(Object other)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            mTimer--;
            if (mTimer <= 0)
            {
                mIsDead = true;
            }
        }
    }
}
