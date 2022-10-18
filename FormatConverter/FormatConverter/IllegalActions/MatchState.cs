using FormatConverter.TreeModel;

namespace FormatConverter.IllegalActions
{
    public class MatchState
    {
        public List<PositionEnum> PositionsStillInPlay { get; set; }
        public List<PositionEnum> RoundOrder { get; set; }
        public List<TurnActionEnum> AvailableActions { get; set; }
        public PositionEnum PosInitiatedBiggestRaise { get; set; }
        public double CallAmount { get; set; }

        public MatchState(List<PositionEnum> roundOrder, List<TurnActionEnum> availableActions,
            PositionEnum posInitiatedBiggestRaise, double callAmount, List<PositionEnum> positionsStillInPlay)
        {
            PositionsStillInPlay = positionsStillInPlay;
            RoundOrder = roundOrder;
            AvailableActions = availableActions;
            PosInitiatedBiggestRaise = posInitiatedBiggestRaise;
            CallAmount = callAmount;
        }

        public MatchState(MatchState state)
        {
            PositionsStillInPlay = state.PositionsStillInPlay;
            RoundOrder = state.RoundOrder;
            AvailableActions = state.AvailableActions;
            PosInitiatedBiggestRaise = state.PosInitiatedBiggestRaise;
            CallAmount = state.CallAmount;
        }
    }
}
