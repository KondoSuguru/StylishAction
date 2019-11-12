using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylishAction.Utility;

namespace StylishAction.Device
{
    sealed class GameDevice
    {
        // 唯一のインスタンス
        private static GameDevice mInstance;

        // デバイス関連のフィールド
        private Renderer mRenderer;
        private static Random mRandom;
        private ContentManager mContent;
        private GraphicsDevice mGraphics;
        private GameTime mGameTime;

        private GameDevice(ContentManager content, GraphicsDevice graphics)
        {
            mRenderer = new Renderer(content, graphics);
            mRandom = new Random();
            this.mContent = content;
            this.mGraphics = graphics;
        }

        public static GameDevice Instance(ContentManager content, GraphicsDevice graphics)
        {
            // インスタンスがまだ生成されていなければ生成する
            if (mInstance == null)
            {
                mInstance = new GameDevice(content, graphics);
            }
            return mInstance;
        }

        public static GameDevice Instance()
        {
            // まだインスタンスが生成されていなければエラー分を出す
            Debug.Assert(mInstance != null,
                "Game1クラスのInitializeメソッド内で引数付きInstanceメソッドを呼んでくる");

            return mInstance;
        }

        public void Initialize() { }

        public void Update(GameTime gameTime)
        {
            // デバイスで絶対に1回のみ更新が必要なもの
            Input.Update();
            this.mGameTime = gameTime;
        }

        public Renderer GetRenderer()
        {
            return mRenderer;
        }

        public Random GetRandom()
        {
            return mRandom;
        }

        public ContentManager GetContentManager()
        {
            return mContent;
        }

        public GraphicsDevice GetGraphicsDevice()
        {
            return mGraphics;
        }

        public GameTime GetGameTime()
        {
            return mGameTime;
        }
    }
}
