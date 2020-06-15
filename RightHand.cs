using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithm
{
    class RightHand
    {
        private Board _board;
        public int PosY { get; private set; }
        public int PosX { get; private set; }

        enum Dir
        {
            Up = 0,
            Left = 1,
            Down = 2,
            Right = 3
        }

        private int _dir = (int)Dir.Up;

        private List<Pos> _points = new List<Pos>();

        public List<Pos> FindRoad(Board board, int posX, int posY)
        {
            this.PosY = posY;
            this.PosX = posX;
            this._board = board;

            // 현재 바라보고 있는 방향을 기준으로, 좌표 변화를 나타낸다.
            int[] frontY = new int[] { -1, 0, 1, 0 };
            int[] frontX = new int[] { 0, -1, 0, 1 };
            int[] rightY = new int[] { 0, -1, 0, 1 };
            int[] rightX = new int[] { 1, 0, -1, 0 };

            _points.Add(new Pos(PosX, PosY));

            // 목적지까지 모든 경로 계산.
            while (PosY != this._board.DestY || PosX != this._board.DestX)
            {
                // 1. 현재 바라보는 방향을 기준으로 오른쪽으로 갈 수 있는지 확인
                if (_board.Tile[PosY + rightY[_dir], PosX + rightX[_dir]] == Board.TileType.Empty)
                {
                    // 오른쪽 방향으로 90도 회전
                    _dir = (_dir - 1 + 4) % 4; // ex => (3 - 1 + 4) % 4 = 2

                    // 앞으로 한보 전진
                    PosY += frontY[_dir];
                    PosX += frontX[_dir];
                    _points.Add(new Pos(PosY, PosX));
                }
                // 2. 현재 바라보는 방향을 기준으로 전진 할 수 있는지 확인
                else if (_board.Tile[PosY + frontY[_dir], PosX + frontX[_dir]] == Board.TileType.Empty)
                {
                    // 앞으로 한보 전진
                    PosY += frontY[_dir];
                    PosX += frontX[_dir];
                    _points.Add(new Pos(PosY, PosX));
                }
                else
                {
                    // 왼쪽으로 90도 회전
                    _dir = (_dir + 1 + 4) % 4;
                }
            }

            return _points;
        }

    }
    
}
