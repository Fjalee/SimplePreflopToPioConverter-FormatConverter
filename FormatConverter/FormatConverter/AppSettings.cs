namespace FormatConverter
{
    public class AppSettingsOptions
    {
        public OutputPositionNamesOptions OutputPositionNames { get; set; }
        public OutputPositionNamesOptions InputPositionNames { get; set; }
        public string InputDir { get; set; }
        public string RegexForVsBetFolder { get; set; }
        public string SeperatorForWordsInFileName { get; set; }
    }

    public class OutputPositionNamesOptions
    {
        public string SBName { get; set; }
        public string BBName { get; set; }
        public string UTGName { get; set; }
        public string MP1Name { get; set; }
        public string MP2Name { get; set; }
        public string MP3Name { get; set; }
        public string HIJName { get; set; }
        public string COName { get; set; }
        public string BTNName { get; set; }
    }

    public class InputPositionNamesOptions
    {
        public string SBName { get; set; }
        public string BBName { get; set; }
        public string UTGName { get; set; }
        public string MP1Name { get; set; }
        public string MP2Name { get; set; }
        public string MP3Name { get; set; }
        public string HIJName { get; set; }
        public string COName { get; set; }
        public string BTNName { get; set; }
    }
}
