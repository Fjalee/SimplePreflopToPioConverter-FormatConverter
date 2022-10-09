namespace FormatConverter.TreeModel
{
    public class MatchesTree
    {
        public List<MatchesTreeNode> PossibleTurns { get; set; }

        public MatchesTree(List<MatchesTreeNode> possibleTurns)
        {
            PossibleTurns = possibleTurns;
        }
    }
}
