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
        protected string mName;
        protected int mSize;
        protected Vector2 mPosition;
        protected Vector2 mOrigin;
        protected bool mIsDead;

        public Object(string name, int size)
        {
            mName = name;
            mSize = size;
            mPosition = Vector2.Zero;
            mOrigin = new Vector2(mPosition.X + size / 2, mPosition.Y + size / 2);
            ObjectManager.Instance().AddObject(this);
        }

        public virtual void Initialize()
        {
            mIsDead = false;
        }

        public virtual void Update(GameTime gameTime)
        {
            mOrigin = new Vector2(mPosition.X + mSize / 2, mPosition.Y + mSize / 2);
        }

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

        public void SetOrigin(Vector2 origin)
        {
            mOrigin = origin;
            mPosition = new Vector2(mOrigin.X - mSize / 2, mOrigin.Y - mSize / 2);
        }

        public Vector2 GetOrigin()
        {
            return mOrigin;
        }

        public int GetSize()
        {
            return mSize;
        }

        public bool IsDead()
        {
            return mIsDead;
        }

        public void Translate(Vector2 translation)
        {
            mPosition += translation;
        }

        public abstract void Collision(Object other);
    }
}
