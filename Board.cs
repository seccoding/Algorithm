using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Algorithm
{
    class Board
    {
        const char CIRCLE = '\u25cf';

        public TileType[,] Tile { get; private set; }
        public int Size { get; private set; }
        public int DestY { get; private set; }
        public int DestX { get; private set; }

        private Player _player;

        public enum TileType
        {
            Empty,
            Wall
        }

        public void Initialze(int size, Player player)
        {
            if (size % 2 == 0)
                return;

            _player = player;

            Tile = new TileType[size, size];
            Size = size;

            DestY = size - 2;
            DestX = size - 2;

            // Mazes for Programmers
            //this.BinaryByBynaryTree();

            // SideWinder
            this.BinaryBySideWinder();
        }

        private void BinaryBySideWinder()
        {
            // 모든 길을 막는 작업
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        // 짝수 번째 공간은 벽으로 처리
                        Tile[y, x] = TileType.Wall;
                    else
                        // 홀수 번째 공간은 빈 공간으로 처리
                        Tile[y, x] = TileType.Empty;
                }
            }

            // 랜덤하게 우측 혹은 아래로 길을 뚫는 작업
            Random rand = new Random();
            int randValue;
            int count;

            int randomIndex;
            for (int y = 0; y < Size; y++)
            {
                count = 1;
                for (int x = 0; x < Size; x++)
                {
                    // 짝수번째 공간은 처리하지 않음.
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;

                    // 좌하단 2번째 공간은 처리하지 않음.
                    if (y == Size - 2 && x == Size - 2)
                        continue;

                    // y축 두번째 라인은 모두 비어있는 처리
                    if (y == Size - 2)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue;
                    }

                    // x축 두번째 라인은 모두 비어있는 처리
                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue;
                    }

                    // 랜덤하게 길 뚫음.
                    randValue = rand.Next(0, 2);
                    if (randValue == 0)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        count++;
                    }
                    else
                    {
                        randomIndex = rand.Next(0, count);
                        Tile[y + 1, x - randomIndex * 2] = TileType.Empty;
                        count = 1;
                    }
                }
            }
        }

        private void BinaryByBynaryTree()
        {
            // 모든 길을 막는 작업
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        // 짝수 번째 공간은 벽으로 처리
                        Tile[y, x] = TileType.Wall;
                    else
                        // 홀수 번째 공간은 빈 공간으로 처리
                        Tile[y, x] = TileType.Empty;
                }
            }

            // 랜덤하게 우측 혹은 아래로 길을 뚫는 작업
            Random rand = new Random();
            int randValue;
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    // 짝수번째 공간은 처리하지 않음.
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;

                    // 좌하단 2번째 공간은 처리하지 않음.
                    if (y == Size - 2 && x == Size - 2)
                        continue;

                    // y축 두번째 라인은 모두 비어있는 처리
                    if (y == Size - 2)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue;
                    }

                    // x축 두번째 라인은 모두 비어있는 처리
                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue;
                    }

                    // 랜덤하게 길 뚫음.
                    randValue = rand.Next(0, 2);
                    if (randValue == 0)
                        Tile[y, x + 1] = TileType.Empty;
                    else
                        Tile[y + 1, x] = TileType.Empty;
                }
            }
        }

        public void Render()
        {
            ConsoleColor prevColor = Console.ForegroundColor;
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    // 플레이어 좌표를 가져와 Player 전용 색상으로 표시.
                    if (y == _player.PosY && x == _player.PosX)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else if (y == DestY && x == DestX)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else
                        Console.ForegroundColor = GetTileColor(Tile[y, x]);
                    Console.Write(CIRCLE);
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = prevColor;
        }

        private ConsoleColor GetTileColor(TileType type)
        {
            return type switch
            {
                TileType.Empty => ConsoleColor.Green,
                TileType.Wall => ConsoleColor.Red,
                _ => ConsoleColor.Green,
            };
        }

    }
}
