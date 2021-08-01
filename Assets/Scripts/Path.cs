using System;

public class Path
{
    public NodeInfo current;
    public Path previous;

    public Path(NodeInfo current, Path previous)
    {
        this.current = current;
        this.previous = previous;
    }
}
