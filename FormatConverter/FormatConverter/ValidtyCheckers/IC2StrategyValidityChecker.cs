namespace FormatConverter.ValidtyCheckers
{
    public interface IC2StrategyValidityChecker
    {
        void ThrowIfInvalidFormat(string strategy);
    }
}
