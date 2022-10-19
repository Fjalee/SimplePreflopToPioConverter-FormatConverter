namespace FormatConverter.TreeModel
{
    public class MatchesTreeNode
    {
        public Turn? Turn { get; set; }
        public List<MatchesTreeNode> ChildNodes { get; set; } = new List<MatchesTreeNode>();
        public MatchesTreeNode? ParentNode { get; set; }

        public MatchesTreeNode(Turn? turn)
        {
            Turn = turn;
        }
    }
}
