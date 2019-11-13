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
        protected Vector2 mSize;
        protected Vector2 mPosition;
        protected Vector2 mOrigin;//中心点
        protected Color mColor;
        protected float mAlpha;
        protected bool mIsDead;

        public Object(string name, Vector2 size)
        {
            mName = name;
            mSize = size;
            mPosition = Vector2.Zero;
            mOrigin = new Vector2(mPosition.X + (mSize.X / 2), mPosition.Y + (mSize.Y / 2));
            mColor = new Color(255, 255, 255);
            mAlpha = 1.0f;
            ObjectManager.Instance().AddObject(this);
        }

        public virtual void Initialize()
        {
            mIsDead = false;
        }

        public virtual void Update(GameTime gameTime)
        {
            mOrigin = new Vector2(mPosition.X + (mSize.X / 2), mPosition.Y + (mSize.Y / 2));
        }

        public virtual void Draw()
        {
            GameDevice.Instance().GetRenderer().DrawTexture(mName, mPosition, mColor, mAlpha);
        }

        public Vector2 GetSize()
        {
            return mSize;
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
            mPosition = new Vector2(mOrigin.X - (mSize.X / 2), mOrigin.Y - (mSize.Y / 2));
        }

        public Vector2 GetOrigin()
        {
            return mOrigin;
        }

        public void SetColor(Color color)
        {
            mColor = color;
        }

        public Color GetColor()
        {
            return mColor;
        }

        public void SetAlpha(float alpha)
        {
            mAlpha = alpha;
        }

        public float GetAlpha()
        {
            return mAlpha;
        }

        public bool IsDead()
        {
            return mIsDead;
        }

        public virtual void Translate(Vector2 translation)
        {
            mPosition += translation;
        }

        public abstract void Collision(Object other);
    }
}
