using Microsoft.Extensions.Options;

namespace FormatConverter
{
    class OutputPositionsMetaData : IOutputPositionsMetaData
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

        public OutputPositionsMetaData(IOptions<OutputPositionNamesOptions> _outputPositionNamesConfig)
        {
            if(_outputPositionNamesConfig.Value.SBName == null ||
               _outputPositionNamesConfig.Value.BBName == null ||
               _outputPositionNamesConfig.Value.UTGName == null ||
               _outputPositionNamesConfig.Value.MP1Name == null ||
               _outputPositionNamesConfig.Value.MP2Name == null ||
               _outputPositionNamesConfig.Value.MP3Name == null ||
               _outputPositionNamesConfig.Value.HIJName == null ||
               _outputPositionNamesConfig.Value.COName == null ||
               _outputPositionNamesConfig.Value.BTNName == null)
            {
                throw new ArgumentException("Not all positions names were set");
            }

            SB = new Player() { Name = _outputPositionNamesConfig.Value.SBName };
            BB = new Player() { Name = _outputPositionNamesConfig.Value.BBName };
            UTG = new Player() { Name = _outputPositionNamesConfig.Value.UTGName };
            MP1 = new Player() { Name = _outputPositionNamesConfig.Value.MP1Name };
            MP2 = new Player() { Name = _outputPositionNamesConfig.Value.MP2Name };
            MP3 = new Player() { Name = _outputPositionNamesConfig.Value.MP3Name };
            HIJ = new Player() { Name = _outputPositionNamesConfig.Value.HIJName };
            CO = new Player() { Name = _outputPositionNamesConfig.Value.COName };
            BTN = new Player() { Name = _outputPositionNamesConfig.Value.BTNName };
        }
    }
}