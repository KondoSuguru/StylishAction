using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylishAction.Effect
{
    class Blast
    {
        private float mAmount;
        private float mMagnitude;
        private Vector2 mCenter;

        public float Amount { get => mAmount; }
        public float Magnitude { get => mMagnitude; }
        public Vector2 Center { get => mCenter; }

        public Blast(float magnitude, Vector2 center)
        {
            mAmount = magnitude;
            mMagnitude = magnitude;
            mCenter = center;
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            mAmount = (mAmount >= 0.0f) ? (mAmount - delta) : (0.0f);
        }
    }
}
