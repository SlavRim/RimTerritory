namespace RimTerritory;

public partial class Territory
{
    public Territory(Thing owner)
    {
        Owner = owner;
    }
    public Territory(Map map, CellRect rect)
    {
        this.map = map;
        Rect = rect;
    }

    public virtual bool IsEntered(Thing thing) => EnteredThings.Contains(thing);
    public virtual bool IsInside(Thing thing) => IsInsideLocal(thing);
    public virtual bool IsInsideLocal(LocalTargetInfo target) => IsInside(target.ToTargetInfo(Map));
    public virtual bool IsInside(TargetInfo target) => Cells.Contains(target.Cell);
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