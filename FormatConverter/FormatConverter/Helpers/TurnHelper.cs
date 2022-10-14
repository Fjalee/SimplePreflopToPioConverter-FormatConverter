using FormatConverter.TreeModel;

namespace FormatConverter.Helpers
{
    public class TurnHelper : ITurnHelper
    {
        public double GetTurnRaise(Turn t)
        {
            var raiseAmount = Convert.ToDouble(t.RaiseAmountInBB);
            if (raiseAmount == 0)
            {
                throw new Exception("Couldn't convert raise amount string to double");
            }
            return raiseAmount;
        }
    }
}
