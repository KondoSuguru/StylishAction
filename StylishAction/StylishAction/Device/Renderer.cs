using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;//Assert用

namespace StylishAction.Device
{
    class Renderer
    {
        private ContentManager mContentManager; //コンテンツ管理者
        private GraphicsDevice mGraphicsDevice; //グラフィック機器
        private SpriteBatch mSpriteBatch; //スプライト一括描画用オブジェクト
        private RenderTarget2D mTarget2D; // 2D用レンダーターゲット

        //複数画像管理用変数の宣言と生成
        private Dictionary<string, Texture2D> mTextures = new Dictionary<string, Texture2D>();

        public Renderer(ContentManager content, GraphicsDevice graphics)
        {
            mContentManager = content;
            mGraphicsDevice = graphics;
            mSpriteBatch = new SpriteBatch(mGraphicsDevice);
        }

        public void LoadContent(string assetName, string filepath = "./Textures/")
        {
            //すでにキー（assetName：アセット名）が登録されているとき
            if (mTextures.ContainsKey(assetName))
            {
#if DEBUG //DEBUGモードの時のみ下記エラー分をコンソールへ表示
                Console.WriteLine(assetName + "はすでに読み込まれています。\n プログラムを確認してください。");
#endif

                //それ以上読み込まないのでここで終了
                return;
            }
            //画像の読み込みとDictionaryへアセット名と画像を登録
            mTextures.Add(assetName, mContentManager.Load<Texture2D>(filepath + assetName));

        }

        /// <summary>
        /// 画像の読み込み
        /// </summary>
        /// <param name="assetName">アセット名（ファイルの名前）</param>
        /// <param name="texture">2D画像オブジェクト</param>
        public void LoadContent(string assetName, Texture2D texture)
        {
            //すでにキー（assetName：アセット名）が登録されているとき
            if (mTextures.ContainsKey(assetName))
            {
#if DEBUG //DEBUGモードの時のみ下記エラー分をコンソールへ表示
                Console.WriteLine(assetName + "はすでに読み込まれています。\n プログラムを確認してください。");
#endif

                //それ以上読み込まないのでここで終了
                return;
            }
            //画像の読み込みとDictionaryへアセット名と画像を登録
            mTextures.Add(assetName, texture);

        }

        public void Unload()
        {
            mTextures.Clear();//Dictionaryの情報をクリア
        }

        public void Begin()
        {
            mSpriteBatch.Begin();
        }

        public void End()
        {
            mSpriteBatch.End();
        }

        public void DrawTexture(string assetName, Vector2 position, float alpha = 1.0f)
        {
            //デバッグモードの時のみ、画像描画前のアセット名チェック
            Debug.Assert(
                mTextures.ContainsKey(assetName),
                "描画時にアセット名の指定を間違えたか、画像の読み込み自体できていません");

            mSpriteBatch.Draw(mTextures[assetName], position, Color.White * alpha);
        }

        public void DrawTexture(string assetName, Vector2 position, Rectangle rect, float alpha = 1.0f)
        {
            //デバッグモードの時のみ、画像描画前のアセット名チェック
            Debug.Assert(
                mTextures.ContainsKey(assetName),
                "描画時にアセット名の指定を間違えたか、画像の読み込み自体できていません");

            mSpriteBatch.Draw(
                mTextures[assetName], //テクスチャ
                position,            //位置
                rect,                //指定範囲（矩形で指定：左上の座標、幅、高さ）
                Color.White * alpha);//透明値
        }

        public void DrawTexture(string assetName, Vector2 position, Color color, float alpha = 1.0f)
        {
            // デバッグモード時のみ、画像描画前のアセット名チェック
            Debug.Assert(
                mTextures.ContainsKey(assetName),
                "描画時にアセット名の指定を間違えたか、" +
                "画像の読み込み自体できていません");

            mSpriteBatch.Draw(mTextures[assetName], position, color * alpha);
        }

        public void DrawTexture(
            string assetName,
            Vector2 position,
            Rectangle? rect, // nullを受け入れられるよう「？」で
            float rotate,
            Vector2 rotatePosition,
            Vector2 scale,
            SpriteEffects effects = SpriteEffects.None,
            float depth = 0.0f,
            float alpha = 1.0f)
        {
            mSpriteBatch.Draw(
                mTextures[assetName], // テクスチャ
                position,            // 位置
                rect,                // 指定範囲（矩形で指定：左上の座標、幅、高さ）
                Color.White * alpha, // 透明値
                rotate,              // 回転角度
                rotatePosition,      // 回転軸
                scale,               // 拡大縮小
                effects,             // 表示反転効果
                depth);              // スプライト深度
        }

        public void DrawNumber(
            string assetName,
            Vector2 position,
            int number,
            float alpha = 1.0f)
        {
            // デバッグモードの時のみ、画像描画前のアセット名チェック
            Debug.Assert(
                mTextures.ContainsKey(assetName),
                "描画時にアセット名の指定を間違えたか、" +
                "画像の読み込み自体できていません");

            // マイナスの数は0
            if (number < 0)
            {
                number = 0;
            }

            int width = 32; // 画像横幅

            // 数字を文字列化し、1文字ずつ取り出す
            foreach (var n in number.ToString())
            {
                // 数字のテクスチャが数字1つにつき幅32高さ64
                // 文字と文字を引き算し、整数値を取得している
                mSpriteBatch.Draw(
                    mTextures[assetName],
                    position,
                    new Rectangle((n - '0') * width, 0, width, 64),
                    Color.White * alpha);

                // 1文字描画したら1桁分右にずらす
                position.X += width;
            }
        }

        public void DrawNumberRightEdgeAlignment(
            string assetName,
            Vector2 position,
            int number,
            float alpha = 1f)
        {
            Debug.Assert(
                mTextures.ContainsKey(assetName),
                "描画時にアセット名の指定を間違えたか、" +
                "画像の読み込み自体できていません");

            // マイナスの数は0
            if (number < 0)
            {
                number = 0;
            }

            int width = 32; // 画像横幅

            //桁数計算(本当は=1)
            int digit = 0;
            for (int i = number; i >= 10; i /= 10)
            {
                digit++;
            }

            position.X -= width * digit;

            // 数字を文字列化し、1文字ずつ取り出す
            foreach (var n in number.ToString())
            {
                // 数字のテクスチャが数字1つにつき幅32高さ64
                // 文字と文字を引き算し、整数値を取得している
                mSpriteBatch.Draw(
                    mTextures[assetName],
                    position,
                    new Rectangle((n - '0') * width, 0, width, 64),
                    Color.White * alpha);

                // 1文字描画したら1桁分右にずらす
                position.X += width;
            }
        }

        public void DrawNumber(
            string assetName,
            Vector2 position,
            float number,
            float alpha = 1.0f)
        {
            // マイナスは0へ
            if (number < 0.0f)
            {
                number = 0.0f;
            }

            int width = 32; // 数字画像1つ分の横幅
                            // 小数部は2桁まで、整数部が1桁の時は0で埋める
            foreach (var n in number.ToString("00.00"))
            {
                // 少数の「.」か？
                if (n == '.')
                {
                    mSpriteBatch.Draw(
                    mTextures[assetName],
                    position,
                    new Rectangle(10 * width, 0, width, 64), // ピリオドは10番目
                    Color.White * alpha);
                }
                else
                {
                    // 数字の描画
                    mSpriteBatch.Draw(
                    mTextures[assetName],
                    position,
                    new Rectangle((n - '0') * width, 0, width, 64),
                    Color.White * alpha);
                }

                // 1文字描画したら1桁分右にずらす
                position.X += width;
            }
        }

        public void InitializeRenderTarget(int width, int height)
        {
            mTarget2D = new RenderTarget2D(mGraphicsDevice, width, height);
        }

        public void BeginRenderTarget()
        {
            // 描画するレンダーターゲットを設定
            mGraphicsDevice.SetRenderTarget(mTarget2D);
            // 描画先へのレンダーターゲットの内容を消す
            mGraphicsDevice.Clear(Color.Transparent);
            // レンダーターゲットへの描画開始
            mSpriteBatch.Begin();
        }

        public void EndRenderTarget()
        {
            // レンダーターゲットへの描画終了
            mSpriteBatch.End();
            // 描画を画面に変える
            mGraphicsDevice.SetRenderTarget(null);
        }

        public void DrawRenderTargetTexture(
            Vector2 position,
            Rectangle? rect, // nullを受け入れられるよう「？」で
            float rotate,
            Vector2 rotatePosition,
            float scale,
            Color color,
            SpriteEffects effects = SpriteEffects.None,
            float depth = 0.0f)
        {
            mSpriteBatch.Draw(
                mTarget2D,
                position,
                rect,
                color,
                rotate,
                rotatePosition,
                scale,
                effects,
                depth);
        }

        public void Begin(SpriteSortMode sortMode, BlendState blendState)
        {
            mSpriteBatch.Begin(sortMode, blendState);
        }
    }
}

