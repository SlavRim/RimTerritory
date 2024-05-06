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
    public record Locator<ThingType>(Territory Territory, ThingRequest ThingRequest, int TicksDelay = 10)
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

        public IEnumerable<ThingType> GridPool() =>
            Territory
            .Cells
            .SelectMany(Territory.Map.thingGrid.ThingsListAtFast)
            .OfType<ThingType>()
            .Where(ThingRequest.Accepts)
            .ToList();

        public IReadOnlyCollection<ThingType> ListerPool() => 
            Territory
            .Map.listerThings
            .ThingsMatching(ThingRequest)
            .OfType<ThingType>()
            .ToList();

        /// <summary>
        /// Locates things in territory cells using <see cref="Predicate"/> and tries to update their enter state by <see cref="EnteredStateGetter"/> predicate.
        /// </summary>
        public void Locate()
        {
            foreach (var thing in (PoolSelector ?? GridPool).Invoke()) 
            {
                if (!Territory.Cells.Contains(thing.Position)) return;

                if (Predicate?.Invoke(thing) ?? true)
                    Territory.SetEnterState(thing, EnteredStateGetter?.Invoke(thing));
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