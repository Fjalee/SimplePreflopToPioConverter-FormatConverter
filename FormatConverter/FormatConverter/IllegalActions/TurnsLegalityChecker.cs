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
                var currState = new MatchState(
                    positionsInUse.ToList().ToList(),
                    _legalityChecker.GetAvailableActionsAfterMove(TurnActionEnum.Raise, null),
                    PositionEnum.BB,
                    1.0,
                    positionsInUse.ToList());

                foreach (var t in turns)
                {
                    _legalityChecker.SimulateATurn(currState, t);
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
