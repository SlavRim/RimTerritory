namespace RimTerritory;

public abstract partial class Territory : IExposable
{
    public Territory()
    {
        EnteredThings = enteredThings.AsReadonly();
    }
    public Territory(Thing owner) : this()
    {
        Owner = owner;
    }
    public Territory(Map map) : this()
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

    /// <summary>
    /// Not guarantees that entered thins is inside, controlled by locator.
    /// </summary>
    public IReadOnlySet<Thing> EnteredThings { get; }
    public IReadOnlyCollection<Pawn> EnteredPawns => EnteredThings.OfType<Pawn>().ToArray();

    /// <summary>
    /// Guarantees that entered thing is inside right now.
    /// </summary>
    public IReadOnlyCollection<Thing> EnteredThingsInside => enteredThings.Where(IsInside).ToArray();
    public IReadOnlyCollection<Pawn> EnteredPawnsInside => EnteredThingsInside.OfType<Pawn>().ToArray();

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
        Scribe_Collections.Look(ref enteredThings, nameof(enteredThings), LookMode.Reference);
    }
}