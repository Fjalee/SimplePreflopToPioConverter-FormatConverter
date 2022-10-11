using FormatConverter.TreeModel;

namespace FormatConverter.IllegalActions
{
    public class LegalityChecker : ILegalityChecker
    {
        public LegalityChecker()
        {

        }

        public void ThrowIfIllegalMove(List<List<Turn>> branches, List<PositionEnum> positionsInUse)
        {
            foreach (var turns in branches)
            {
                var positionsStillInPlay = positionsInUse.ToList();
                var roundOrder = positionsStillInPlay.ToList();
                var availableActions = GetAvailableActionsAfterMove(TurnActionEnum.Raise, null);
                var posInitiatedBiggestRaise = PositionEnum.BB;
                var callAmoount = 1.0;

                foreach (var t in turns)
                {
                    //Person who initiated biggest raise has their move again
                    if (t.Player.Position == posInitiatedBiggestRaise)
                    {
                        availableActions = GetAvailableActionsAfterMove(TurnActionEnum.Check, availableActions);
                    }
                    //Checks if raises more than last raise
                    if (t.RaiseAmountInBB != null)
                    {
                        var raiseAmount = Convert.ToDouble(t.RaiseAmountInBB);
                        if (raiseAmount == 0)
                        {
                            throw new Exception("Couldn't convert raise amount string to double");
                        }
                        if (t.Action == TurnActionEnum.Raise && raiseAmount <= callAmoount)
                        {
                            throw new Exception("Illegal action " + t.Action);
                        }
                    }

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
                    //Checks if only Raise and AllIn have RaiseAmount
                    if ((t.Action == TurnActionEnum.Fold || t.Action == TurnActionEnum.Call || t.Action == TurnActionEnum.Check)
                        && !String.IsNullOrEmpty(t.RaiseAmountInBB))
                    {
                        throw new Exception(t.Action + " action can't have raise amount " + t.RaiseAmountInBB);
                    }
                    //Checks if person initiated Allin doesnt move anymore
                    if (t.Player.Position == posInitiatedBiggestRaise
                        && !availableActions.Contains(TurnActionEnum.Raise))
                    {
                        throw new Exception("Moved after everyone Folded or AllIn'ed");
                    }

                    if (t.Action == TurnActionEnum.Fold)
                    {
                        positionsStillInPlay.Remove(t.Player.Position);
                    }
                    else
                    {
                        //Person who initiated biggest raise has their move again
                        if (t.Player.Position == posInitiatedBiggestRaise)
                        {
                            callAmoount = 1.0;
                            availableActions = GetAvailableActionsAfterMove(t.Action, availableActions);
                            posInitiatedBiggestRaise = t.Player.Position;
                        }
                        else
                        {
                            //If initiates biggest raise
                            if (t.Action == TurnActionEnum.Raise
                                || (t.Action == TurnActionEnum.AllIn && availableActions.Contains(TurnActionEnum.Raise)))
                            {
                                posInitiatedBiggestRaise = t.Player.Position;
                                availableActions = GetAvailableActionsAfterMove(t.Action, availableActions);
                            }
                        }
                        if (t.Action == TurnActionEnum.Raise || t.Action == TurnActionEnum.AllIn)
                        {
                            callAmoount = Convert.ToDouble(t.RaiseAmountInBB);
                        }
                    }

                    roundOrder.Remove(t.Player.Position);
                    if (roundOrder.Count == 0)
                    {
                        roundOrder = positionsStillInPlay.ToList();
                    }
                }
            }
        }

        private List<TurnActionEnum> GetAvailableActionsAfterMove(TurnActionEnum currMove, List<TurnActionEnum> lastAvailableMoves)
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

        public void ThrowIfPlayerMovesAfterFold(List<List<Turn>> branches, List<PositionEnum> positionsInUse)
        {
            foreach (var turns in branches)
            {
                var currPositions = positionsInUse.ToList();
                foreach (var t in turns)
                {
                    if (t.Action == TurnActionEnum.Fold)
                    {
                        if (currPositions.Contains(t.Player.Position))
                        {
                            currPositions.Remove(t.Player.Position);
                        }
                        else
                        {
                            throw new Exception(t.Player.Position + " moved after a fold.");
                        }
                    }
                }
            }
        }

        public void ThrowIfAnyPositionWasSkippedInTurn(List<List<Turn>> branches, List<PositionEnum> positionsInUse)
        {
            foreach (var turns in branches)
            {
                var currPositions = positionsInUse.ToList();
                foreach (var t in turns)
                {
                    if (currPositions.Count() <= 0)
                    {
                        break;
                    }
                    if (currPositions.First() == t.Player.Position)
                    {
                        currPositions.Remove(t.Player.Position);
                    }
                    else
                    {
                        throw new Exception(currPositions.First() + " position wasn't found in a branch.");
                    }
                }
            }
        }
    }
}
