using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1
{

    class Point
    {
        public int X;
        public int Y;
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public void SetPosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool IsIn(Point rect, Point size)
        {
            if (X > rect.X && X < rect.X + size.X)
            {
                if (Y > rect.Y && Y < rect.Y + size.Y)
                {
                    return true;
                }
            }
            return false;
        }
        public static Point operator +(Point a, Point b) =>
            new Point(a.X + b.X, a.Y + b.Y);
        public static Point operator -(Point a, Point b) =>
            new Point(a.X - b.X, a.Y - b.Y);
        public static Point operator *(Point a, int b) =>
            new Point(a.X * b, a.Y * b);

        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }
        public static bool operator ==(Point a, Point b)
        {
            if (a.X != b.X)
            {
                return false;
            }
            if (a.Y != b.Y)
            {
                return false;
            }
            return true;
        }
    }


    class Menu
    {
        public Texture2D ChooseTexture;
        public Texture2D SoundSetting;
        public Texture2D Background;
        public SpriteFont Font;
        Point _blockSize;
        Point _windowSize;
        public static int WIDTH = 80;
        public static int HEIGHT = 45;
        public static int CHOOSE = -1;
        public static int GAMESTART = 0;
        public static int LOAD = 1;
        public static int SETTING = 2;
        public static int EXIT = 3;
        public static int OTHER = 100;
        private Queue<Item> _tasks;
        private int states;

        public Menu(int width, int height)
        {
            _windowSize = new Point(width, height);
            int blockW = width / WIDTH;
            int blockH = height / HEIGHT;
            _blockSize = new Point(blockW, blockH);
            states = CHOOSE;

        }
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Background,
                new Rectangle(0, 0, _windowSize.X, _windowSize.Y),   //draw background
                Color.LightGray);
            MouseState mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);
            foreach (var task in _tasks)
            {
                if (mousePosition.IsIn(task.Position, task.Size))
                {
                    task.Draw(batch, true);
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        states = task.id;
                        task.MouseEvent(mouseState);
                    }
                }
                else
                {
                    task.Draw(batch);
                }
            }

        }

        public void InitDrawTask()
        {
            _tasks = new Queue<Item>();
            _tasks.Enqueue(ProduceList("Game Start", GAMESTART));
            _tasks.Enqueue(ProduceList("Load Game", LOAD));
            _tasks.Enqueue(ProduceList("Setting", SETTING));
            _tasks.Enqueue(ProduceList("Exit", EXIT));
        }

        public void InitState()
        {
            states = CHOOSE;
        }

        public int GetStates()
        {
            return states;
        }

        private MenuItem ProduceList(String buf, int id)
        {
            var result = new MenuItem(buf);
            result.Font = this.Font;
            result.Size = new Point(_blockSize.X * 8, _blockSize.Y * 3);
            result.Position = new Point(WIDTH * 3 / 4 * _blockSize.X, HEIGHT / 2 * _blockSize.Y + id * _blockSize.Y * 5);
            result.Texture = ChooseTexture;
            result.id = id;
            return result;
        }


    }
}
