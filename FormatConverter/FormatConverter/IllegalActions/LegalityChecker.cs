using FormatConverter.Helpers;
using FormatConverter.TreeModel;

namespace FormatConverter.IllegalActions
{
    public class LegalityChecker : ILegalityChecker
    {
        private readonly ITurnHelper _turnsHelper;

        public LegalityChecker(ITurnHelper turnsHelper)
        {
            _turnsHelper = turnsHelper;
        }

        //Checks if moevs legal
        //Changes state for the next turn
        public void SimulateATurn(MatchState currState, Turn currTurn)
        {
            //Person who initiated biggest raise has their move again
            if (currTurn.Player.Position == currState.PosInitiatedBiggestRaise)
            {
                currState.AvailableActions = GetAvailableActionsAfterMove(TurnActionEnum.Check, currState.AvailableActions);
            }

            ThrowIfIllegalMove(currState.RoundOrder, currState.AvailableActions, currTurn, currState.PosInitiatedBiggestRaise, currState.CallAmount);

            if (currTurn.Action == TurnActionEnum.Fold)
            {
                currState.PositionsStillInPlay.Remove(currTurn.Player.Position);
            }
            else
            {
                //Person who initiated biggest raise has their move again
                if (currTurn.Player.Position == currState.PosInitiatedBiggestRaise)
                {
                    currState.CallAmount = 1.0;
                    currState.AvailableActions = GetAvailableActionsAfterMove(currTurn.Action, currState.AvailableActions);
                    currState.PosInitiatedBiggestRaise = currTurn.Player.Position;
                }
                else
                {
                    //If initiates biggest raise
                    if (currTurn.Action == TurnActionEnum.Raise
                        || (currTurn.Action == TurnActionEnum.AllIn && currState.AvailableActions.Contains(TurnActionEnum.Raise)))
                    {
                        currState.PosInitiatedBiggestRaise = currTurn.Player.Position;
                        currState.AvailableActions = GetAvailableActionsAfterMove(currTurn.Action, currState.AvailableActions);
                    }
                }
                if (currTurn.Action == TurnActionEnum.Raise || currTurn.Action == TurnActionEnum.AllIn)
                {
                    currState.CallAmount = Convert.ToDouble(currTurn.RaiseAmountInBB, CultureHelper.Culture);
                }
            }

            if (currTurn.Action == TurnActionEnum.AllIn
                || TurnActionHelper.OnlyContainsFoldAndCheck(currState.AvailableActions))
            {
                currState.PositionsStillInPlay.Remove(currTurn.Player.Position);
            }

            currState.RoundOrder.Remove(currTurn.Player.Position);
            if (currState.RoundOrder.Count == 0)
            {
                currState.RoundOrder = currState.PositionsStillInPlay.ToList();
            }
            currState.TurnsDone++;
        }

        public void ThrowIfIllegalMove(List<PositionEnum> roundOrder, List<TurnActionEnum> availableActions, Turn t, PositionEnum posInitiatedBiggestRaise, double callAmoount)
        {
            //Checks turn order
            if (t.Player.Position != roundOrder.First())
            {
                throw new Exception(t.Player.Position + " moved instead of " + roundOrder.First());
            }
            //Checks if legal move
            if (!availableActions.Contains(t.Action))
            {
                throw new Exception("Illegal action " + t.Action);
            }
            //Checks if person initiated Allin doesnt move anymore
            if (t.Player.Position == posInitiatedBiggestRaise
                && !availableActions.Contains(TurnActionEnum.Raise))
            {
                throw new Exception("Moved after everyone Folded or AllIn'ed");
            }
            //Checks if raises more than last raise
            if (t.Action == TurnActionEnum.Raise && _turnsHelper.GetTurnRaise(t) <= callAmoount)
            {
                throw new Exception("Illegal action " + t.Action);
            }
            //Checks if only Raise and AllIn have RaiseAmount
            if ((t.Action == TurnActionEnum.Fold || t.Action == TurnActionEnum.Call || t.Action == TurnActionEnum.Check)
                && !String.IsNullOrEmpty(t.RaiseAmountInBB))
            {
                throw new Exception(t.Action + " action can't have raise amount " + t.RaiseAmountInBB);
            }
        }

        public List<TurnActionEnum> GetAvailableActionsAfterMove(TurnActionEnum currMove, List<TurnActionEnum> lastAvailableMoves)
        {
            if (currMove == TurnActionEnum.Check)
            {
                return new List<TurnActionEnum>() { TurnActionEnum.Check, TurnActionEnum.Raise, TurnActionEnum.AllIn, TurnActionEnum.Fold };
            }
            if (currMove == TurnActionEnum.AllIn)
            {
                return new List<TurnActionEnum>() { TurnActionEnum.Call, TurnActionEnum.Fold };
            }
            if (currMove == TurnActionEnum.Raise)
            {
                return new List<TurnActionEnum>() { TurnActionEnum.Call, TurnActionEnum.Raise, TurnActionEnum.AllIn, TurnActionEnum.Fold };
            }

            if (lastAvailableMoves == null)
            {
                throw new NullReferenceException();
            }

            return lastAvailableMoves;
        }
    }
}
