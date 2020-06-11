using System;

namespace Algorithm
{
    
    class Program
    {
        const int WAIT_TICK = 1000 / 30;
        
        static void Main(string[] args)
        {
            Player player = new Player();
            Board board = new Board();
            board.Initialze(25, player);
            player.Initialize(1, 1, board);

            // 커서를 숨긴다.
            Console.CursorVisible = false;

            // 프레임 관리
            int lastTick = 0;
            int currentTick = 0;
            int elapsedTick = 0;
            int deltaTick;
            while (true)
            {
                // #region = 코드 범위 할당 = 접었다 폈다 할 수 있음.
                #region # 프레임 관리
                // 시스템 시작 한 후의 전체 경과 시간 (ms)
                currentTick = System.Environment.TickCount;

                // 반복문이 실행된 시간 ( 경과시간 - 마지막 실행시간)
                elapsedTick = currentTick - lastTick;

                // 만약 경과한 시간이 1/30초 보다 작다면
                if (elapsedTick < WAIT_TICK) continue;

                deltaTick = currentTick - lastTick;

                // 새로운 시간 할당
                lastTick = currentTick;
                #endregion

                // 입력

                // 로직
                player.Update(deltaTick);

                // 렌더링
                Console.SetCursorPosition(0, 0); // 커서를 0번째 라인에 0번째 순서로 수정
                board.Render();
            }
        }
    }
}
