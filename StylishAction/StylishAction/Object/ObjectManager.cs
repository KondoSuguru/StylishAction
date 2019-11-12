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

        private ObjectManager()
        {
            mObjects = new List<Object>();
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
            foreach(var o in mObjects)
            {
                o.Initialize();
            }
        }

        public void AddObject(Object obj)
        {
            if (obj == null)
                return;
            mObjects.Add(obj);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var o in mObjects)
            {
                o.Update(gameTime);
            }
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
        }

        public List<Object> GetObjects()
        {
            return mObjects;
        }
    }
}
