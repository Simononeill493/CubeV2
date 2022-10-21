using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CubeV2
{
    internal class Game
    {
        public bool GameWon => WinCondition.Check(CurrentBoard);
        public BoardWinCondition WinCondition = BoardWinCondition.None;

        public Board CurrentBoard { get; private set; }
        private BoardTemplate CurrentTemplate;
        private BoardTemplateTemplate CurrentTemplateTemplate;

        public void SetTemplateTemplate(BoardTemplateTemplate templateTemplate) => CurrentTemplateTemplate = templateTemplate;
        public void ResetBoardTemplate() => CurrentTemplate = CurrentTemplateTemplate.GenerateTemplate();
        public void ResetBoard() => SetBoard(CurrentTemplate.GenerateBoard());

        public void SetBoard(Board b) => CurrentBoard = b;
        public void TickBoard() => CurrentBoard.Tick();

        public List<Instruction> KnownInstructions = new List<Instruction>();

        public Game()
        {
            if (Config.KnowAllInstructionsByDefault)
            {
                KnownInstructions = InstructionDatabase.GetAll();
            }
        }
    }
}
