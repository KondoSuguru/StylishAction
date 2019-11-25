using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StylishAction.Device;
using StylishAction.Utility;

namespace StylishAction.Scene
{
    class Title : SceneBase, IScene
    {

        public Title()
        {
            Renderer r = GameDevice.Instance().GetRenderer();
            r.LoadContent("bg");
            r.LoadContent("title");
            Sound s = GameDevice.Instance().GetSound();
            s.LoadBGM("titleBGM");
            s.LoadSE("selectSE");
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
            var s = GameDevice.Instance().GetSound();
            s.PlayBGM("titleBGM");

            if (Input.GetKeyTrigger(Keys.Enter))
            {
                s.StopBGM();
                s.PlaySE("selectSE");
                mIsEnd = true;
            }
        }

        public void Draw()
        {
            Renderer r = GameDevice.Instance().GetRenderer();
            r.DrawTexture("bg", Vector2.Zero);
            r.DrawTexture("title", Vector2.Zero);
        }
    }
}
