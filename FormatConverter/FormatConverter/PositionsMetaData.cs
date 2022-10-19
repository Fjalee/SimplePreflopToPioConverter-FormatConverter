namespace FormatConverter
{
    public class PositionsMetaData
    {
        public List<Player> Postions { get; set; }

        public PositionsMetaData(string SBName, string BBName, string UTGName, string MP1Name, string MP2Name, string MP3Name, string HIJName, string COName, string BTNName)
        {
            Postions = new List<Player>()
            {
                new Player(SBName, PositionEnum.SB),
                new Player(BBName, PositionEnum.BB),
                new Player(UTGName, PositionEnum.UTG),
                new Player(MP1Name, PositionEnum.MP1),
                new Player(MP2Name, PositionEnum.MP2),
                new Player(MP3Name, PositionEnum.MP3),
                new Player(HIJName, PositionEnum.HIJ),
                new Player(COName, PositionEnum.CO),
                new Player(BTNName, PositionEnum.BTN)
            };
        }

        public Player GetPlayer(string positionString)
        {
            var player = Postions.FirstOrDefault(p => p.PositionName == positionString);
            return player;
        }

        public Player GetPlayer(PositionEnum positionEnum)
        {
            var player = Postions.FirstOrDefault(p => p.Position == positionEnum);
            return player;
        }
    }
}