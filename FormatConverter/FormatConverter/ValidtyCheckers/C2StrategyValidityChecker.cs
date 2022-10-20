using Microsoft.Extensions.Options;

namespace FormatConverter.ValidtyCheckers
{
    public class C2StrategyValidityChecker : IC2StrategyValidityChecker
    {
        private readonly StrategyDelimitersOptions _delimConfig;

        public C2StrategyValidityChecker(IOptions<StrategyDelimitersOptions> delimConfig)
        {
            _delimConfig = delimConfig.Value;
        }

        public void ThrowIfInvalidFormat(string strategy)
        {
            var stratUnits = strategy.Split(_delimConfig.Positions);
            var percDelim = _delimConfig.Percentage;
            var regexPatternPercentage = _delimConfig.RegexPatternPercentage;

            var isValidFirstAndLast = ValidityCheckerHelper.IsValidFirstAndLast(
                "AA" + percDelim + regexPatternPercentage,
                "KK" + percDelim + regexPatternPercentage,
                stratUnits);

            if (!isValidFirstAndLast)
            {
                throw new Exception("First or Last values is not valid in strategy " + strategy);
            }

            foreach (var unit in stratUnits)
            {
                var isValid = ValidityCheckerHelper.IsValid(_delimConfig.RegexPatternHand + percDelim + regexPatternPercentage, unit);
                if (!isValid)
                {
                    throw new Exception("Wrong  strategy unit: " + unit + ". In strategy: \n" + strategy);
                }
            }
        }
    }
}
