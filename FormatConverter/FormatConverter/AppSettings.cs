namespace FormatConverter
{
    public class AppSettingsOptions
    {
        public PositionNamesOptions? PositionNames { get; set; }
        public string? InputDir { get; set; }
    }

    public class PositionNamesOptions
    {
        public string? SBName { get; set; }
        public string? BBName { get; set; }
        public string? UTGName { get; set; }
        public string? MP1Name { get; set; }
        public string? MP2Name { get; set; }
        public string? MP3Name { get; set; }
        public string? HIJName { get; set; }
        public string? COName { get; set; }
        public string? BTNName { get; set; }
    }
}
