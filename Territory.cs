namespace RimTerritory;

public abstract partial class Territory : IExposable
{
    public Territory(Thing owner)
    {
        Owner = owner;
    }
    public Territory(Map map)
    {
        this.map = map;
    }

    /// <summary>
    /// Owner of this territory, 
    /// </summary>
    public Thing? Owner { get; }

    protected readonly Map map;
    /// <summary>
    /// By default Owner map otherwise null or map from constructor.
    /// </summary>
    public virtual Map Map => Owner?.MapHeld ?? map;

    /// <summary>
    /// By default Owner position otherwise invalid.
    /// </summary>
    public virtual IntVec3 Position => Owner?.Position ?? IntVec3.Invalid;

    protected HashSet<Thing> enteredThings = new();

    public IReadOnlyCollection<Thing> EnteredThings => enteredThings.Where(IsInside).ToList();
    public IReadOnlyCollection<Pawn> EnteredPawns => EnteredThings.OfType<Pawn>().ToList();

    protected List<IntVec3> cells;
    /// <summary>
    /// Cached cells of this territory.
    /// </summary>
    public IReadOnlyList<IntVec3> Cells => cells ??= (GetCells().ToList());

    /// <summary>
    /// Finds cells of this territory.<br/>
    /// Use <see cref="Cells"/> instead.
    /// </summary>
    protected abstract IEnumerable<IntVec3> GetCells();
    public IReadOnlyList<IntVec3> UpdateCells()
    {
        cells = null;
        return Cells;
    }

    /// <summary>
    /// Saves/load entered things cache.
    /// </summary>
    public virtual void ExposeData()
    {
        Scribe_Collections.Look(ref enteredThings, nameof(enteredThings));
    }
}