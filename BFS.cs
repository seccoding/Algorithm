using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithm
{
    class BFS
    {
        private Board _board;
        public int PosY { get; private set; }
        public int PosX { get; private set; }

        public List<Pos> FindRoad(Board board, int posX, int posY)
        {
            this.PosY = posY;
            this.PosX = posX;
            this._board = board;

            int[] deltaY = new int[] { -1, 0, 1, 0 };
            int[] deltaX = new int[] { 0, -1, 0, 1 };

            bool[,] found = new bool[_board.Size, _board.Size];
            Pos[,] parent = new Pos[_board.Size, _board.Size];

            Queue<Pos> q = new Queue<Pos>();
            q.Enqueue(new Pos(PosY, PosX));
            found[PosY, PosX] = true;
            parent[PosY, PosX] = new Pos(PosY, PosX);

            Pos pos;
            int nowY;
            int nowX;

            int nextY;
            int nextX;
            while (q.Count > 0)
            {
                pos = q.Dequeue();
                nowY = pos.Y;
                nowX = pos.X;

                for (int i = 0; i < 4; i++)
                {
                    nextY = nowY + deltaY[i];
                    nextX = nowX + deltaX[i];

                    if (nextX < 0 || nextX >= _board.Size || nextY < 0 || nextY >= _board.Size)
                        continue;

                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;

                    if (found[nextY, nextX])
                        continue;

                    q.Enqueue(new Pos(nextY, nextX));
                    found[nextY, nextX] = true;
                    parent[nextY, nextX] = new Pos(nowY, nowX);
                }
            }

            return CalcPathFromParent(parent);
        }

        
        private List<Pos> CalcPathFromParent(Pos[,] parent)
        {
            List<Pos> _points = new List<Pos>();

            // BFS 역순으로 길 찾기
            int y = _board.DestY;
            int x = _board.DestX;

            Pos parentPos;
            while (parent[y, x].Y != y || parent[y, x].X != x)
            {
                _points.Add(new Pos(y, x));

                parentPos = parent[y, x];
                y = parentPos.Y;
                x = parentPos.X;
            }

            _points.Add(new Pos(y, x));
            _points.Reverse();

            return _points;
        }
    }

}
