using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StylishAction.Device;
using StylishAction.Scene;
using StylishAction.Utility;

namespace StylishAction
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager mGraphics;
        SpriteBatch mSpriteBatch;
        private SceneManager mSceneManager;

        public Game1()
        {
            mGraphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            mGraphics.PreferredBackBufferWidth = Screen.WIDTH;
            mGraphics.PreferredBackBufferHeight = Screen.HEIGHT;
        }

        protected override void Initialize()
        {
            GameDevice.Instance(Content, GraphicsDevice);

            mSceneManager = new SceneManager();
            mSceneManager.Add(Scene.Scene.Title, new Title());
            mSceneManager.Add(Scene.Scene.GamePlay, new GamePlay());
            mSceneManager.Change(Scene.Scene.Title);

            HitStop.mIsHitStop = false;

            base.Initialize();
        }


        protected override void LoadContent()
        {
            mSpriteBatch = new SpriteBatch(GraphicsDevice);
        }


        protected override void UnloadContent()
        {
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GameDevice.Instance().Update(gameTime);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;//1フレーム進むのにかかった時間(秒)

            if (HitStop.mIsHitStop) 
            {
                deltaTime = 0;//ヒットストップ時はdeltaTimeを0にしてtimerや移動などを止める
                HitStop.mHitStopTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(HitStop.mHitStopTime <= 0)
                {
                    HitStop.mIsHitStop = false;
                }
            }

            mSceneManager.Update(deltaTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GameDevice.Instance().GetRenderer().Begin();

            // 画面クリア時の色を設定
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (HitStop.mIsHitStop)
            {
                GraphicsDevice.Clear(Color.Green); //ヒットストップ時
            }

            mSceneManager.Draw();

            GameDevice.Instance().GetRenderer().End();

            base.Draw(gameTime);
        }
    }
}
