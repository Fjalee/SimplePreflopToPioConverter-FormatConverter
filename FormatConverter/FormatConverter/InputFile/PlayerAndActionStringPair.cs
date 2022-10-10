namespace FormatConverter.InputFile
{
    public class PlayerAndActionStringPair
    {
        public string Player { get; set; }
        public string Action { get; set; }

        public PlayerAndActionStringPair(string player, string action)
        {
            Player = player;
            Action = action;
        }
    }
}
