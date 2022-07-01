public class MoveAction : ActionBase
{

    private readonly Tools.Directions _direction;

    public MoveAction(Unit unit, Tools.Directions direction) : base(unit)
    {
        _direction = direction;
    }
    public override void Execute()
    {
        _unit.Move(_direction);
    }

    

    public override void Undo()
    {
        _unit.Move(_direction.GetOpositeDirection());
    }

    
}
