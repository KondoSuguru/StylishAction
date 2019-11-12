using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StylishAction.Device;
using StylishAction.Scene;

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
        }

        protected override void Initialize()
        {
            GameDevice.Instance(Content, GraphicsDevice);

            mSceneManager = new SceneManager();
            mSceneManager.Add(Scene.Scene.Title, new Title());
            mSceneManager.Add(Scene.Scene.GamePlay, new GamePlay());
            mSceneManager.Change(Scene.Scene.Title);

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
            mSceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GameDevice.Instance().GetRenderer().Begin();

            // 画面クリア時の色を設定
            GraphicsDevice.Clear(Color.CornflowerBlue);

            mSceneManager.Draw();

            GameDevice.Instance().GetRenderer().End();

            base.Draw(gameTime);
        }
    }
}
