using Microsoft.Xna.Framework;
using StylishAction.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylishAction.Object
{
    public abstract class Object
    {
        protected Vector2 mPosition;
        protected string mName;
        protected bool mIsDead;

        public Object(string name)
        {
            mPosition = Vector2.Zero;
            mName = name;
        }

        public virtual void Initialize()
        {
            mIsDead = false;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw()
        {
            GameDevice.Instance().GetRenderer().DrawTexture(mName, mPosition);
        }

        public void SetPosition(Vector2 position)
        {
            mPosition = position;
        }

        public Vector2 GetPosition()
        {
            return mPosition;
        }

        public void Translate(Vector2 translation)
        {
            mPosition += translation;
        }
    }
}
