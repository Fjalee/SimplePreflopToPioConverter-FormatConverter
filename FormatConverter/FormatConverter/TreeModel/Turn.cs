namespace FormatConverter.TreeModel
{
    public class Turn
    {
        public Player Player { get; set; }
        public TurnActionEnum Action { get; set; }
        public string Strategy { get; set; }
        public string? RaiseAmountInBB { get; set; }

        public Turn(Player player, TurnActionEnum action, string strategy)
        {
            Player = player;
            Action = action;
            Strategy = strategy;
        }
    }
}