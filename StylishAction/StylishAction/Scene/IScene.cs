using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylishAction.Scene
{
    interface IScene
    {
        void Initialize();
        void Update(float deltaTime);
        void Draw();
        void Shutdown();

        bool IsEnd();
        Scene Next();
    }
}
