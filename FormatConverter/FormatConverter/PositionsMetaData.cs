namespace FormatConverter
{
    public class PositionsMetaData
    {
        public Player SB { get; set; }
        public Player BB { get; set; }
        public Player UTG { get; set; }
        public Player MP1 { get; set; }
        public Player MP2 { get; set; }
        public Player MP3 { get; set; }
        public Player HIJ { get; set; }
        public Player CO { get; set; }
        public Player BTN { get; set; }

        public PositionsMetaData(string SBName, string BBName, string UTGName, string MP1Name, string MP2Name, string MP3Name, string HIJName, string COName, string BTNName)
        {
            SB = new Player(SBName, PositionEnum.SB);
            BB = new Player(BBName, PositionEnum.BB);
            UTG = new Player(UTGName, PositionEnum.UTG);
            MP1 = new Player(MP1Name, PositionEnum.MP1);
            MP2 = new Player(MP2Name, PositionEnum.MP2);
            MP3 = new Player(MP3Name, PositionEnum.MP3);
            HIJ = new Player(HIJName, PositionEnum.HIJ);
            CO = new Player(COName, PositionEnum.CO);
            BTN = new Player(BTNName, PositionEnum.BTN);
        }
    }
}