using Microsoft.Extensions.Options;

namespace FormatConverter
{
    class InputPositionsMetaData : IInputPositionsMetaData
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

        public InputPositionsMetaData(IOptions<InputPositionNamesOptions> _inputPositionNamesConfig)
        {
            if(_inputPositionNamesConfig.Value.SBName == null ||
               _inputPositionNamesConfig.Value.BBName == null ||
               _inputPositionNamesConfig.Value.UTGName == null ||
               _inputPositionNamesConfig.Value.MP1Name == null ||
               _inputPositionNamesConfig.Value.MP2Name == null ||
               _inputPositionNamesConfig.Value.MP3Name == null ||
               _inputPositionNamesConfig.Value.HIJName == null ||
               _inputPositionNamesConfig.Value.COName == null ||
               _inputPositionNamesConfig.Value.BTNName == null)
            {
                throw new ArgumentException("Not all positions names were set");
            }

            SB = new Player() { Name = _inputPositionNamesConfig.Value.SBName };
            BB = new Player() { Name = _inputPositionNamesConfig.Value.BBName };
            UTG = new Player() { Name = _inputPositionNamesConfig.Value.UTGName };
            MP1 = new Player() { Name = _inputPositionNamesConfig.Value.MP1Name };
            MP2 = new Player() { Name = _inputPositionNamesConfig.Value.MP2Name };
            MP3 = new Player() { Name = _inputPositionNamesConfig.Value.MP3Name };
            HIJ = new Player() { Name = _inputPositionNamesConfig.Value.HIJName };
            CO = new Player() { Name = _inputPositionNamesConfig.Value.COName };
            BTN = new Player() { Name = _inputPositionNamesConfig.Value.BTNName };
        }
    }
}