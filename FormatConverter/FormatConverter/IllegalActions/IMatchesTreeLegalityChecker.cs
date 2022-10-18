using FormatConverter.TreeModel;

namespace FormatConverter.IllegalActions
{
    public interface IMatchesTreeLegalityChecker
    {
        void ThrowIfIllegalMove(MatchesTreeNode tree, List<PositionEnum> positionsInUse);
    }
}
