using System;
public class NodeInfo
{
    public Point point;

    public float fValue;

    public float hValue;

    public float gValue;

    public NodeInfo(Point point, float hValue, float gValue)
    {

        this.point = point;

        this.hValue = hValue;
        this.gValue = gValue;

        this.fValue = hValue + gValue;
    }
}
