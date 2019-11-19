using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace StylishAction.Utility
{
    class CountDownTimer : Timer
    {
        public CountDownTimer()
            : base()
        {
            Initialize();
        }

        public CountDownTimer(float second)
            : base(second)
        {
            Initialize();
        }

        public override void Initialize()
        {
            mCurrentTime = mLimitTime;
        }

        public override bool IsTime()
        {
            return mCurrentTime <= 0.0f;
        }

        public override float Rate()
        {
            return 1.0f - mCurrentTime / mLimitTime;
        }

        public override void Update(float deltaTime)
        {
            mCurrentTime -= deltaTime;
        }
    }
}
