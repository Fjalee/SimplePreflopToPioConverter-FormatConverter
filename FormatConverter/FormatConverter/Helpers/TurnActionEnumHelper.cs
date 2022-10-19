using FormatConverter.TreeModel;

namespace FormatConverter.Helpers
{
    public static class TurnActionEnumHelper
    {
        public static bool OnlyContainsFoldAndCheck(List<TurnActionEnum> currList)
        {
            var callFoldList = new List<TurnActionEnum>() { TurnActionEnum.Call, TurnActionEnum.Fold };
            return currList.All(callFoldList.Contains);
        }
    }
}
