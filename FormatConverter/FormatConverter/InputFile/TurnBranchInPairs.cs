namespace FormatConverter.InputFile
{
    public class TurnBranchInPairs
    {
        public PlayerAndActionStringPair Pair { get; set; }
        public string PathToFile { get; set; }

        public TurnBranchInPairs(PlayerAndActionStringPair pair, string pathToFile)
        {
            Pair = pair;
            PathToFile = pathToFile;
        }
    }
}
