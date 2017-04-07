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

    class Block : Item
    {
        public static int UP = 0;
        public static int DOWN = 1;
        public static int LEFT = 2;
        public static int RIGHT = 3;
        public Texture2D LineTexture;

        public Block(Point point, Point size)
        {
            this.Position = point;
            this.Size = size;
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
        }

        public void Draw(SpriteBatch batch, int In, int Out)
        {
            var temp1 = CreateRectangle(In);
            var temp2 = CreateRectangle(Out);
            batch.Draw(LineTexture, temp1, Color.White);
            batch.Draw(LineTexture, temp2, Color.White);
        }

        private Rectangle CreateRectangle(int position)
        {
            if (position == UP)
            {
                return new Rectangle(Position.X + (Size.X * 4 / 10), Position.Y, Size.X / 5, Size.Y / 2);
            }
            if (position == DOWN)
            {
                return new Rectangle(Position.X + (Size.X * 4 / 10), Position.Y + Size.Y / 2, Size.X / 5, Size.Y / 2);
            }
            if (position == LEFT)
            {
                return new Rectangle(Position.X, Position.Y + (Size.Y * 4 / 10), Size.X / 2, Size.Y / 5);
            }
            if (position == RIGHT)
            {
                return new Rectangle(Position.X + Size.X / 2, Position.Y + (Size.Y * 4 / 10), Size.X / 2, Size.Y / 5);
            }
            return new Rectangle(0, 0, 0, 0);
        }
    }

}
