using FormatConverter.TreeModel;

namespace FormatConverter.Output
{
    public interface IMatchesTreeOutputer
    {
        void DoOutput(MatchesTreeNode treeRoot);
    }
}
