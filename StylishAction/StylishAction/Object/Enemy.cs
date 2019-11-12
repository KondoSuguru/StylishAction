﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StylishAction.Device;

namespace StylishAction.Object
{
    class Enemy : Object
    {
        private readonly float mGravity;
        private Vector2 mVelocity;

        public Enemy(string name, int size) : base(name, size)
        {
            mGravity = 1;
        }

        public override void Initialize()
        {
            base.Initialize();
            mPosition = new Vector2(500,500);
            mVelocity = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        private void Fall()
        {
            if (mPosition.Y < Screen.HEIGHT - 32)
            {
                mVelocity.Y += mGravity;
                mVelocity.Y = ((mVelocity.Y >= 100) ? 100 : mVelocity.Y);
            }
            else
            {
                mPosition.Y = Screen.HEIGHT - 32;
                mVelocity.Y = 0;
            }
        }

        public override void Collision(Object other)
        {

        }
    }
}
