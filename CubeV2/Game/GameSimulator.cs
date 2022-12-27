using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class GameSimulator
    {
        public static int Simulate(UserInput input,BoardTemplateTemplate template,BoardWinCondition winCon, int timeout,int iterations)
        {
            var oldMode = BoardCallback.Mode;
            //TODO uhmmmmmm
            BoardCallback.Mode = GamePlaybackMode.Headless;

            int numWins = 0;

            for(int i=0;i<iterations;i++)
            {
                var board = template.GenerateTemplate().GenerateBoard();
                BoardCallback.HeadlessBoard = board;

                for(int j=0;j<timeout;j++)
                {
                    board.Tick(input);

                    if(winCon.Check(board))
                    {
                        numWins++;
                        break;
                    }
                }
            }

            BoardCallback.Mode = oldMode;
            return numWins;
        }
    }
}
