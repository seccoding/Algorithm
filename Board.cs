using Algorithm.Mazes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Algorithm
{
    

    class Board
    {
        const char CIRCLE = '\u25cf';

        public enum TileType
        {
            Empty,
            Wall
        }

        public int Size { get; private set; }
        public int DestY { get; private set; }
        public int DestX { get; private set; }

        private Player _player;

        public TileType[,] Tile { get; private set; }

        public void Initialze(int size, Player player)
        {
            if (size % 2 == 0)
                return;

            _player = player;

            Size = size;

            DestY = size - 2;
            DestX = size - 2;

            // Mazes for Programmers
            //Tile = new BinaryTree().MakeMazes(Size);

            // SideWinder
            Tile = new SideWinder().MakeMazes(Size);
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
