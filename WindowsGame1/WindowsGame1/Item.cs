using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1
{
    abstract class Item
    {
        public String buffer;
        public Point Size;
        public Point Position;
        public int State;
        public Texture2D Texture;
        public int id;
        public virtual void MouseEvent(MouseState state)
        {

        }

        public virtual void KeyboradEvent(KeyboardState state)
        {

        }

        public abstract void Draw(SpriteBatch batch, bool isFocus = false);

    }

    class MenuItem : Item
    {

        public SpriteFont Font;
        public MenuItem()
        {

        }

        public MenuItem(String buffer)
        {
            this.buffer = buffer;
        }

        public override void Draw(SpriteBatch batch, bool isFocus = false)
        {
            Rectangle area = new Rectangle(Position.X, Position.Y, Size.X, Size.Y);
            if (isFocus)
            {
                batch.Draw(Texture, area, Color.Black);

            }
            else
            {
                batch.Draw(Texture, area, Color.White);
            }
            batch.DrawString(Font, buffer, new Vector2(Position.X + Size.X / 4, Position.Y + Size.Y / 6), Color.White);
        }
    }

}
