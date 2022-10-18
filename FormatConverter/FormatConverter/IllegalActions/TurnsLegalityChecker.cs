using FormatConverter.TreeModel;

namespace FormatConverter.IllegalActions
{
    public class TurnsLegalityChecker : ITurnsLegalityChecker
    {
        private readonly ILegalityChecker _legalityChecker;

        public TurnsLegalityChecker(ILegalityChecker legalityChecker)
        {
            _legalityChecker = legalityChecker;
        }

        public void ThrowIfIllegalMove(List<List<Turn>> branches, List<PositionEnum> positionsInUse)
        {
            foreach (var turns in branches)
            {
                var positionsStillInPlay = positionsInUse.ToList();
                var roundOrder = positionsStillInPlay.ToList();
                var availableActions = _legalityChecker.GetAvailableActionsAfterMove(TurnActionEnum.Raise, null);
                var posInitiatedBiggestRaise = PositionEnum.BB;
                var callAmount = 1.0;

                foreach (var t in turns)
                {
                    //Person who initiated biggest raise has their move again
                    if (t.Player.Position == posInitiatedBiggestRaise)
                    {
                        availableActions = _legalityChecker.GetAvailableActionsAfterMove(TurnActionEnum.Check, availableActions);
                    }

                    _legalityChecker.ThrowIfIllegalMove(roundOrder, availableActions, t, posInitiatedBiggestRaise, callAmount);

                    if (t.Action == TurnActionEnum.Fold)
                    {
                        positionsStillInPlay.Remove(t.Player.Position);
                    }
                    else
                    {
                        //Person who initiated biggest raise has their move again
                        if (t.Player.Position == posInitiatedBiggestRaise)
                        {
                            callAmount = 1.0;
                            availableActions = _legalityChecker.GetAvailableActionsAfterMove(t.Action, availableActions);
                            posInitiatedBiggestRaise = t.Player.Position;
                        }
                        else
                        {
                            //If initiates biggest raise
                            if (t.Action == TurnActionEnum.Raise
                                || (t.Action == TurnActionEnum.AllIn && availableActions.Contains(TurnActionEnum.Raise)))
                            {
                                posInitiatedBiggestRaise = t.Player.Position;
                                availableActions = _legalityChecker.GetAvailableActionsAfterMove(t.Action, availableActions);
                            }
                        }
                        if (t.Action == TurnActionEnum.Raise || t.Action == TurnActionEnum.AllIn)
                        {
                            callAmount = Convert.ToDouble(t.RaiseAmountInBB);
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

        //Legacy
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

        //Legacy
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
