using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylishAction.Device;
using Microsoft.Xna.Framework;

namespace StylishAction.Scene
{
    class SceneManager
    {
        // シーン管理用ディクショナリ
        private Dictionary<Scene, IScene> mScenes = new Dictionary<Scene, IScene>();
        // 現在のシーン
        private IScene mCurrentScene = null;

        public SceneManager()
        {
        }

        public void Add(Scene name, IScene scene)
        {
            if (mScenes.ContainsKey(name))
            {
                return;
            }
            mScenes.Add(name, scene);
        }

        public void Change(Scene name)
        {
            if (mCurrentScene != null)
            {
                mCurrentScene.Shutdown();
            }

            mCurrentScene = mScenes[name];

            mCurrentScene.Initialize();
        }

        public void Update(GameTime gameTime)
        {
            if (mCurrentScene == null)
            {
                return;
            }

            mCurrentScene.Update(gameTime);

            if (mCurrentScene.IsEnd())
            {
                Change(mCurrentScene.Next());
            }
        }

        public void Draw()
        {
            if (mCurrentScene == null)
            {
                return;
            }
            mCurrentScene.Draw();
        }
    }
}
