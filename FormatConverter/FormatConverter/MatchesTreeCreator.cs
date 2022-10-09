using FormatConverter.TreeModel;
using Microsoft.Extensions.Logging;

namespace FormatConverter
{
    public class MatchesTreeCreator : IMatchesTreeCreator
    {
        private ILogger<MatchesTreeCreator> _logger;
        private IPositionsMetaData _positionsMetaData;

        public MatchesTreeCreator(ILogger<MatchesTreeCreator> logger, IPositionsMetaData positionsMetaData)
        {
            _logger = logger;
            _positionsMetaData = positionsMetaData;
        }

        public MatchesTree Create(string inputDir)
        {
            var childDirs = GetInputDirectoryChildren(inputDir);


            return null;
        }

        private List<string> GetInputDirectoryChildren(string inputDir)
        {
            var childDirs = Directory.GetFiles(inputDir, "*.*", SearchOption.AllDirectories).ToList();
            var distinctChildDirs = childDirs.Distinct().ToList();
            if (childDirs.Count != distinctChildDirs.Count)
            {
                throw new FormatException("Duplicate directories are not allowed in the input root");
            }

            return distinctChildDirs;
        }
    }
}
