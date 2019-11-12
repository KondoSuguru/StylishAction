using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StylishAction.Device;
using StylishAction.Object;
using StylishAction.Utility;

namespace StylishAction.Scene
{
    class GamePlay : SceneBase, IScene
    {
        public GamePlay()
        {
            var r = GameDevice.Instance().GetRenderer();
            r.LoadContent("player");
            r.LoadContent("enemy");
        }

        public void Initialize()
        {
            mIsEnd = false;
            mNextScene = Scene.Title;
            ObjectManager.Instance().Initialize();

            new Player("player", 32);
            new Enemy("enemy", 32);
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
            ObjectManager.Instance().Clear();
        }

        public void Update(GameTime gameTime)
        {
            ObjectManager.Instance().Update(gameTime);

            if (Input.GetKeyTrigger(Keys.Enter))
            {
                mIsEnd = true;
            }
        }
        public void Draw()
        {
            ObjectManager.Instance().Draw();
        }
    }
}
