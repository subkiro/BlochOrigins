public abstract class ActionBase
{
    protected  Unit _unit;
    protected ActionBase(Unit unit)
    {
        _unit = unit;
    }
    public abstract void Execute();

    public abstract void Undo();

}
