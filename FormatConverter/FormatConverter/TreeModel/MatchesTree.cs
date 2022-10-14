namespace FormatConverter.TreeModel
{
    public class MatchesTree
    {
        public List<MatchesTreeNode> Nodes { get; set; }

        public MatchesTree(List<MatchesTreeNode> nodes)
        {
            Nodes = nodes;
        }
    }
}
