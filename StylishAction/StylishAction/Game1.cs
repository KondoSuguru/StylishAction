using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StylishAction.Device;
using StylishAction.Scene;
using StylishAction.Utility;
using StylishAction.Effect;
using StylishAction.Object;

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
        private HPUI mHPUI;

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

            mHPUI = new HPUI();

            HitStop.mIsHitStop = false;

            GameDevice.Instance().GetRenderer().InitializeRenderTarget(Screen.WIDTH, Screen.HEIGHT);

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

            if (!HitStop.mIsHitStop)
            {
                HitStop.mHitStopScale -= (float)gameTime.ElapsedGameTime.TotalSeconds * 2f;
                if (HitStop.mHitStopScale <= 1)
                {
                    HitStop.mHitStopScale = 1;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GameDevice.Instance().GetRenderer().BeginRenderTarget();
         
            GraphicsDevice.Clear(Color.CornflowerBlue);

            mSceneManager.Draw();

            GameDevice.Instance().GetRenderer().EndRenderTarget();

            //GameDevice.Instance().GetRenderer().Begin(SpriteSortMode.Deferred, BlendState.Additive);
            GameDevice.Instance().GetRenderer().Begin();

            if (ObjectManager.Instance().GetPlayer() != null)
            {
                Vector2 origin = ObjectManager.Instance().GetPlayer().GetOrigin();
                GameDevice.Instance().GetRenderer().DrawRenderTargetTexture(origin, null, 0.0f, origin, HitStop.mHitStopScale, Color.White);
            }
            else
            {
                GameDevice.Instance().GetRenderer().DrawRenderTargetTexture(new Vector2(Screen.WIDTH / 2, Screen.HEIGHT / 2), null, 0.0f, new Vector2(Screen.WIDTH / 2, Screen.HEIGHT / 2), HitStop.mHitStopScale, Color.White);
            }

            mHPUI.Draw();

            GameDevice.Instance().GetRenderer().End();

            base.Draw(gameTime);
        }
    }
}
