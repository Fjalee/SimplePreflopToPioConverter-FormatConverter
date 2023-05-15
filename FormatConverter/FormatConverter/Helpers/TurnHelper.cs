using FormatConverter.TreeModel;

namespace FormatConverter.Helpers
{
    public class TurnHelper : ITurnHelper
    {
        public double GetTurnRaise(Turn t)
        {
            var raiseAmount = Convert.ToDouble(t.RaiseAmountInBB, CultureHelper.Culture);
            if (raiseAmount == 0)
            {
                throw new Exception("Couldn't convert raise amount string to double");
            }
            return raiseAmount;
        }

        public int GetNBet(int oldNBet, TurnActionEnum action)
        {
            switch (action)
            {
                case TurnActionEnum.Fold or TurnActionEnum.Call or TurnActionEnum.Check:
                    return oldNBet;
                case TurnActionEnum.Raise or TurnActionEnum.AllIn:
                    return ++oldNBet;
                default:
                    throw new Exception("Turn specified action: " + action + " getting nbet was not defined.");
            }
        }
    }
}
