using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;

namespace Algorithm
{
    class Pos
    {
        public Pos(int Y, int X)
        {
            this.Y = Y;
            this.X = X;
        }
        public int Y;
        public int X;
    }

    class Player
    {
        private Board _board;
        private List<Pos> _points;
        public int PosY { get; private set; }
        public int PosX { get; private set; }

        public void Initialize(int posY, int posX, Board board)
        {
            this._board = board;
            this.PosY = posY;
            this.PosX = posX;

            //this._points = new RightHand().FindRoad(this._board, this.PosY, this.PosX);
            //this._points = new BFS().FindRoad(this._board, this.PosY, this.PosX);
            this._points = new AStar().FindRoad(this._board, this.PosY, this.PosX);
        }

        const int MOVE_TICK = 10;
        int _sumTick;
        int _lastIndex = 0;

        public void Update(int deltaTick)
        {
            if ( _lastIndex >= _points.Count )
            {
                _lastIndex = 0;
                _points.Clear();
                _board.Initialze(_board.Size, this);
                Initialize(1, 1, _board);

            }

            _sumTick += deltaTick;
            if ( _sumTick >= MOVE_TICK )
            {
                _sumTick = 0;

                PosY = _points[_lastIndex].Y;
                PosX = _points[_lastIndex].X;
                _lastIndex++;
            }
        }
        
    }
}
