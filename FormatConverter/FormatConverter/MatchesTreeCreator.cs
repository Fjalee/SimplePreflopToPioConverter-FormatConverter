using FormatConverter.InputFile;
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
        private readonly AppSettingsOptions _config;

        public MatchesTreeCreator(ILogger<MatchesTreeCreator> logger, IOptions<AppSettingsOptions> configOptions)
        {
            _logger = logger;
            _config = configOptions.Value;
        }

        public MatchesTree Create(string inputDir)
        {
            var childDirs = GetInputDirectoryChildren(inputDir);

            var childFiles = ParseFileNamesFromDirs(childDirs);
            var playersAndActions = ParsePlayerAndActionFromFileNames(childFiles);

            return null;
        }

        private List<List<PlayerAndActionStringPair>> ParsePlayerAndActionFromFileNames(List<string> fileNames)
        {
            var result = new List<List<PlayerAndActionStringPair>>();

            var seperator = _config.SeperatorForWordsInFileName;
            foreach (var fileName in fileNames)
            {
                var resultItem = new List<PlayerAndActionStringPair>();

                var splits = fileName.Split(seperator);
                for (var i = 0; i < splits.Length; i+=2)
                {
                    var newPair = new PlayerAndActionStringPair(splits[i], splits[i + 1]);
                    resultItem.Add(newPair);
                }

                if(splits.Length/2 != resultItem.Count)
                {
                    throw new Exception("function ParsePlayerAndActionFromFileNames malfunctioned");
                }

                result.Add(resultItem);
            }

            return result;
        }

        private List<string> ParseFileNamesFromDirs(List<string> dirs)
        {
            return dirs.Select(d => Path.GetFileNameWithoutExtension(d)).ToList();
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




        //private List<string> RemoveInputFolderPathFromDirs(List<string> dirs, string inputDir)
        //{
        //    var result = new List<string>();
        //    foreach (var dir in dirs)
        //    {
        //        result.Add(dir[(inputDir.Length + 1)..]);
        //    }
        //    return result;
        //}

        //private List<string> RemoveVsBetFolderFromDirs(List<string> dirs)
        //{
        //    var removedAtleastOne = false;

        //    var result = new List<string>();
        //    foreach (var dir in dirs)
        //    {
        //        var vsBetRegex = new Regex("\\\\" + _config.RegexForVsBetFolder);
        //        var newDir = vsBetRegex.Replace(dir, "");
        //        result.Add(newDir);

        //        if (!removedAtleastOne)
        //        {
        //            if (newDir != dir)
        //            {
        //                removedAtleastOne = true;
        //            }
        //        }
        //    }

        //    if (!removedAtleastOne)
        //    {
        //        _logger.LogWarning("No vsbet folders were found (eg. 'vs_3bet')");
        //    }

        //    return result;
        //}

        //private List<string> RemoveFileExtensionFromDirs(List<string> dirs)
        //{
        //    var result = new List<string>();
        //    foreach (var dir in dirs)
        //    {
        //        var extension = dir.Split(".").Last();
        //        result.Add(dir[..(dir.Length - extension.Length - 1)]);
        //    }
        //    return result;
        //}
    }
}
