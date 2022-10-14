using FormatConverter.Helpers;
using FormatConverter.TreeModel;

namespace FormatConverter.IllegalActions
{
    public class LegalityChecker : ILegalityChecker
    {
        private readonly ITurnHelper _turnsHelper;

        public LegalityChecker(ITurnHelper turnsHelper)
        {
            _turnsHelper = turnsHelper;
        }

        public void ThrowIfIllegalMove(List<PositionEnum> roundOrder, List<TurnActionEnum> availableActions, Turn t, PositionEnum posInitiatedBiggestRaise, double callAmoount)
        {
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
            //Checks if person initiated Allin doesnt move anymore
            if (t.Player.Position == posInitiatedBiggestRaise
                && !availableActions.Contains(TurnActionEnum.Raise))
            {
                throw new Exception("Moved after everyone Folded or AllIn'ed");
            }
            //Checks if raises more than last raise
            if (t.Action == TurnActionEnum.Raise && _turnsHelper.GetTurnRaise(t) <= callAmoount)
            {
                throw new Exception("Illegal action " + t.Action);
            }
            //Checks if only Raise and AllIn have RaiseAmount
            if ((t.Action == TurnActionEnum.Fold || t.Action == TurnActionEnum.Call || t.Action == TurnActionEnum.Check)
                && !String.IsNullOrEmpty(t.RaiseAmountInBB))
            {
                throw new Exception(t.Action + " action can't have raise amount " + t.RaiseAmountInBB);
            }
        }

        public List<TurnActionEnum> GetAvailableActionsAfterMove(TurnActionEnum currMove, List<TurnActionEnum> lastAvailableMoves)
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
    }
}
