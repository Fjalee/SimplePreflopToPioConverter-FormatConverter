using FormatConverter.TreeModel;

namespace FormatConverter.IllegalActions
{
    public interface ITurnsLegalityChecker
    {
        public void ThrowIfAnyPositionWasSkippedInTurn(List<List<Turn>> branches, List<PositionEnum> positionsInUse);
        public void ThrowIfPlayerMovesAfterFold(List<List<Turn>> branches, List<PositionEnum> positionsInUse);
        public void ThrowIfIllegalMove(List<List<Turn>> branches, List<PositionEnum> positionsInUse);
    }
}
