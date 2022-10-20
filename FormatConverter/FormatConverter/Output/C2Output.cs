using FormatConverter.TreeModel;

namespace FormatConverter.Output
{
    public class C2Output : IMatchesTreeOutputer
    {
        public void DoOutput(MatchesTreeNode treeRoot)
        {
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
