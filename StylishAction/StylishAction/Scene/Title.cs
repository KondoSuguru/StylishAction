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
        }

        public bool IsEnd()
        {
            return mIsEnd;
        }

        public Scene Next()
        {
            return Scene.GamePlay;
        }

        public void Shutdown()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            if (Input.GetKeyTrigger(Keys.Z))
            {
                mIsEnd = true;
            }
        }

        public void Draw()
        {
            
        }
    }
}
