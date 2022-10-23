using FormatConverter.Helpers;
using FormatConverter.TreeModel;
using Microsoft.Extensions.Options;

namespace FormatConverter.Output
{
    public class C2Output : IMatchesTreeOutputer
    {
        private readonly AppSettingsOptions _config;
        private readonly ITurnHelper _turnHelper;

        public int OutputFilesCount { get; set; } = 0;

        public C2Output(IOptions<AppSettingsOptions> configOptions, ITurnHelper turnHelper)
        {
            _config = configOptions.Value;
            _turnHelper = turnHelper;
        }

        public int DoOutput(MatchesTreeNode treeRoot)
        {
            var dir = DirectoryHelper.CreateDirThrowIfExists(_config.OutputDir);
            RecursivelyOutputEveryNode(treeRoot.ChildNodes, dir, 1);
            return OutputFilesCount;
        }

        private void RecursivelyOutputEveryNode(List<MatchesTreeNode> childNodes, string parentDir, int nBet)
        {
            foreach (var currChild in childNodes)
            {
                if (currChild.Turn.Action != TurnActionEnum.Fold)
                {
                    OutputNodeStrategyFile(currChild.Turn, parentDir);
                    OutputFilesCount++;
                }
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
            var positionNameOutput = PositionsHelper.GetPlayer(turn.Player.Position, _config.OutputPatterns.PositionNames);
            var folderActionNamesOutput = _config.OutputPatterns.FolderActionNames;
            var nBetText = _config.OutputPatterns.NBetText;
            var newDir = "";

            if (turn.Action == TurnActionEnum.Raise)
            {
                newDir = parentDir + "\\" + positionNameOutput.PositionName + nBet + nBetText;
            }
            else if (turn.Action == TurnActionEnum.Fold || turn.Action == TurnActionEnum.Call || turn.Action == TurnActionEnum.AllIn)
            {
                newDir = parentDir + "\\" + positionNameOutput.PositionName + TurnActionHelper.GetActionString(turn.Action, folderActionNamesOutput);
            }
            else
            {
                throw new Exception("Turn specified action: " + turn.Action + " output was not defined.");
            }

            return DirectoryHelper.CreateDirThrowIfExists(newDir);
        }

        private string OutputNodeStrategyFile(Turn turn, string parentDir)
        {
            var positionNameOutput = PositionsHelper.GetPlayer(turn.Player.Position, _config.OutputPatterns.PositionNames);
            var strategyFilesActionNamesOutput = _config.OutputPatterns.StrategyFileActionNames;
            var newFilePath = parentDir + "\\" + positionNameOutput.PositionName + "_" + TurnActionHelper.GetActionString(turn.Action, strategyFilesActionNamesOutput);
            DirectoryHelper.CreateTxtFileThrowIfExists(newFilePath, turn.Strategy);
            return newFilePath;
        }
    }
}
