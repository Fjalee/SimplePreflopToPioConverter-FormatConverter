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
            DirectoryHelper.CreateDirThrowIfExists(_config.OutputDir);
            RecursivelyOutputEveryNode(treeRoot.ChildNodes);
        }

        private void RecursivelyOutputEveryNode(List<MatchesTreeNode> childNodes)
        {
            foreach (var currChild in childNodes)
            {
                OutputNode();
                if (currChild.ChildNodes.Count > 0)
                {
                    RecursivelyOutputEveryNode(currChild.ChildNodes);
                }
            }
        }

        private void OutputNode()
        {

        }
    }
}
