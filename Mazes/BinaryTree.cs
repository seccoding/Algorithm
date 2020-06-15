using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithm.Mazes
{
    class BinaryTree
    {
        
        public Board.TileType[,] Tile { get; private set; }

        public Board.TileType[,] MakeMazes(int Size)
        {
            Tile = new Board.TileType[Size, Size];

            // 모든 길을 막는 작업
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        // 짝수 번째 공간은 벽으로 처리
                        Tile[y, x] = Board.TileType.Wall;
                    else
                        // 홀수 번째 공간은 빈 공간으로 처리
                        Tile[y, x] = Board.TileType.Empty;
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
                        Tile[y, x + 1] = Board.TileType.Empty;
                        continue;
                    }

                    // x축 두번째 라인은 모두 비어있는 처리
                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = Board.TileType.Empty;
                        continue;
                    }

                    // 랜덤하게 길 뚫음.
                    randValue = rand.Next(0, 2);
                    if (randValue == 0)
                        Tile[y, x + 1] = Board.TileType.Empty;
                    else
                        Tile[y + 1, x] = Board.TileType.Empty;
                }
            }

            return Tile;
        }

    }
}
