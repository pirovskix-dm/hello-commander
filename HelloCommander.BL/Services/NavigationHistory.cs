using System.Collections;

namespace HelloCommander.BL.Services;

public class NavigationHistory : INavigationHistory
{
    public IHcPath Current => _currentNode.Path;

    public bool IsMoveBackEnabled => _currentNode.Previous != null;

    public bool IsMoveForwardEnabled => _currentNode.Next != null;

    public event Action Changed;

    private HistoryNode _currentNode;

    private HistoryNode _root;

    public NavigationHistory()
    {
    }

    public void Add(IHcPath hcPath)
    {
        var newNode = new HistoryNode()
        {
            Path = hcPath
        };

        if (_currentNode != null)
        {
            newNode.Previous = _currentNode;
            _currentNode.Next = newNode;
        }
        else
        {
            _root = newNode;
        }

        _currentNode = newNode;
        RaiseHistoryChanged();
    }

    public IHcPath Revert()
    {
        if (!IsMoveBackEnabled)
        {
            throw new InvalidOperationException("Revert is not available");
        }

        _currentNode = _currentNode.Previous;
        _currentNode.Next = null;
        RaiseHistoryChanged();
        return _currentNode.Path;
    }

    public IHcPath MoveBack()
    {
        if (!IsMoveBackEnabled)
        {
            throw new InvalidOperationException("Moving back is not available");
        }

        _currentNode = _currentNode.Previous;
        RaiseHistoryChanged();
        return _currentNode.Path;
    }

    public IHcPath MoveForward()
    {
        if (!IsMoveForwardEnabled)
        {
            throw new InvalidOperationException("Moving forward is not available");
        }

        _currentNode = _currentNode.Next;
        RaiseHistoryChanged();
        return _currentNode.Path;
    }

    private void RaiseHistoryChanged()
    {
        Changed?.Invoke();
    }

    public IEnumerator<IHcPath> GetEnumerator()
    {
        var node = _root;
        while (node != null)
        {
            var returnNode = node;
            node = node.Next;
            yield return returnNode.Path;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
