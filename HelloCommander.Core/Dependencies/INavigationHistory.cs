namespace HelloCommander.Core.Dependencies;

public class HistoryNode
{
    public HistoryNode Previous { get; set; }

    public HistoryNode Next { get; set; }

    public IHcPath Path { get; set; }
}

public interface INavigationHistory : IEnumerable<IHcPath>
{
    IHcPath Current { get; }
    bool IsMoveBackEnabled { get; }
    bool IsMoveForwardEnabled { get; }
    event Action Changed;

    void Add(IHcPath hcDirectory);
    IHcPath Revert();
    IHcPath MoveBack();
    IHcPath MoveForward();
}
