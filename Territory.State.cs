namespace RimTerritory;

partial class Territory
{
    public Thing? Owner { get; }

    private readonly Map map;
    public virtual Map Map => Owner?.MapHeld ?? map;

    public virtual IntVec3 Position => Rect.CenterCell;
    public virtual IntVec2 Size => new(Rect.Width, Rect.Height);

    public virtual CellRect Rect
    {
        get
        {
            if (rect.HasValue || Owner is null) return rect.Value;
            var position = Owner.Position.ToIntVec2;
            var size = Owner.def.size;
            position -= size;
            return rect ??= new(position.x, position.z, size.x, size.z);
        }
        set
        {
            rect = value;
            UpdateCells();
        }
    }
    private CellRect? rect;

    protected HashSet<Thing> enteredThings = new();
    public IReadOnlyCollection<Thing> EnteredThings => enteredThings.Where(IsInside).ToList();
    public IReadOnlyCollection<Pawn> EnteredPawns => EnteredThings.OfType<Pawn>().ToList();

    protected List<IntVec3> cells;
    public IReadOnlyList<IntVec3> Cells => cells ??= (GetCells().ToList());

    protected virtual IEnumerable<IntVec3> GetCells() => Rect.Cells;
    public IReadOnlyList<IntVec3> UpdateCells()
    {
        cells = null;
        return Cells;
    }

    public virtual void ExposeData()
    {
        Scribe_Collections.Look(ref enteredThings, nameof(enteredThings));
    }
}