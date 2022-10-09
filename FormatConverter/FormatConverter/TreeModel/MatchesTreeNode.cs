namespace FormatConverter.TreeModel
{
    public class MatchesTreeNode
    {
        public Turn Turn { get; set; }
        public MatchesTreeNode? NextTurn { get; set; }

        public MatchesTreeNode(Turn turn, MatchesTreeNode? nextTurn)
        {
            Turn = turn;
            NextTurn = nextTurn;
        }
    }
}
