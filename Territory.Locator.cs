namespace RimTerritory;

partial class Territory
{
    /// <summary>
    /// Locates <typeparamref name="ThingType"/> things in <see cref="Territory"/> and updates their entered state.<br/>
    /// This approach is for caching.
    /// </summary>
    /// <typeparam name="ThingType"></typeparam>
    /// <param name="Territory"></param>
    /// <param name="ThingRequest"></param>
    /// <param name="TicksDelay"></param>
    public record struct Locator<ThingType>(Territory Territory, ThingRequest ThingRequest, int TicksDelay = 10)
        where ThingType : Thing
    {
        /// <summary>
        /// Predicate to locate things in pool.
        /// </summary>
        public Predicate<ThingType>? Predicate;
        /// <summary>
        /// Selects pool to locate things in.
        /// </summary>
        public Func<IEnumerable<ThingType>>? PoolSelector;
        /// <summary>
        /// Invokes to set enter state of thing.<br/>
        /// Overrides actual entered state.<br/>
        /// Sets actual state by default.
        /// </summary>
        public Func<ThingType, bool?>? EnteredStateGetter;

        /// <summary>
        /// Pool of things to search.
        /// </summary>
        public IReadOnlyCollection<ThingType> Pool => (
            PoolSelector?.Invoke() ??
            Territory?.Map?.listerThings.ThingsMatching(ThingRequest).OfType<ThingType>() ??
            Enumerable.Empty<ThingType>()
        ).ToList();

        /// <summary>
        /// Locates things in <see cref="Pool"/> by <see cref="Predicate"/> and tries to update their enter state by <see cref="EnteredStateGetter"/> predicate.
        /// </summary>
        public void Locate()
        {
            var pool = Pool;
            if (pool is null) return;
            foreach (var thing in pool)
            {
                try
                {
                    if (Predicate?.Invoke(thing) ?? true)
                        Territory.SetEnterState(thing, EnteredStateGetter?.Invoke(thing));
                }
                catch { }
            }
        }

        private int tick;
        /// <summary>
        /// Calls <see cref="Locate"/> every <see cref="TicksDelay"/> ticks.
        /// </summary>
        public void Tick()
        {
            if (Territory is null) return;
            if (++tick < TicksDelay) return;
            tick = 0;

            Locate();
        }
    }
}