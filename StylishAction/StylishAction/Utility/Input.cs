using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylishAction.Utility
{
    static class Input
    {
        // 移動量
        private static Vector2 velocity = Vector2.Zero;
        // キーボード
        private static KeyboardState mCurrentKey; // 現在のキーの状態
        private static KeyboardState mPreviousKey; // 1フレーム前のキーの状態
        private static Keys mBufferKey; //バッファしているキー
        // マウス
        private static MouseState mCurrentMouse; // 現在のマウスの状態
        private static MouseState mPreviousMouse; // 1フレーム前のマウスの状態

        private static bool mIsBuffer = false; //バッファを保持するかどうか
        private static bool mIsSetBuffer = false; //バッファをセットするかどうか

        public static void Update()
        {
            // キーボード
            mPreviousKey = mCurrentKey;
            mCurrentKey = Keyboard.GetState();

            // マウス
            mPreviousMouse = mCurrentMouse;
            mCurrentMouse = Mouse.GetState();

            // 更新
            UpdateVelocity();
        }

        // キーボード関連
        public static Vector2 Velocity()
        {
            return velocity;
        }

        private static void UpdateVelocity()
        {
            // 毎ループ初期化
            velocity = Vector2.Zero;

            // 右
            if (mCurrentKey.IsKeyDown(Keys.Right) || (mCurrentKey.IsKeyDown(Keys.D)))
            {
                velocity.X += 1.0f;
            }

            // 左
            if (mCurrentKey.IsKeyDown(Keys.Left) || (mCurrentKey.IsKeyDown(Keys.A)))
            {
                velocity.X -= 1.0f;
            }

            // 上
            if (mCurrentKey.IsKeyDown(Keys.Up) || (mCurrentKey.IsKeyDown(Keys.W)))
            {
                velocity.Y -= 1.0f;
            }

            // 下
            if (mCurrentKey.IsKeyDown(Keys.Down) || (mCurrentKey.IsKeyDown(Keys.S)))
            {
                velocity.Y += 1.0f;
            }

            // 正規化
            if (velocity.Length() != 0)
            {
                velocity.Normalize();
            }
        }

        /// <summary>
        /// キーが押された瞬間か？
        /// </summary>
        /// <param name="key">チェックしたいキー</param>
        /// <returns>現在キーが押されていて、1フレーム前に押されていなければtrue</returns>
        public static bool IsKeyDown(Keys key)
        {
            if(mIsSetBuffer && key == mBufferKey) //バッファセット状態で、バッファを取ったキーと判定したいキーが同じなら
            {
                mIsSetBuffer = false; //バッファセット状態を解除
                mBufferKey = Keys.End; //まず使わないであろうキーをバッファキーにセット
                return true; //押したということにする。
            }
            return mCurrentKey.IsKeyDown(key) && !mPreviousKey.IsKeyDown(key);
        }

        /// <summary>
        /// キーが押された瞬間か？
        /// </summary>
        /// <param name="key">チェックしたいキー</param>
        /// <returns>押された瞬間ならtrue</returns>
        public static bool GetKeyTrigger(Keys key)
        {
            return IsKeyDown(key);
        }

        /// <summary>
        /// キーが押されているか？
        /// </summary>
        /// <param name="key">調べたいキー</param>
        /// <returns>キーが押されていたらtrue</returns>
        public static bool GetKeyState(Keys key)
        {
            return mCurrentKey.IsKeyDown(key);
        }

        //キーが離された瞬間
        public static bool GetKeyUp(Keys key)
        {
            return !mCurrentKey.IsKeyDown(key) && mPreviousKey.IsKeyDown(key);
        }

        public static void SetBufferKey(Keys key)
        {
            //バッファ取りたいキーを入力したら
            if (GetKeyTrigger(key))
            {
                mIsBuffer = true; //バッファ保持状態にする
                mBufferKey = key;//入力状態をバッファしたキーをセット
            }
        }

        public static void BufferInput()
        {
            if (mIsBuffer)//バッファ保持状態なら
            {
                mIsSetBuffer = true; //バッファセット状態にする
                mIsBuffer = false; //バッファ保持状態解除
            }
        }

        // マウス関連
        /// <summary>
        /// マウスの左ボタンが押された瞬間か？
        /// </summary>
        /// <returns>現在押されていて、1フレーム前に押されていなければtrue</returns>
        public static bool IsMouseLButtonDown()
        {
            return mCurrentMouse.LeftButton == ButtonState.Pressed && mPreviousMouse.LeftButton == ButtonState.Released;
        }

        /// <summary>
        /// マウスの左ボタンが離された瞬間か？
        /// </summary>
        /// <returns>現在はなされていて、1フレーム前に押されていたらtrue</returns>
        public static bool IsMouseLButtonUp()
        {
            return mCurrentMouse.LeftButton == ButtonState.Released && mPreviousMouse.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// マウスの左ボタンが押されているか？
        /// </summary>
        /// <returns>左ボタンが押されていたらtrue</returns>
        public static bool IsMouseLButton()
        {
            return mCurrentMouse.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// マウスの右ボタンが押された瞬間か？
        /// </summary>
        /// <returns>現在押されていて、1フレーム前に押されていなければtrue</returns>
        public static bool IsMouseRButtonDown()
        {
            return mCurrentMouse.LeftButton == ButtonState.Pressed && mPreviousMouse.LeftButton == ButtonState.Released;
        }

        /// <summary>
        /// マウスの右ボタンが離された瞬間か？
        /// </summary>
        /// <returns>現在はなされていて、1フレーム前に押されていたらtrue</returns>
        public static bool IsMouseRButtonUp()
        {
            return mCurrentMouse.LeftButton == ButtonState.Released && mPreviousMouse.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// マウスのボタンが押されているか？
        /// </summary>
        /// <returns>右ボタンが押されていたらtrue</returns>
        public static bool IsMouseRButton()
        {
            return mCurrentMouse.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// マウスの位置を返す
        /// </summary>
        public static Vector2 MousePosition
        {
            // プロパティでGetterを生成
            get
            {
                return new Vector2(mCurrentMouse.X, mCurrentMouse.Y);
            }
        }

        /// <summary>
        /// マウスのスクロールホイールの変化量
        /// </summary>
        /// <returns>1フレーム前と現在のホイール量の差分</returns>
        public static int GetMouseWheel()
        {
            return mPreviousMouse.ScrollWheelValue - mCurrentMouse.ScrollWheelValue;
        }
    }
}
