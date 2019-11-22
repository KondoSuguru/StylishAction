using Microsoft.Xna.Framework;
using StylishAction.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace StylishAction.Effect
{
    sealed class HitStopEffect
    {
        private static HitStopEffect mInstance;

        private Blast mBlast;
        private readonly int ShakeStrenge = 1;

        private HitStopEffect()
        {

        }

        public static HitStopEffect Instance()
        {
            // インスタンスがまだ生成されていなければ生成する
            if (mInstance == null)
            {
                mInstance = new HitStopEffect();
            }
            return mInstance;
        }

        public void Update(GameTime gameTime)
        {
            if(mBlast == null)
            {
                return;
            }

            mBlast.Update(gameTime);
        }

        public void SetBlast(float mag, Vector2 center)
        {
            mBlast = new Blast(mag, center);
        }

        public void Draw()
        {
            if(mBlast == null)
            {
                return;
            }

            if(mBlast.Amount <= 0.0f)
            {
                mBlast = null;
                return;
            }

            for(int i = 0; i< ShakeStrenge; i++)
            {
                Vector2 origin = mBlast.Center / 2;

                Vector2 position = mBlast.Center - origin;

                float alpha = 0.35f * (mBlast.Amount / mBlast.Magnitude);
                Color color = new Color(1.0f, 1.0f, 1.0f, alpha);

                float scale = (1.0f + (mBlast.Magnitude - mBlast.Amount) * 0.1f +((float)(i + 1) / 40.0f));

                GameDevice.Instance().GetRenderer().DrawRenderTargetTexture(
                    position,
                    null,
                    0.0f,
                    origin,
                    scale,
                    color,
                    SpriteEffects.None,
                    1.0f);
            }
        }

    }
}
