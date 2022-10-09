using FormatConverter.TreeModel;

namespace FormatConverter
{
    public interface IMatchesTreeCreator
    {
        public MatchesTree Create(string inputDir);
    }
}