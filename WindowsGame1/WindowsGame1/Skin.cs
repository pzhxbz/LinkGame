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
        public Texture2D LineTexture2D;
        public Texture2D GameBack;
        public Skin()
        {

        }

        public void load(ContentManager Content)
        {
            MenuBackground = Content.Load<Texture2D>("Background");
            MenuChoose = Content.Load<Texture2D>("itemChoose");
            Font = Content.Load<SpriteFont>("Font");
            MousePointer = Content.Load<Texture2D>("cursor");
            Picture = new Texture2D[10];
            for (int i = 0; i < 10; i++)
            {
                Picture[i] = Content.Load<Texture2D>(Convert.ToString(i + 1));
            }
            LineTexture2D = Content.Load<Texture2D>("line");
            GameBack = Content.Load<Texture2D>("GameBackground");
        }
    }
}
