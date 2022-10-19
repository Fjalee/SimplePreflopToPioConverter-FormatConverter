using FormatConverter.TreeModel;

namespace FormatConverter.IllegalActions
{
    public class MatchesTreeLegalityChecker : IMatchesTreeLegalityChecker
    {
        private readonly ILegalityChecker _legalityChecker;

        public MatchesTreeLegalityChecker(ILegalityChecker legalityChecker)
        {
            _legalityChecker = legalityChecker;
        }

        public void ThrowIfIllegalMove(MatchesTreeNode root, List<PositionEnum> positionsInUse)
        {
            var currState = new MatchState(
                positionsInUse.ToList().ToList(),
                _legalityChecker.GetAvailableActionsAfterMove(TurnActionEnum.Raise, null),
                PositionEnum.BB,
                1.0,
                positionsInUse.ToList());

            RecursivelySimulateEveryTurn(root.ChildNodes, currState);
        }

        private void RecursivelySimulateEveryTurn(List<MatchesTreeNode> childNodes, MatchState currState)
        {
            foreach (var currChild in childNodes)
            {
                var cloneState = new MatchState(currState);
                _legalityChecker.SimulateATurn(cloneState, currChild.Turn);
                if (currChild.ChildNodes.Count > 0)
                {
                    RecursivelySimulateEveryTurn(currChild.ChildNodes, cloneState);
                }
            }
        }
    }
}
