using FormatConverter.TreeModel;

namespace FormatConverter
{
    public interface IMatchesTreeCreator
    {
        public MatchesTreeNode Create(string inputDir);
    }
}