using FormatConverter.TreeModel;

namespace FormatConverter.Helpers
{
    public static class TurnActionHelper
    {
        public static bool OnlyContainsFoldAndCheck(List<TurnActionEnum> currList)
        {
            var callFoldList = new List<TurnActionEnum>() { TurnActionEnum.Call, TurnActionEnum.Fold };
            return currList.All(callFoldList.Contains);
        }


        public static string GetActionString(TurnActionEnum action, OutputActionNamesOptions posNames)
        {
            switch(action)
            {
                case TurnActionEnum.Fold:
                    return posNames.Fold;
                case TurnActionEnum.Check:
                    return posNames.Check;
                case TurnActionEnum.Call:
                    return posNames.Call;
                case TurnActionEnum.Raise:
                    return posNames.Raise;
                case TurnActionEnum.AllIn:
                    return posNames.AllIn;
                default:
                    throw new ArgumentException("TurnActionEnum: " + action + ", was not declared in TurnActionHelper"); 
            }
        }
    }
}
