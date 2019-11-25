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
        private Player mPlayer;
        private Enemy mBoss;
        private bool mIsClear;
        private bool mIsGameOver;

        public GamePlay()
        {
            var r = GameDevice.Instance().GetRenderer();
            r.LoadContent("player_left");
            r.LoadContent("player_right");
            r.LoadContent("player_up");
            r.LoadContent("player_down");
            r.LoadContent("sikaku");
            r.LoadContent("boss");
            r.LoadContent("boss_left");
            r.LoadContent("boss_right");
            r.LoadContent("wall");
            r.LoadContent("ground");
            r.LoadContent("bg");
            r.LoadContent("HP_frame");
            r.LoadContent("HP_player");
            r.LoadContent("HP_boss");
            r.LoadContent("clear");
            r.LoadContent("gameover");
            var s = GameDevice.Instance().GetSound();
            s.LoadBGM("playBGM");
            s.LoadSE("shotSE");
            s.LoadSE("damageSE");
            s.LoadSE("loseSE");
            s.LoadSE("winSE");
        }

        public void Initialize()
        {
            mIsEnd = false;
            mIsClear = false;
            mIsGameOver = false;
            mNextScene = Scene.Title;
            ObjectManager.Instance().Initialize();

            mPlayer = new Player("player_right", new Vector2(32, 32));
            mBoss = new Enemy("boss_left", new Vector2(64, 64));
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
            var s = GameDevice.Instance().GetSound();
            if (mIsClear || mIsGameOver)
            {
                if (Input.GetKeyTrigger(Keys.Enter))
                {
                    s.PlaySE("selectSE");
                    mIsEnd = true;
                }
                return;
            }

            
            s.PlayBGM("playBGM");
            ObjectManager.Instance().Update(deltaTime);
            if (mPlayer.IsDead())
            {
                s.StopBGM();
                s.PlaySE("loseSE");
                mIsGameOver = true;
            }
            if (mBoss.IsDead())
            {
                s.StopBGM();
                s.PlaySE("winSE");
                mIsClear = true;
            }

        }
        public void Draw()
        {
            GameDevice.Instance().GetRenderer().DrawTexture("bg", Vector2.Zero);
            ObjectManager.Instance().Draw();
            if (mIsClear)
            {
                GameDevice.Instance().GetRenderer().DrawTexture("clear", Vector2.Zero);
            }
            if (mIsGameOver)
            {
                GameDevice.Instance().GetRenderer().DrawTexture("gameover", Vector2.Zero);
            }
        }
    }
}
