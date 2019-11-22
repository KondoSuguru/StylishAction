﻿using System;
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
            r.LoadContent("wall");
            r.LoadContent("ground");
        }

        public void Initialize()
        {
            mIsEnd = false;
            mNextScene = Scene.Title;
            ObjectManager.Instance().Initialize();

            new Player("enemy", new Vector2(32, 32));
            new Enemy("enemy", new Vector2(32, 32), 100);
            new Enemy("enemy", new Vector2(32, 32), 0);
            new Wall("ground", new Vector2(640, 64), new Vector2(0, - 64));
            new Wall("ground", new Vector2(640, 64), new Vector2(640, - 64));
            new Wall("ground", new Vector2(640, 64), new Vector2(1280, - 64));
            new Wall("wall", new Vector2(64, 640), new Vector2(0, 0));
            new Wall("wall", new Vector2(64, 640), new Vector2(0, 640));
            new Wall("wall", new Vector2(64, 640), new Vector2(Screen.WIDTH - 64, 0));
            new Wall("wall", new Vector2(64, 640), new Vector2(Screen.WIDTH - 64, 640));
            new Wall("ground", new Vector2(640, 64), new Vector2(0, Screen.HEIGHT - 64));
            new Wall("ground", new Vector2(640, 64), new Vector2(640, Screen.HEIGHT - 64));
            new Wall("ground", new Vector2(640, 64), new Vector2(1280, Screen.HEIGHT - 64));
            new Wall("ground", new Vector2(640, 64), new Vector2(Screen.WIDTH / 2 - 320, Screen.HEIGHT / 2));
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

        public void Update(float deltaTime)
        {
            ObjectManager.Instance().Update(deltaTime);

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
