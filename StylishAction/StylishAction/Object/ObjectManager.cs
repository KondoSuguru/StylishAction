using StylishAction.Device;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylishAction.Object
{
    sealed class ObjectManager
    {
        private static ObjectManager mInstance;
        private List<Object> mPlayers;
        private List<Object> mEnemys;
        private List<Object> mStages;
        private List<Object> mEffects;
        private List<Object> mAddObjects;

        private ObjectManager()
        {
            mPlayers = new List<Object>();
            mEnemys = new List<Object>();
            mStages = new List<Object>();
            mEffects = new List<Object>();
            mAddObjects = new List<Object>();
        }

        public static ObjectManager Instance()
        {
            if (mInstance == null)
            {
                mInstance = new ObjectManager();
            }
            return mInstance;
        }

        public void Initialize()
        {
            mPlayers.Clear();
            mEnemys.Clear();
            mStages.Clear();
            mEffects.Clear();
            mAddObjects.Clear();
        }

        public void AddObject(Object obj)
        {
            if (obj == null)
                return;
            mAddObjects.Add(obj);
        }

        public void Update(float deltaTime)
        {
            foreach (var p in mPlayers)
            {
                p.Update(deltaTime);
            }
            foreach (var e in mEnemys)
            {
                e.Update(deltaTime);
            }
            foreach (var s in mStages)
            {
                s.Update(deltaTime);
            }
            foreach(var e in mEffects)
            {
                e.Update(deltaTime);
            }

            foreach (var addObj in mAddObjects)
            {
                addObj.Initialize();
                if(addObj is Player || addObj is PlayerWeakAttack)
                {
                    mPlayers.Add(addObj);
                }
                if(addObj is Enemy || addObj is BossAttack)
                {
                    mEnemys.Add(addObj);
                }
                if(addObj is Wall)
                {
                    mStages.Add(addObj);
                }
                if(addObj is DamageEffect)
                {
                    mEffects.Add(addObj);
                }
            }
            mAddObjects.Clear();

            if (deltaTime != 0)
            {
                Collision_P_E();
                RemoveDeadObject();
            }
        }

        public void Draw()
        {
            foreach(var s in mStages)
            {
                s.Draw();
            }
            foreach(var e in mEnemys)
            {
                e.Draw();
            }
            foreach(var p in mPlayers)
            {
                p.Draw();
            }
            foreach(var e in mEffects)
            {
                e.Draw();
            }
        }

        public void Clear()
        {
            mPlayers.Clear();
            mEnemys.Clear();
            mStages.Clear();
            mEffects.Clear();
            mAddObjects.Clear();
        }

        public List<Object> GetPlayers()
        {
            return mPlayers;
        }

        public List<Object> GetEnemys()
        {
            return mEnemys;
        }

        public List<Object> GetStages()
        {
            return mStages;
        }

        public Player GetPlayer()
        {
            for(int i = 0; i < mPlayers.Count; i++)
            {
                if(mPlayers[i] is Player)
                {
                    return (Player)mPlayers[i];
                }
            }
            return null;
        }

        public Enemy GetBoss()
        {
            for (int i = 0; i < mEnemys.Count; i++)
            {
                if (mEnemys[i] is Enemy)
                {
                    return (Enemy)mEnemys[i];
                }
            }
            return null;
        }

        private void Collision_P_E()
        {
            //プレイヤーリストとエネミーリストの当たり判定（円）
            for(int i = 0; i < mPlayers.Count; i++)
            {
                Object p = mPlayers[i];
                for (int j = 0; j < mEnemys.Count; j++)
                {
                    Object e = mEnemys[j];
                    if(Vector2.Distance(p.GetOrigin(),e.GetOrigin()) <= (p.GetSize().X + e.GetSize().X) / 2)
                    {
                        p.Collision(e);
                        e.Collision(p);
                    }
                }
            }
        }

        private void RemoveDeadObject()
        {
            mPlayers.RemoveAll(obj => obj.IsDead());
            mEnemys.RemoveAll(obj => obj.IsDead());
            mStages.RemoveAll(obj => obj.IsDead());
        }

        public bool IsStageCollisionX(Vector2 origin, Vector2 size)
        {
            //ステージと当たっているか（x方向、円と矩形）
            for(int i = 0; i < mStages.Count; i++)
            {
                Object s = mStages[i];
                if((origin.Y > s.GetPosition().Y - (size.Y / 2)) && (origin.Y < s.GetPosition().Y + s.GetSize().Y + (size.Y / 2)))
                {
                    if(Math.Abs(origin.X - s.GetOrigin().X) < (size.X + s.GetSize().X) / 2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsStageCollisionY(Vector2 origin, Vector2 size)
        {
            //ステージと当たっているか（y方向、円と矩形）
            for (int i = 0; i < mStages.Count; i++)
            {
                Object s = mStages[i];
                if ((origin.X > s.GetPosition().X - (size.X / 2)) && (origin.X < s.GetPosition().X + s.GetSize().X + (size.X / 2)))
                {
                    if (Math.Abs(origin.Y - s.GetOrigin().Y) < (size.Y + s.GetSize().Y) / 2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
