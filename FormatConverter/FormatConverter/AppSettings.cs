namespace FormatConverter
{
    public class AppSettingsOptions
    {
        public OutputPatternsOptions OutputPatterns { get; set; }
        public InputPatternsOptions InputPatterns { get; set; }
        public OutputPositionNamesOptions OutputPositionNames { get; set; }
        public InputPositionNamesOptions InputPositionNames { get; set; }
        public string InputDir { get; set; }
        public string OutputDir { get; set; }
        public string RegexForVsBetFolder { get; set; }
        public string SeperatorForWordsInFileName { get; set; }
    }

    public class InputPatternsOptions
    {
        public InputPositionNamesOptions PositionNames { get; set; }
        public InputActionNamesOptions ActionNames { get; set; }
        public string AmountCurrency { get; set; }
        public StrategyDelimitersOptions StrategyDelimiters { get; set; }
    }

    public class StrategyDelimitersOptions
    {
        public string Positions { get; set; }
        public string Percentage { get; set; }
        public string RegexPatternPercentage { get; set; }
        public string RegexPatternHand { get; set; }
    }

    public class OutputPatternsOptions
    {
        public OutputPositionNamesOptions PositionNames { get; set; }
        public OutputActionNamesOptions ActionNames { get; set; }
        public OutputActionNamesOptions FolderActionNames { get; set; }
        public string AmountCurrency { get; set; }
        public string NBetText { get; set; }
    }

    public class InputActionNamesOptions
    {
        public string Call { get; set; }
        public string Fold { get; set; }
        public string AllIn { get; set; }
        public string Raise { get; set; }
        public string Check { get; set; }
    }

    public class OutputActionNamesOptions
    {
        public string Call { get; set; }
        public string Fold { get; set; }
        public string AllIn { get; set; }
        public string Raise { get; set; }
        public string Check { get; set; }
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
