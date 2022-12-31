namespace RimTerritory;

public partial class Territory
{
    public Thing? Owner { get; }

    private readonly Map map;
    public virtual Map Map => Owner?.MapHeld ?? map;

    private readonly IntVec3 position;
    public virtual IntVec3 Position => Owner?.Position ?? position;

    private IntVec2? size;
    public virtual IntVec2 Size => size ??= (Owner?.def?.size ?? IntVec2.One);

    protected HashSet<Thing> enteredThings = new();
    public IReadOnlyCollection<Thing> EnteredThings => enteredThings.Where(IsInside).ToList();
    public IReadOnlyCollection<Pawn> EnteredPawns => EnteredThings.OfType<Pawn>().ToList();

    public virtual float Radius { get; set; }

    protected List<IntVec3> radiusCells;
    public IReadOnlyList<IntVec3> RadiusCells => radiusCells ??= (GetRadiusCells().ToList());

    protected virtual IEnumerable<IntVec3> GetRadiusCells()
    {
        List<IntVec3> cells = new();
        for (var x = 0; x < Size.x; x++)
        {
            for (var z = 0; z < Size.z; z++)
            {
                var offset = new IntVec3(x, 0, z);
                cells.AddRange(GenRadial.RadialCellsAround(Position + offset, Radius, false));
            }
        }
        return cells.Distinct();
    }
    public IReadOnlyList<IntVec3> UpdateRadiusCells()
    {
        radiusCells = null;
        return RadiusCells;
    }

    public virtual void ExposeData()
    {
        Scribe_Collections.Look(ref enteredThings, nameof(enteredThings));
    }
}