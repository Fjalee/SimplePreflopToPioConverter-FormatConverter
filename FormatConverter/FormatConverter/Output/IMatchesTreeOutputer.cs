using FormatConverter.TreeModel;

namespace FormatConverter.Output
{
    public interface IMatchesTreeOutputer
    {
        int DoOutput(MatchesTreeNode treeRoot);
    }
}
