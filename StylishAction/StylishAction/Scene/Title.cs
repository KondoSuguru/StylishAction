using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StylishAction.Utility;

namespace StylishAction.Scene
{
    class Title : SceneBase, IScene
    {
        public Title()
        {

        }

        public void Initialize()
        {
            mIsEnd = false;
            mNextScene = Scene.GamePlay;
        }

        public bool IsEnd()
        {
            return mIsEnd;
        }

        public Scene Next()
        {
            return mNextScene;
        }

        public void Shutdown()
        {
            
        }

        public void Update(float deltaTime)
        {
            if (Input.GetKeyTrigger(Keys.Enter))
            {
                mIsEnd = true;
            }
        }

        public void Draw()
        {
            
        }
    }
}
