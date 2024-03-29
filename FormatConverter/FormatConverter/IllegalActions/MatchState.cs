﻿using FormatConverter.TreeModel;

namespace FormatConverter.IllegalActions
{
    public class MatchState
    {
        public List<PositionEnum> PositionsStillInPlay { get; set; }
        public List<PositionEnum> RoundOrder { get; set; }
        public List<TurnActionEnum> AvailableActions { get; set; }
        public PositionEnum PosInitiatedBiggestRaise { get; set; }
        public double CallAmount { get; set; }
        public int TurnsDone { get; set; }

        public MatchState(List<PositionEnum> roundOrder, List<TurnActionEnum> availableActions,
            PositionEnum posInitiatedBiggestRaise, double callAmount, List<PositionEnum> positionsStillInPlay, int turnsDone=0)
        {
            PositionsStillInPlay = positionsStillInPlay;
            RoundOrder = roundOrder;
            AvailableActions = availableActions;
            PosInitiatedBiggestRaise = posInitiatedBiggestRaise;
            CallAmount = callAmount;
            TurnsDone = turnsDone;
        }

        public MatchState(MatchState state)
        {
            PositionsStillInPlay = state.PositionsStillInPlay.ToList();
            RoundOrder = state.RoundOrder.ToList();
            AvailableActions = state.AvailableActions.ToList();
            PosInitiatedBiggestRaise = state.PosInitiatedBiggestRaise;
            CallAmount = state.CallAmount;
            TurnsDone = state.TurnsDone;
        }
    }
}
