using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StylishAction.Device;
using StylishAction.Object;

namespace StylishAction.Scene
{
    class GamePlay : SceneBase, IScene
    {
        public GamePlay()
        {
            var r = GameDevice.Instance().GetRenderer();
            r.LoadContent("Player1");

            ObjectManager.Instance().AddObject(new Player("Player1"));
        }

        public void Initialize()
        {
            mIsEnd = false;
            ObjectManager.Instance().Initialize();
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
            ObjectManager.Instance().Clear();
        }

        public void Update(GameTime gameTime)
        {
            ObjectManager.Instance().Update(gameTime);
        }
        public void Draw()
        {
            ObjectManager.Instance().Draw();
        }
    }
}
