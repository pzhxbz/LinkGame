using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    class Skin
    {
        public Texture2D MousePointer;
        public Texture2D[] Picture;
        public Texture2D MenuChoose;
        public Texture2D MenuBackground;
        public SpriteFont Font;
        public Skin()
        {

        }

        public void load(ContentManager Content)
        {
            MenuBackground = Content.Load<Texture2D>("Background");
            MenuChoose = Content.Load<Texture2D>("itemChoose");
            Font = Content.Load<SpriteFont>("Font");
            MousePointer = Content.Load<Texture2D>("cursor");
        }
    }
}
