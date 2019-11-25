using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylishAction.Device
{
    class Sound
    {
        #region フィールドとコンストラクタ
        //コンテンツ管理者
        private ContentManager contentManager;
        //MP3管理用
        private Dictionary<string, Song> bgms;
        //WAV管理用
        private Dictionary<string, SoundEffect> soundEffects;
        //WAVインスタンス管理用（WAVの高度な利用）
        private Dictionary<string, SoundEffectInstance> seInstances;
        //WAVインスタンスの再生管理用ディクショナリ
        private Dictionary<string, SoundEffectInstance> sePlayDict;
        //現在再生中のMP3アセット名
        private string currentBGM;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="content"></param>
        public Sound(ContentManager content)
        {
            //Game1クラスのコンテンツ管理者と紐づけ
            contentManager = content;
            //BGMは繰り返し再生
            MediaPlayer.IsRepeating = true;

            //各ディクショナリの実体生成
            bgms = new Dictionary<string, Song>();
            soundEffects = new Dictionary<string, SoundEffect>();
            seInstances = new Dictionary<string, SoundEffectInstance>();
            sePlayDict = new Dictionary<string, SoundEffectInstance>();

            currentBGM = null;
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Unload()
        {
            //ディクショナリをクリア
            bgms.Clear();
            soundEffects.Clear();
            seInstances.Clear();
            sePlayDict.Clear();
        }

        #endregion フィールドとコンストラクタ

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        /// <param name="name">使えないであろうアセット名</param>
        /// <returns></returns>
        private string ErrorMessage(string name)
        {
            return "再生する音データのアセット名(" + name + ")がありません" +
                "アセット名の確認、Dictionaryに登録しているか確認してください";
        }

        #region BGM(MP3:MediaPlayer)関連

        /// <summary>
        /// BGM(MP3)の読み込み
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="filepath">ファイルパス</param>
        public void LoadBGM(string name, string filepath = "./Sound/")
        {
            //既に登録されているか？
            if (bgms.ContainsKey(name))
            {
                return;
            }
            //MP3の読み込みとディクショナリへ登録
            bgms.Add(name, contentManager.Load<Song>(filepath + name));
        }

        /// <summary>
        /// BGMが停止中か？
        /// </summary>
        /// <returns>停止中ならtrue</returns>
        public bool IsStopBGM()
        {
            return (MediaPlayer.State == MediaState.Stopped);
        }

        /// <summary>
        /// BGMが再生中か？
        /// </summary>
        /// <returns>再生中ならtrue</returns>
        public bool IsPlayingBGM()
        {
            return (MediaPlayer.State == MediaState.Playing);
        }

        /// <summary>
        /// BGMが一時停止中か？
        /// </summary>
        /// <returns>一時停止中ならtrue</returns>
        public bool IsPauseBGM()
        {
            return (MediaPlayer.State == MediaState.Paused);
        }

        /// <summary>
        /// BGM停止
        /// </summary>
        public void StopBGM()
        {
            MediaPlayer.Stop();
            currentBGM = null;
        }

        /// <summary>
        /// BGM再生
        /// </summary>
        /// <param name="name"></param>
        public void PlayBGM(string name)
        {
            Debug.Assert(bgms.ContainsKey(name), ErrorMessage(name));

            if (currentBGM == name)
            {
                return;
            }

            if (IsPlayingBGM())
            {
                StopBGM();
            }

            //ボリューム設定
            MediaPlayer.Volume = 0.5f;

            currentBGM = name;
            MediaPlayer.Play(bgms[currentBGM]);
        }

        /// <summary>
        /// BGM一時停止
        /// </summary>
        public void PauseBGM()
        {
            if (IsPlayingBGM())
            {
                MediaPlayer.Pause();
            }
        }

        /// <summary>
        /// BGM一時停止からの再生
        /// </summary>
        public void ResumeBGM()
        {
            if (IsPauseBGM())
            {
                MediaPlayer.Resume();
            }
        }

        /// <summary>
        /// BGMループフラグを変更
        /// </summary>
        /// <param name="loopFlag"></param>
        public void ChangeBGMLoopFlag(bool loopFlag)
        {
            MediaPlayer.IsRepeating = loopFlag;
        }

        #endregion BGM(MP3:MediaPlayer)関連

        #region WAV(SE:SoundEffect)関連

        public void LoadSE(string name, string filepath = "./Sound/")
        {
            if (soundEffects.ContainsKey(name))
            {
                return;
            }
            soundEffects.Add(name, contentManager.Load<SoundEffect>(filepath + name));
        }

        public void PlaySE(string name)
        {
            Debug.Assert(soundEffects.ContainsKey(name), ErrorMessage(name));

            soundEffects[name].Play();
        }
        #endregion WAV(SE:SoundEffect)関連

        #region WAVインスタンス関連

        /// <summary>
        /// WAVインスタンスの作成
        /// </summary>
        /// <param name="name">アセット名</param>
        public void CreateSEInstance(string name)
        {
            if (seInstances.ContainsKey(name))
            {
                return;
            }

            Debug.Assert(
                soundEffects.ContainsKey(name),
                "先に" + name + "の読み込み処理を行ってください"
                );

            seInstances.Add(name, soundEffects[name].CreateInstance());
        }

        /// <summary>
        /// 指定SE再生
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="no">管理番号</param>
        /// <param name="loopFlag"></param>
        public void PlaySEInstance(string name, int no, bool loopFlag = false)
        {
            Debug.Assert(seInstances.ContainsKey(name), ErrorMessage(name));

            if (sePlayDict.ContainsKey(name + no))
            {
                return;
            }
            var date = seInstances[name];
            date.IsLooped = loopFlag;
            date.Play();
            sePlayDict.Add(name + no, date);
        }

        /// <summary>
        /// 指定SE停止
        /// </summary>
        /// <param name="name"></param>
        /// <param name="no"></param>
        public void StoppedSE(string name, int no)
        {
            if (sePlayDict.ContainsKey(name + no) == false)
            {
                return;
            }
            if (sePlayDict[name + no].State == SoundState.Playing)
            {
                sePlayDict[name + no].Stop();
            }
        }


        /// <summary>
        /// SE全停止
        /// </summary>
        public void StoppesSE()
        {
            foreach (var se in sePlayDict)
            {
                if (se.Value.State == SoundState.Playing)
                {
                    se.Value.Stop();
                }
            }
        }


        /// <summary>
        /// 指定したSEを削除
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="no">管理番号</param>
        public void RemoveSE(string name, int no)
        {
            if (sePlayDict.ContainsKey(name + no) == false)
            {
                return;
            }
            sePlayDict.Remove(name + no);
        }


        public void RemoveSE()
        {
            sePlayDict.Clear();
        }

        /// <summary>
        /// 指定SE一時停止
        /// </summary>
        /// <param name="name"></param>
        /// <param name="no"></param>
        public void PauseSE(string name, int no)
        {
            if (sePlayDict.ContainsKey(name + no) == false)
            {
                return;
            }
            if (sePlayDict[name + no].State == SoundState.Playing)
            {
                sePlayDict[name + no].Pause();
            }
        }

        /// <summary>
        /// 全SE一時停止
        /// </summary>
        public void PauseSE()
        {
            foreach (var se in sePlayDict)
            {
                if (se.Value.State == SoundState.Playing)
                {
                    se.Value.Pause();
                }
            }
        }

        /// <summary>
        /// 一時停止中の全SEを復帰
        /// </summary>
        /// <param name="name"></param>
        /// <param name="no"></param>
        public void ResumeSE(string name, int no)
        {
            if (sePlayDict.ContainsKey(name + no) == false)
            {
                return;
            }

            if (sePlayDict[name + no].State == SoundState.Paused)
            {
                sePlayDict[name + no].Resume();
            }
        }


        public bool IsPlaySEInstance(string name, int no)
        {
            return sePlayDict[name + no].State == SoundState.Playing;
        }

        public bool IsStoppedSEInstance(string name, int no)
        {
            return sePlayDict[name + no].State == SoundState.Stopped;
        }

        public bool IsPausedSEInstance(string name, int no)
        {
            return sePlayDict[name + no].State == SoundState.Paused;
        }

        #endregion WAVインスタンス関連
    }
}
