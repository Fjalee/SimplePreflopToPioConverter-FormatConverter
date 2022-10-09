using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FormatConverter
{
    class PositionsMetaData : IPositionsMetaData
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

        public PositionsMetaData(IOptions<PositionNamesOptions> _positionNamesConfig, ILogger<PositionsMetaData> logger)
        {
            if(_positionNamesConfig.Value.SBName == null ||
               _positionNamesConfig.Value.BBName == null ||
               _positionNamesConfig.Value.UTGName == null ||
               _positionNamesConfig.Value.MP1Name == null ||
               _positionNamesConfig.Value.MP2Name == null ||
               _positionNamesConfig.Value.MP3Name == null ||
               _positionNamesConfig.Value.HIJName == null ||
               _positionNamesConfig.Value.COName == null ||
               _positionNamesConfig.Value.BTNName == null)
            {
                logger.LogCritical("Not all positions names were set");
            }

            SB = new Player() { Name = _positionNamesConfig.Value.SBName };
            BB = new Player() { Name = _positionNamesConfig.Value.BBName };
            UTG = new Player() { Name = _positionNamesConfig.Value.UTGName };
            MP1 = new Player() { Name = _positionNamesConfig.Value.MP1Name };
            MP2 = new Player() { Name = _positionNamesConfig.Value.MP2Name };
            MP3 = new Player() { Name = _positionNamesConfig.Value.MP3Name };
            HIJ = new Player() { Name = _positionNamesConfig.Value.HIJName };
            CO = new Player() { Name = _positionNamesConfig.Value.COName };
            BTN = new Player() { Name = _positionNamesConfig.Value.BTNName };
        }
    }
}