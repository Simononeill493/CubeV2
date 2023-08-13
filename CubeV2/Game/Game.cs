using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CubeV2
{
    public abstract class Game
    {
        public bool GameWon => WinCondition.Check(CurrentBoard);
        public BoardWinCondition WinCondition = BoardWinCondition.None;

        public List<Instruction> KnownInstructions = new List<Instruction>();

        public Entity FocusEntity { get; private set; }
        public Board CurrentBoard { get; private set; }
        public BoardTemplate CurrentTemplate { get; private set; }
        public BoardTemplateTemplate CurrentTemplateTemplate { get; private set; }

        public void SetFocusEntity(Entity p) => FocusEntity = p;
        public void ClearFocusEntity() => FocusEntity = null;

        public void SetBoard(Board b) 
        {
            AnimationTracker.Reset();

            CurrentBoard = b;
            SetFocusEntity(CurrentBoard.GetActiveEntityByTag(Config.PlayerTag).FirstOrDefault());
            CustomSetUpBoard(b);
        }

        public void UnsetBoard()
        {
            CurrentBoard = null;
            ClearFocusEntity();
        }

        public void ResetBoard() => SetBoard(CurrentTemplate.GenerateBoard());
        public void TickBoard(UserInput input)
        {
            CurrentBoard.Tick(input);
        }

        public void SetTemplateTemplate(BoardTemplateTemplate templateTemplate) => CurrentTemplateTemplate = templateTemplate;
        public void ResetBoardTemplate() => CurrentTemplate = CurrentTemplateTemplate.GenerateTemplate();

        public Game()
        {
            if (Config.KnowAllInstructionsByDefault)
            {
                KnownInstructions = InstructionDatabase.GetAll();
            }
        }

        public abstract BoardTemplateTemplate CreateTemplateTemplate();
        public virtual void CustomSetUpBoard(Board b) { }

    }
}
