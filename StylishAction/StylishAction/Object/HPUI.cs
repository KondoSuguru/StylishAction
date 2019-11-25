using Microsoft.Xna.Framework;
using StylishAction.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylishAction.Object
{
    class HPUI
    {
        public HPUI()
        {

        }
        public void Draw()
        {
            if (ObjectManager.Instance().GetPlayer() != null)
            {
                for (int i = 0; i < ObjectManager.Instance().GetPlayer().GetMaxHP(); i++)
                {
                    GameDevice.Instance().GetRenderer().DrawTexture("HP_frame", new Vector2(100 + i * 64, 50));
                }
                for (int i = 0; i < ObjectManager.Instance().GetPlayer().GetHP(); i++)
                {
                    GameDevice.Instance().GetRenderer().DrawTexture("HP_player", new Vector2(100 + i * 64, 50));
                }
            }
            if (ObjectManager.Instance().GetBoss() != null)
            {
                for (int i = 0; i < ObjectManager.Instance().GetBoss().GetMaxHP(); i++)
                {
                    GameDevice.Instance().GetRenderer().DrawTexture("HP_frame", new Vector2(Screen.WIDTH - 164 - i * 64, 50));
                }
                for (int i = 0; i < ObjectManager.Instance().GetBoss().GetHP(); i++)
                {
                    GameDevice.Instance().GetRenderer().DrawTexture("HP_boss", new Vector2(Screen.WIDTH - 164 - i * 64, 50));
                }
            }
        }
    }

    
}
