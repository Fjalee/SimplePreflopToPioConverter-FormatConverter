using FormatConverter.TreeModel;

namespace FormatConverter.IllegalActions
{
    public interface ILegalityChecker
    {
        void ThrowIfIllegalMove(List<PositionEnum> roundOrder, List<TurnActionEnum> availableActions, Turn t, PositionEnum posInitiatedBiggestRaise, double callAmoount);
        List<TurnActionEnum> GetAvailableActionsAfterMove(TurnActionEnum currMove, List<TurnActionEnum> lastAvailableMoves);
        void SimulateATurn(MatchState currState, Turn currTurn);
    }
}
