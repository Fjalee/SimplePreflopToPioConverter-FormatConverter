using FormatConverter.Helpers;
using FormatConverter.TreeModel;
using Microsoft.Extensions.Options;

namespace FormatConverter.Output
{
    public class C2Output : IMatchesTreeOutputer
    {
        private readonly AppSettingsOptions _config;
        private readonly ITurnHelper _turnHelper;

        public C2Output(IOptions<AppSettingsOptions> configOptions, ITurnHelper turnHelper)
        {
            _config = configOptions.Value;
            _turnHelper = turnHelper;
        }

        public void DoOutput(MatchesTreeNode treeRoot)
        {
            var dir = DirectoryHelper.CreateDirThrowIfExists(_config.OutputDir);
            RecursivelyOutputEveryNode(treeRoot.ChildNodes, dir, 1);
        }

        private void RecursivelyOutputEveryNode(List<MatchesTreeNode> childNodes, string parentDir, int nBet)
        {
            foreach (var currChild in childNodes)
            {
                if (currChild.ChildNodes.Count > 0)
                {
                    var newNBet = _turnHelper.GetNBet(nBet, currChild.Turn.Action);
                    var newDir = OutputNode(currChild.Turn, parentDir, newNBet);
                    RecursivelyOutputEveryNode(currChild.ChildNodes, newDir, newNBet);
                }
            }
        }

        private string OutputNode(Turn turn, string parentDir, int nBet)
        {
            var positionNameOutput = Program.OutputPositionsMetaData.GetPlayer(turn.Player.Position);
            var actionNamesOutput = _config.OutputPatterns.ActionNames;
            var nBetText = _config.OutputPatterns.NBetText;
            var bbText = _config.OutputPatterns.AmountCurrency;
            var newFilePath = "";
            var newDir = "";

            switch (turn.Action)
            {
                case TurnActionEnum.Fold:
                    newDir = parentDir + "\\" + positionNameOutput.PositionName + actionNamesOutput.Fold;
                    break;
                case TurnActionEnum.Call:
                    newDir = parentDir + "\\" + positionNameOutput.PositionName + actionNamesOutput.Call;
                    newFilePath = parentDir + "\\" + positionNameOutput.PositionName + "_" + actionNamesOutput.Call;
                    DirectoryHelper.CreateTxtFileThrowIfExists(newFilePath, turn.Strategy);
                    break;
                case TurnActionEnum.Raise:
                    newDir = parentDir + "\\" + positionNameOutput.PositionName + nBet + nBetText +
                        "_" + turn.RaiseAmountInBB + bbText;
                    newFilePath = parentDir + "\\" + positionNameOutput.PositionName +
                        "_" + actionNamesOutput.Raise + "_" + turn.RaiseAmountInBB + bbText;
                    DirectoryHelper.CreateTxtFileThrowIfExists(newFilePath, turn.Strategy);
                    break;
                case TurnActionEnum.AllIn:
                    newDir = parentDir + "\\" + positionNameOutput.PositionName + nBet + nBetText
                        + "_" + actionNamesOutput.AllIn;
                    newFilePath = parentDir + "\\" + positionNameOutput.PositionName
                        + "_" + actionNamesOutput.Raise + "_" + actionNamesOutput.AllIn;
                    DirectoryHelper.CreateTxtFileThrowIfExists(newFilePath, turn.Strategy);
                    break;
                default:
                    throw new Exception("Turn specified action: " + turn.Action + " output was not defined.");
            }

            return DirectoryHelper.CreateDirThrowIfExists(newDir);
        }
    }
}
