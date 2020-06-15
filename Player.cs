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
        public int PosY { get; private set; }
        public int PosX { get; private set; }

        public Board _board;

        private Random rand = new Random();

        enum Dir
        {
            Up = 0,
            Left = 1,
            Down = 2,
            Right = 3
        }

        int _dir = (int) Dir.Up;
        List<Pos> _points = new List<Pos>();

        public void Initialize(int posY, int posX, Board board)
        {
            this.PosY = posY;
            this.PosX = posX;
            this._board = board;

            //RightHand();
            //BFS();
            AStar();
        }

        struct PQNode : IComparable<PQNode>
        {
            public int F;
            public int G;

            public int Y;
            public int X;

            public int CompareTo([AllowNull] PQNode other)
            {
                if (F == other.F) return 0;
                return F < other.F ? 1 : -1;
            }
        }

        private void AStar()
        {
                                    // U, L, D, R, UL, DL, DR, UR
            int[] deltaY = new int[] { -1, 0, 1, 0, -1, 1, 1, -1 };
            int[] deltaX = new int[] { 0, -1, 0, 1, -1, -1, 1, 1 };
            int[] cost = new int[] { 10, 10, 10, 10, 14, 14, 14, 14 };

            // 점수 매기기
            // F = G + H
            // F = 최종점수 (작을 수록 좋음. 경로에 따라 달라짐)
            // G = 시작점에서 해당 좌표까지 이동하는데 드는 비용 (작을 수록 좋음, 경로에 따라 달라짐)
            // H = 목적지에서 얼마나 가까운지 (작을 수록 좋음, 고정)

            // (Y, X) 이미 방문 했는지 여부 (방문 = Closed 상태)
            bool[,] closed = new bool[_board.Size, _board.Size];

            // (y, x) 가는 길을 한 번이라도 발견했는지 상태 저장
            // 발견X = MaxValue;
            // 발견O = F = G+ H
            int[,] open = new int[_board.Size, _board.Size];
            for (int y = 0; y < _board.Size; y++)
                for (int x = 0; x < _board.Size; x++)
                    open[y, x] = Int32.MaxValue;

            Pos[,] parent = new Pos[_board.Size, _board.Size];

            // 시작점 발견 (예약 진행)
            open[PosY, PosX] = 10 * (Math.Abs(_board.DestY - PosY) + Math.Abs(_board.DestX - PosX));

            // 오픈 리스트에 있는 정보 중, 가장 좋은 후보를 빠르게 뽑아오기 위한 도구
            PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();
            pq.Push(new PQNode() { 
                F = 10* (Math.Abs(_board.DestY - PosY) + Math.Abs(_board.DestX - PosX)), 
                G = 0, 
                Y = PosY, 
                X = PosX });
            parent[PosY, PosX] = new Pos(PosY, PosX);

            PQNode node;
            int nextY, nextX;
            while (pq.Count() > 0)
            {
                //제일 좋은 후보를 찾는다.
                node = pq.Pop();

                // 동일한 좌표를 여러 경로로 찾아서, 더 빠른 경로로 인해서 이미 방문된 경우 싑
                if (closed[node.Y, node.X])
                    continue;

                // 방문한다
                closed[node.Y, node.X] = true;
                // 목적지에 도착했으면 바로 종료
                if (node.Y == _board.DestY && node.X == _board.DestX)
                    break;

                // 상하좌우 등 이동할 수 있는 좌표인지 확인해서 예약한다.
                for(int i = 0; i < deltaY.Length; i++)
                {
                    nextY = node.Y + deltaY[i];
                    nextX = node.X + deltaX[i];

                    // 유효범위를 벗어났으면 스킵
                    if (nextX < 0 || nextX >= _board.Size || nextY < 0 || nextY >= _board.Size)
                        continue;
                    // 벽으로 막혀서 갈 수 없으면 스킵
                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;
                    // 이미 방문한 곳은 스킵
                    if (closed[nextY, nextX])
                        continue;

                    // G = 시작점에서 해당 좌표까지 이동하는데 드는 비용 (작을 수록 좋음, 경로에 따라 달라짐)
                    int g = node.G + cost[i];
                    // H = 목적지에서 얼마나 가까운지 (작을 수록 좋음, 고정)
                    int h =  10* (Math.Abs(_board.DestY - nextY) + Math.Abs(_board.DestX - nextX));

                    // 다른 경로에서 더 빠른길을 찾았으면 스킵
                    if (open[nextY, nextX] < g + h)
                        continue;

                    // 예약 진행
                    open[nextY, nextX] = g + h;
                    pq.Push(new PQNode() { F = 10* (g + h), G = g, Y = nextY, X = nextX });
                    parent[nextY, nextX] = new Pos(node.Y, node.X);
                }
            }

            CalcPathFromParent(parent);
        }

        private void BFS()
        {

            int[] deltaY = new int[] { -1, 0, 1, 0 };
            int[] deltaX = new int[] { 0, -1, 0, 1};

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
            while ( q.Count > 0 )
            {
                pos = q.Dequeue();
                nowY = pos.Y;
                nowX = pos.X;

                for ( int i = 0; i < 4; i++ )
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

            CalcPathFromParent(parent);

        }

        private void CalcPathFromParent(Pos[,] parent)
        {
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
        }

        private void RightHand()
        {
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
