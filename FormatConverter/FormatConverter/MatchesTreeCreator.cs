using FormatConverter.InputFile;
using FormatConverter.TreeModel;
using static FormatConverter.Program;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using FormatConverter.IllegalActions;
using FormatConverter.ValidtyCheckers;

namespace FormatConverter
{ 
    public class MatchesTreeCreator : IMatchesTreeCreator
    {
        private readonly ILogger<MatchesTreeCreator> _logger;
        private readonly AppSettingsOptions _config;
        private readonly ITurnsLegalityChecker _turnsLegalityChecker;
        private readonly IMatchesTreeLegalityChecker _matchesTreeLegalityChecker;
        private readonly IC2StrategyValidityChecker _c2StrategyValidityChecker;

        public MatchesTreeCreator(ILogger<MatchesTreeCreator> logger, IOptions<AppSettingsOptions> configOptions,
            ITurnsLegalityChecker turnsLegalityChecker, IMatchesTreeLegalityChecker matchesTreeLegalityChecker,
            IC2StrategyValidityChecker c2StrategyValidityChecker)
        {
            _logger = logger;
            _config = configOptions.Value;
            _turnsLegalityChecker = turnsLegalityChecker;
            _matchesTreeLegalityChecker = matchesTreeLegalityChecker;
            _c2StrategyValidityChecker = c2StrategyValidityChecker;
        }

        public MatchesTreeNode Create(string inputDir)
        {
            var childDirs = GetInputDirectoryChildren(inputDir);

            var playersAndActions = ParsePlayerAndActionFromFileNames(childDirs);
            var turnsBranches = CreateTurnsFromActionPairs(playersAndActions);

            var positionsInUse = GetWhatPositionsAreInUse(turnsBranches);
            positionsInUse = CorrectOrderOfPositions(positionsInUse);

            turnsBranches = AddMissingFolds(turnsBranches, positionsInUse);

            _turnsLegalityChecker.ThrowIfAnyPositionWasSkippedInTurn(turnsBranches, positionsInUse);
            _turnsLegalityChecker.ThrowIfPlayerMovesAfterFold(turnsBranches, positionsInUse);
            _turnsLegalityChecker.ThrowIfIllegalMove(turnsBranches, positionsInUse);

            var result = CreateMatchesTree(turnsBranches);

            _matchesTreeLegalityChecker.ThrowIfIllegalMove(result, positionsInUse);

            return result;
        }

        private MatchesTreeNode CreateMatchesTree(List<List<Turn>> turnsBranches)
        {
            var results = new MatchesTreeNode(null);
            foreach (var turns in turnsBranches)
            {
                var parentNode = FindOrCreateParentNode(turns.First(), results.ChildNodes);
                foreach (var t in turns.Skip(1))
                {
                    parentNode = FindOrCreateParentNode(t, parentNode.ChildNodes);
                }
            }

            return results;
        }

        private MatchesTreeNode FindOrCreateParentNode(Turn currTurn, List<MatchesTreeNode> parentNodes)
        {
            var result = parentNodes.Where(b => b.Turn.Player.Position == currTurn.Player.Position)
                .SingleOrDefault(b => b.Turn.Action == currTurn.Action);

            if (result == null
                || result.Turn.Action != currTurn.Action)
            {
                result = new MatchesTreeNode(currTurn);
                parentNodes.Add(result);
            }
            if (result.Turn.RaiseAmountInBB != currTurn.RaiseAmountInBB)
            {
                throw new Exception("Same node but bb amounts are different");
            }

            return result;
        }

        private List<List<Turn>> AddMissingFolds(List<List<Turn>> turnsBranches, List<PositionEnum> positionsInUse)
        {
            var result = new List<List<Turn>>();

            foreach (var branch in turnsBranches)
            {
                var currBranch = branch.ToList();
                var currPositionsInPlay = positionsInUse.ToList();
                var currRound = currPositionsInPlay.ToList();
                var newBranch = new List<Turn>();

                while (currBranch.Count != 0)
                {
                    if (currRound.Count == 0)
                    {
                        currRound = currPositionsInPlay.ToList();
                    }
                    var activePos = currRound.First();
                    var activeTurn = currBranch.First();
                    if (activeTurn.Player.Position != activePos)
                    {
                        currPositionsInPlay.Remove(activePos);
                        newBranch.Add(CreateFoldTurn(activePos));
                    }
                    else
                    {
                        if (activeTurn.Action == TurnActionEnum.Fold)
                        {
                            currPositionsInPlay.Remove(activeTurn.Player.Position);
                        }
                        currBranch.Remove(activeTurn);
                        newBranch.Add(activeTurn);
                    }
                    currRound.RemoveAt(0);
                }

                result.Add(newBranch);
            }

            return result;
        }

        private List<List<Turn>> AddStartingFolds(List<List<Turn>> turnsBranches, List<PositionEnum> positionsInUse)
        {
            var result = new List<List<Turn>>();

            foreach (var branch in turnsBranches)
            {
                var indexOrderOfFirstNonFoldPosition = positionsInUse.IndexOf(branch.First().Player.Position);
                var missingPositions = positionsInUse.GetRange(0, indexOrderOfFirstNonFoldPosition);
                var missingFolds = missingPositions.Select(p => CreateFoldTurn(p)).ToList();
                missingFolds.AddRange(branch);
                result.Add(missingFolds);
            }

            return result;
        }

        private Turn CreateFoldTurn(PositionEnum position)
        {
            var result = new Turn(InputPositionsMetaData.GetPlayer(position), TurnActionEnum.Fold, "");
            return result;
        }

        private List<PositionEnum> CorrectOrderOfPositions(List<PositionEnum> positions)
        {
            var result = PositionsOrder.ToList();

            foreach (var p in PositionsOrder)
            {
                if (!positions.Contains(p))
                {
                    result.Remove(p);
                }
            }

            return result;
        }

        private List<PositionEnum> GetWhatPositionsAreInUse(List<List<Turn>> turnsBranches)
        {
            var result = new List<PositionEnum>();

            var flattenedList = turnsBranches.SelectMany(t => t).ToList();

            foreach (var item in flattenedList)
            {
                if (!result.Contains(item.Player.Position))
                {
                    result.Add(item.Player.Position);
                }
            }

            return result;
        }

        private List<List<Turn>> CreateTurnsFromActionPairs(List<TurnBranchInPairs> pairsList)
        {
            var result = new List<List<Turn>>();

            foreach (var pairs in pairsList)
            {
                var turns = new List<Turn>();

                foreach (var p in pairs.Pairs)
                {
                    var player = InputPositionsMetaData.GetPlayer(p.Player);

                    var actionAndAmount = ParseAction(p.Action);
                    var action = actionAndAmount.Item1;

                    var strategy = ""; //fix get from txt

                    var turn = new Turn(player, action, strategy);
                    turn.RaiseAmountInBB = actionAndAmount.Item2;
                    turns.Add(turn);
                }


                turns.Last().Strategy = pairs.Strategy;
                result.Add(turns);
            }

            return result;
        }

        private Tuple<TurnActionEnum, string> ParseAction(string action)
        {
            var callString = _config.InputPatterns.ActionNames.Call;
            var foldString = _config.InputPatterns.ActionNames.Fold;
            var allInString = _config.InputPatterns.ActionNames.AllIn;
            var checkString = _config.InputPatterns.ActionNames.Check;

            var raiseString = _config.InputPatterns.ActionNames.Raise;
            var amountCurrencyString = _config.InputPatterns.AmountCurrency;

            if (action == callString)
            {
                return new Tuple<TurnActionEnum, string>(TurnActionEnum.Call, null);
            }
            if (action == checkString)
            {
                return new Tuple<TurnActionEnum, string>(TurnActionEnum.Check, null);
            }
            if (action == foldString)
            {
                return new Tuple<TurnActionEnum, string>(TurnActionEnum.Fold, null);
            }
            if (action == allInString)
            {
                return new Tuple<TurnActionEnum, string>(TurnActionEnum.AllIn, null);
            }

            var raiseAmountRegex = new Regex("[\\d]+.[\\d]+");
            var raiseAmount = raiseAmountRegex.Match(action).Value;
            if (action == raiseString + raiseAmount + amountCurrencyString)
            {
                return new Tuple<TurnActionEnum, string>(TurnActionEnum.Raise, raiseAmount);
            }

            throw new InvalidDataException("Could now parse action \"" + action + "\"");
        }

        private List<TurnBranchInPairs> ParsePlayerAndActionFromFileNames(List<string> dirs)
        {
            var result = new List<TurnBranchInPairs>();

            var seperator = _config.SeperatorForWordsInFileName;
            foreach (var dir in dirs)
            {
                var fileName = Path.GetFileNameWithoutExtension(dir);
                var resPairs = new List<PlayerAndActionStringPair>();

                var splits = fileName.Split(seperator);
                for (var i = 0; i < splits.Length; i+=2)
                {
                    var newPair = new PlayerAndActionStringPair(splits[i], splits[i + 1]);
                    resPairs.Add(newPair);
                }

                if(splits.Length/2 != resPairs.Count)
                {
                    throw new Exception("function ParsePlayerAndActionFromFileNames malfunctioned");
                }

                var strategy = File.ReadAllText(dir);
                _c2StrategyValidityChecker.ThrowIfInvalidFormat(strategy);
                var resultItem = new TurnBranchInPairs(resPairs, strategy);
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
