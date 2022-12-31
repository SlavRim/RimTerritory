namespace RimTerritory;

public partial class Territory
{
    public Territory(Thing owner, float radius)
    {
        Owner = owner;
        Radius = radius;
    }
    public Territory(Map map, float radius, IntVec3 position, IntVec2? size = null)
    {
        this.map = map;
        this.position = position;
        this.size = size;
        Radius = radius;
    }

    public virtual bool IsEntered(Thing thing) => EnteredThings.Contains(thing);
    public virtual bool IsInside(Thing thing) => IsInsideLocal(thing);
    public virtual bool IsInsideLocal(LocalTargetInfo target) => IsInside(target.ToTargetInfo(Map));
    public virtual bool IsInside(TargetInfo target) => RadiusCells.Contains(target.Cell);
    public virtual bool SetEnterState(Thing thing, bool? inside = null)
    {
        var result = inside ?? IsInside(thing);
        if (result) TryEnter(thing);
        else TryExit(thing);
        return result;
    }
    protected void TryEnter(Thing thing)
    {
        if (IsEntered(thing))
        {
            CallEvents(EventType.Stay, thing);
            return;
        }
        enteredThings.Add(thing);
        CallEvents(EventType.Enter, thing);
    }

    protected void TryExit(Thing thing)
    {
        if (!IsEntered(thing)) return;
        enteredThings.Remove(thing);
        CallEvents(EventType.Exit, thing);
    }
}