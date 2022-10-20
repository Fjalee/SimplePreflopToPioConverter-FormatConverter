namespace FormatConverter.InputFile
{
    public class TurnBranchInPairs
    {
        public List<PlayerAndActionStringPair> Pairs { get; set; }
        public string Strategy { get; set; }

        public TurnBranchInPairs(List<PlayerAndActionStringPair> pairs, string strategy)
        {
            Pairs = pairs;
            Strategy = strategy;
        }
    }
}
