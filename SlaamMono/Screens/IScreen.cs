using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono
{
    public interface IScreen
    {
        void Open();

        void Update();

        void Draw(SpriteBatch batch);

        void Close();
    }
}
