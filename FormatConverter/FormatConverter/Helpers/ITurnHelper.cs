using FormatConverter.TreeModel;

namespace FormatConverter.Helpers
{
    public interface ITurnHelper
    {
        double GetTurnRaise(Turn t);
        int GetNBet(int oldNBet, TurnActionEnum action);

    }
}