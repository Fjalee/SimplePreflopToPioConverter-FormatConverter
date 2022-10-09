using FormatConverter.TreeModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace FormatConverter
{
    public class MatchesTreeCreator : IMatchesTreeCreator
    {
        private readonly ILogger<MatchesTreeCreator> _logger;
        private readonly IOutputPositionsMetaData _outputPositionsMetaData;
        private readonly AppSettingsOptions _config;

        public MatchesTreeCreator(ILogger<MatchesTreeCreator> logger, IOutputPositionsMetaData outputPositionsMetaData,
            IOptions<AppSettingsOptions> configOptions)
        {
            _logger = logger;
            _outputPositionsMetaData = outputPositionsMetaData;
            _config = configOptions.Value;
        }

        public MatchesTree Create(string inputDir)
        {
            var childDirs = GetInputDirectoryChildren(inputDir);
            childDirs = RemoveVsBetFolderFromDirs(childDirs);
            childDirs = RemoveFileExtensionFromDirs(childDirs);

            return null;
        }

        private List<string> RemoveVsBetFolderFromDirs(List<string> dirs)
        {
            var removedAtleastOne = false;

            var result = new List<string>();
            foreach (var dir in dirs)
            {
                var vsBetRegex = new Regex("\\\\" + _config.RegexForVsBetFolder);
                var newDir = vsBetRegex.Replace(dir, "");
                result.Add(newDir);

                if (!removedAtleastOne)
                {
                    if (newDir != dir)
                    {
                        removedAtleastOne = true;
                    }
                }
            }

            if (!removedAtleastOne)
            {
                _logger.LogWarning("No vsbet folders were found (eg. 'vs_3bet')");
            }

            return result;
        }

        private List<string> RemoveFileExtensionFromDirs(List<string> dirs)
        {
            var result = new List<string>();
            foreach (var dir in dirs)
            {
                var extension = dir.Split(".").Last();
                result.Add(dir[..(dir.Length - extension.Length - 1)]);
            }
            return result;
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
