using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Slaam
{
    public interface Screen
    {
        void Initialize();

        void Update();

        void Draw(SpriteBatch batch);

        void Dispose();
    }
}
