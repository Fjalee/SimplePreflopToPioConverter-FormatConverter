using FormatConverter.TreeModel;
using Microsoft.Extensions.Options;

namespace FormatConverter.Output
{
    public class C2Output : IMatchesTreeOutputer
    {
        private readonly AppSettingsOptions _config;

        public C2Output(IOptions<AppSettingsOptions> configOptions)
        {
            _config = configOptions.Value;
        }

        public void DoOutput(MatchesTreeNode treeRoot)
        {
            var dir = DirectoryHelper.CreateDirThrowIfExists(_config.OutputDir);
            RecursivelyOutputEveryNode(treeRoot.ChildNodes, dir);
        }

        private void RecursivelyOutputEveryNode(List<MatchesTreeNode> childNodes, string parentDir)
        {
            foreach (var currChild in childNodes)
            {
                if (currChild.ChildNodes.Count > 0)
                {
                    var newDir = OutputNode(currChild.Turn, parentDir);
                    RecursivelyOutputEveryNode(currChild.ChildNodes, newDir);
                }
            }
        }

        private string OutputNode(Turn turn, string parentDir)
        {
            var positionNameOutput = Program.OutputPositionsMetaData.GetPlayer(turn.Player.Position);
            var actionNamesOutput = _config.OutputPatterns.ActionNames;
            var newDir = "";
            switch (turn.Action)
            {
                case TurnActionEnum.Fold:
                    newDir = parentDir + "\\" + positionNameOutput.PositionName + actionNamesOutput.Fold;
                    break;
                case TurnActionEnum.Call:
                    newDir = parentDir + "\\" + positionNameOutput.PositionName + actionNamesOutput.Call;
                    break;
                case TurnActionEnum.Raise or TurnActionEnum.AllIn:
                    newDir = parentDir + "\\" + positionNameOutput.PositionName + actionNamesOutput.Raise;
                    break;
                default:
                    throw new Exception("Turn specified action: " + turn.Action + " output was not defined.");
            }

            return DirectoryHelper.CreateDirThrowIfExists(newDir);
        }
    }
}
