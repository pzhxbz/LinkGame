using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{

    class Table
    {
        private Point _size;
        private int _num = 64;
        private bool[] _isChoose;
        private Block[] _blocks;
        private Point _blockPosition;
        private Point _blockSize;
        public Texture2D[] BlockTexture;
        public Texture2D LineTexture2D;
        public Texture2D Background;
        public Table(int width, int height)
        {
            _size = new Point(width, height);
            _blockPosition = new Point(width / 2 - height / 2, 0);
            _isChoose = new bool[64];
            _blocks = new Block[64];
            _blockSize = new Point(height / 8, height / 8);
        }

        public void Init()
        {
            var rand = new Random();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i == 0 || i == 7 || j == 0 || j == 7)
                    {
                        _isChoose[i * 8 + j] = true;
                    }
                    else
                    {
                        _isChoose[i * 8 + j] = false;
                    }
                    _blocks[i * 8 + j] = new Block(new Point(_blockPosition.X + _blockSize.X * j, _blockPosition.Y + _blockSize.Y * i), _blockSize);
                    _blocks[i * 8 + j].Texture = BlockTexture[rand.Next(BlockTexture.Length - 1)];
                    _blocks[i * 8 + j].LineTexture = LineTexture2D;
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Background,
                new Rectangle(0, 0, _size.X, _size.Y),   //draw background
                Color.White);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_isChoose[i * 8 + j] == false)
                    {
                        _blocks[i * 8 + j].Draw(batch);
                    }
                }
            }
        }
    }

}
