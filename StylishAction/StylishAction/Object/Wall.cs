using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylishAction.Object
{
    class Wall : Object
    {
        public Wall(string name, Vector2 size, Vector2 position) : base(name, size)
        {
            mPosition = position;
        }

        public override void Collision(Object other)
        {
            
        }
    }
}
