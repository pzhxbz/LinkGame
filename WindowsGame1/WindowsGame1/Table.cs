using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1
{

    class Table
    {
        private Point _size;
        private int _num = 36;
        private bool[] _isChoose;
        private Block[] _blocks;
        private int[][] _blocksData;
        private Point _blockPosition;
        private Point _blockSize;
        private int _nowChoose = -1;
        private ArrayList[] m_same;
        private Timer timer;
        private bool _searchFlag = false;
        private ArrayList getLine;
        private ArrayList lineInfo;
        private ArrayList drawLineTask;
        public Texture2D[] BlockTexture;
        public Texture2D LineTexture2D;
        public Texture2D Background;
        public SpriteFont font;
        public Table(int width, int height)
        {
            _size = new Point(width, height);
            _blockPosition = new Point(width / 2 - height / 2, 0);
            _isChoose = new bool[64];
            _blocks = new Block[64];
            _blockSize = new Point(height / 8, height / 8);
            _blocksData = new int[8][];
            for (int i = 0; i < 8; i++)
            {
                _blocksData[i] = new int[8];
            }
            m_same = new ArrayList[10];//假定只有10种
            drawLineTask = new ArrayList();
        }

        public void Init()
        {
            var rand = new Random();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    _blocks[i * 8 + j] = new Block(new Point(_blockPosition.X + _blockSize.X * j, _blockPosition.Y + _blockSize.Y * i), _blockSize);
                    _blocks[i * 8 + j].LineTexture = LineTexture2D;
                    int randNum = rand.Next(BlockTexture.Length - 1);

                    _blocks[i * 8 + j].Texture = BlockTexture[randNum];
                    if (i == 0 || i == 7 || j == 0 || j == 7)
                    {
                        _isChoose[i * 8 + j] = true;
                        _blocksData[i][j] = 0;
                        continue;
                    }
                    _isChoose[i * 8 + j] = false;
                    _blocksData[i][j] = randNum + 1;

                }
            }
            timer = new Timer(60);
            timer.Font = font;
            timer.Position = new Point(_size.X * 4 / 5, _size.Y * 4 / 5);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Background,
                new Rectangle(0, 0, _size.X, _size.Y),   //draw background
                Color.White);
            if (_searchFlag == false)
            {
                getLine = CanLine();
                _searchFlag = true;
            }

            if (_num == 0 || getLine.Count == 0)
            {
                batch.DrawString(font, "congratulation!There is no blocks can line", new Vector2(_size.X / 2, _size.Y / 2), Color.Red);
                return;
            }
            if (timer.GetLatsTime() <= 0)
            {
                batch.DrawString(font, "time over!", new Vector2(_size.X / 2, _size.Y / 2), Color.Red);
                return;
            }
            timer.Draw(batch);
            MouseState mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                _nowChoose = -1;
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_isChoose[i * 8 + j] == false)
                    {
                        if (mousePosition.IsIn(_blocks[i * 8 + j].Position, _blockSize))
                        {
                            _blocks[i * 8 + j].Draw(batch, true);
                            if (mouseState.LeftButton == ButtonState.Pressed)
                            {
                                if (_nowChoose == -1)
                                {
                                    _nowChoose = i * 8 + j;
                                }
                                else
                                {
                                    Point a = new Point(i, j);
                                    Point b = new Point(_nowChoose / 8, _nowChoose % 8);
                                    if (a == b)
                                    {
                                        continue;
                                        //_nowChoose = -1;
                                        //break;
                                    }
                                    foreach (var line in getLine)
                                    {
                                        if ((a == ((Point[])line)[0] && b == ((Point[])line)[1]) || (a == ((Point[])line)[1] && b == ((Point[])line)[0]))
                                        {
                                            var lines = (Point[])lineInfo[getLine.IndexOf(line)];
                                            var tempData = new DrawLineData(batch, lines);
                                            //Thread thread = new Thread(() => DrawBlocksLines(tempData));
                                            //thread.Start();

                                            //DrawBlocksLines(tempData);
                                            drawLineTask.Add(tempData);
                                            _blocksData[a.X][a.Y] = 0;
                                            _blocksData[b.X][b.Y] = 0;
                                            _isChoose[_nowChoose] = true;
                                            _isChoose[i * 8 + j] = true;
                                            _nowChoose = -1;
                                            _num -= 2;
                                            _searchFlag = false;
                                            break;
                                        }
                                    }
                                    _nowChoose = -1;

                                }
                            }
                        }
                        else if (_nowChoose == i * 8 + j)
                        {
                            _blocks[i * 8 + j].Draw(batch, true);
                        }
                        else
                        {
                            _blocks[i * 8 + j].Draw(batch);
                        }

                    }
                }
            }
            for (int i = 0; i < drawLineTask.Count; i++)
            {
                var temp = (DrawLineData)drawLineTask[i];
                DrawBlocksLines(temp);
                temp.Times--;
                if (temp.Times <= 0)
                {
                    drawLineTask.RemoveAt(i);
                }
            }

        }

        private ArrayList CanLine()
        {
            lineInfo = null;
            lineInfo = new ArrayList();
            for (int i = 0; i < 10; i++)
            {
                m_same[i] = null;
            }
            for (int x = 0; x < 8; ++x)
            {
                for (int y = 0; y < 8; ++y)
                {
                    var index = _blocksData[x][y];
                    if (m_same[index] == null)
                        m_same[index] = new ArrayList();
                    m_same[index].Add(new Point(x, y));
                }
            }
            Point[] pt = new Point[2];
            ArrayList m_results = new ArrayList();
            for (int i = 1; i < m_same.GetLength(0); ++i)
            {//遍历ArrayList[]
                if (m_same[i] != null)
                {
                    for (int j = 0; j < m_same[i].Count - 1; ++j)
                    {//遍历相同元素集合
                        for (int k = j + 1; k < m_same[i].Count; ++k)
                        {//取组合数C(n,2)
                            pt[0] = (Point)m_same[i][j];
                            pt[1] = (Point)m_same[i][k];
                            if (pt[0] == pt[1])
                            {
                                continue;
                            }
                            Point[] temp;
                            if (Available(ref pt, out temp))
                            {
                                m_results.Add(new Point[2] { pt[0], pt[1] });
                                lineInfo.Add(temp);
                            }
                            //return pt;
                        }
                    }
                }
            }
            return m_results;
        }
        private void findEx(ref Point pt, out ArrayList rowEx, out ArrayList colEx)
        {
            colEx = new ArrayList();
            colEx.Add(pt);
            //纵向
            int k = 1;
            int x = pt.X;
            int y = pt.Y;
            bool flag1 = true, flag2 = true;//两个方向是否非空
            while (true)
            {
                //flag1->上 flag2->下
                //边界路径
                if (x - k == 0 || x + k == 7)
                {
                    if (x - k == 0 && flag1 != false)
                    {
                        colEx.Add(new Point(x - k, y));
                        flag1 = false;
                    }
                    if (x + k == 7 && flag2 != false)
                    {
                        colEx.Add(new Point(x + k, y));
                        flag2 = false;
                    }
                }
                if (x - k >= 0 && x - k <= 7 && flag1 != false)
                {
                    if (_blocksData[x - k][y] == 0)
                        colEx.Add(new Point(x - k, y));
                    else
                        flag1 = false;
                }
                if (x + k >= 0 && x + k <= 7 && flag2 != false)
                {
                    if (_blocksData[x + k][y] == 0)
                        colEx.Add(new Point(x + k, y));
                    else
                        flag2 = false;
                }
                if (!flag1 && !flag2)
                    break;
                k++;
            }
            //横向
            rowEx = new ArrayList();
            rowEx.Add(pt);
            k = 1;
            flag1 = true;
            flag2 = true;
            while (true)
            {
                //flag1->左 flag2->右
                //边界路径
                if (y - k == 0 || y + k == 7)
                {
                    if (y - k == 0 && flag1 != false)
                    {
                        rowEx.Add(new Point(x, y - k));
                        flag1 = false;
                    }
                    if (y + k == 7 && flag2 != false)
                    {
                        rowEx.Add(new Point(x, y + k));
                        flag2 = false;
                    }
                }
                if (y - k >= 0 && y - k <= 7 && flag1 != false)
                {
                    if (_blocksData[x][y - k] == 0)
                        rowEx.Add(new Point(x, y - k));
                    else
                        flag1 = false;
                }
                if (y + k >= 0 && y + k <= 7 && flag2 != false)
                {
                    if (_blocksData[x][y + k] == 0)
                        rowEx.Add(new Point(x, y + k));
                    else
                        flag2 = false;
                }
                if (!flag1 && !flag2)
                    break;

                k++;
            }
        }

        private bool Available(ref Point[] pt, out Point[] line)
        {
            ArrayList[] rowEx, colEx;//建立横纵扩展位            
            rowEx = new ArrayList[2];
            colEx = new ArrayList[2];
            //分别计算两点的扩展位置(包括本身)
            findEx(ref pt[0], out rowEx[0], out colEx[0]);
            findEx(ref pt[1], out rowEx[1], out colEx[1]);

            //rowEx[]

            for (int i = 0; i < rowEx[0].Count; ++i)
            {
                for (int j = 0; j < rowEx[1].Count; ++j)
                {
                    int y1 = ((Point)rowEx[0][i]).Y;
                    int x1 = ((Point)rowEx[0][i]).X;
                    int y2 = ((Point)rowEx[1][j]).Y;
                    int x2 = ((Point)rowEx[1][j]).X;
                    if (y1 == y2)
                    {
                        bool flag = true;
                        //公共部分,检查是否连通
                        for (int k = Math.Min(x1, x2) + 1; k < Math.Max(x1, x2); ++k)
                        {
                            if (y1 != 0 && y1 != 7 && _blocksData[k][y1] != 0)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (y1 == 0 || y1 == 7 || flag)
                        {
                            line = new Point[4];
                            line[0] = pt[0];
                            line[1] = new Point(x1, y1);
                            line[2] = new Point(x2, y2);
                            line[3] = pt[1];
                            return true;
                        }
                    }
                }
            }
            //colEx[]
            for (int i = 0; i < colEx[0].Count; ++i)
            {
                for (int j = 0; j < colEx[1].Count; ++j)
                {
                    int y1 = ((Point)colEx[0][i]).Y;
                    int x1 = ((Point)colEx[0][i]).X;
                    int y2 = ((Point)colEx[1][j]).Y;
                    int x2 = ((Point)colEx[1][j]).X;
                    if (x1 == x2)
                    {
                        bool flag = true;
                        //公共部分,检查是否连通
                        for (int k = Math.Min(y1, y2) + 1; k < Math.Max(y1, y2); ++k)
                        {
                            if (x1 != 0 && x1 != 7 && _blocksData[x1][k] != 0)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (x1 == 0 || x1 == 7 || flag)
                        {
                            line = new Point[4];
                            line[0] = pt[0];
                            line[1] = new Point(x1, y1);
                            line[2] = new Point(x2, y2);
                            line[3] = pt[1];
                            return true;
                        }
                    }
                }
            }
            line = null;
            return false;
        }
        private void DrawBlocksLines(DrawLineData data)
        {
            SpriteBatch batch = data.Batch;
            Point[] line = data.LineInfo;
            Point[] position = new Point[4];
            for (int i = 0; i < 4; i++)
            {
                var temp = _blocks[line[i].X * 8 + line[i].Y];
                position[i] = new Point(temp.Position.X + temp.Size.X / 2, temp.Position.Y + temp.Size.Y / 2);
            }
            for (int i = 0; i < 3; i++)
            {
                var temp1 = position[i];
                var temp2 = position[i + 1];
                if (temp1 == temp2)
                {
                    continue;
                }
                if (temp1.X > temp2.X || temp1.Y > temp2.Y)
                {
                    var area = new Rectangle(temp2.X, temp2.Y, temp1.X - temp2.X + 10, temp1.Y - temp2.Y + 10);
                    batch.Draw(_blocks[i].LineTexture, area, Color.White);
                }
                else
                {
                    var area = new Rectangle(temp1.X, temp1.Y, temp2.X - temp1.X + 10, temp2.Y - temp1.Y + 10);
                    batch.Draw(_blocks[i].LineTexture, area, Color.White);
                }
            }
            //Thread.Sleep(1000);
        }

    }

    class DrawLineData
    {
        public SpriteBatch Batch;
        public Point[] LineInfo;
        public int Times;
        public DrawLineData(SpriteBatch batch, Point[] lineInfo)
        {
            this.Batch = batch;
            this.LineInfo = lineInfo;
            this.Times = 30;
        }
    }

}
