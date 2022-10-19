using FormatConverter.TreeModel;

namespace FormatConverter.IllegalActions
{
    public interface IMatchesTreeLegalityChecker
    {
        void ThrowIfIllegalMove(MatchesTreeNode root, List<PositionEnum> positionsInUse);
    }
}
