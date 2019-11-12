using StylishAction.Device;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylishAction.Object
{
    sealed class ObjectManager
    {
        private static ObjectManager mInstance;
        private List<Object> mObjects;
        private List<Object> mAddObjects;

        private ObjectManager()
        {
            mObjects = new List<Object>();
            mAddObjects = new List<Object>();
        }

        public static ObjectManager Instance()
        {
            if (mInstance == null)
            {
                mInstance = new ObjectManager();
            }
            return mInstance;
        }

        public void Initialize()
        {
            mObjects.Clear();
            mAddObjects.Clear();
        }

        public void AddObject(Object obj)
        {
            if (obj == null)
                return;
            mAddObjects.Add(obj);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var o in mObjects)
            {
                o.Update(gameTime);
            }

            foreach(var addObj in mAddObjects)
            {
                addObj.Initialize();
                mObjects.Add(addObj);
            }
            mAddObjects.Clear();

            Collision();
            RemoveDeadObject();
        }

        public void Draw()
        {
            foreach(var o in mObjects)
            {
                o.Draw();
            }
        }

        public void Clear()
        {
            mObjects.Clear();
            mAddObjects.Clear();
        }

        public List<Object> GetObjects()
        {
            return mObjects;
        }

        private void Collision()
        {
            //総当たり判定
            for(int i = 0; i < mObjects.Count - 1; i++)
            {
                Object obj1 = mObjects[i];
                for (int j = i + 1; j < mObjects.Count; j++)
                {
                    Object obj2 = mObjects[j];
                    if(Vector2.Distance(obj1.GetPosition(),obj2.GetPosition()) <= (obj1.GetSize() + obj2.GetSize()) / 2)
                    {
                        obj1.Collision(obj2);
                        obj2.Collision(obj1);
                    }
                }
            }
        }

        private void RemoveDeadObject()
        {
            mObjects.RemoveAll(obj => obj.IsDead());
        }
    }
}
