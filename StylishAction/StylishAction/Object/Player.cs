using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StylishAction.Utility;

namespace StylishAction.Object
{
    class Player : Object
    {
        public Player(string name) : base(name)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            mPosition = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            Translate(Input.Velocity());
        }
    }
}
