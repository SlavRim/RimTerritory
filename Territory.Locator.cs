namespace RimTerritory;

partial class Territory
{
    public record struct Locator<T>(Territory Territory, ThingRequest ThingRequest)
        where T : Thing
    {
        public int TicksDelay = 10;
        public Predicate<T>? Predicate;
        public Func<IEnumerable<T>>? PoolSelector;
        public Func<T, bool?>? Entered;

        public IReadOnlyCollection<T> Pool => (PoolSelector?.Invoke() ?? Territory.Map.listerThings.ThingsMatching(ThingRequest).OfType<T>()).ToList();

        public void Locate()
        {
            try
            {
                var pool = Pool;
                foreach (var thing in pool)
                    if (Predicate?.Invoke(thing) ?? true)
                        Territory.SetEnterState(thing, Entered?.Invoke(thing));
            }
            catch { }
        }

        private int tick;
        public void Tick()
        {
            if (Territory is null) return;
            if (++tick < TicksDelay) return;
            tick = 0;

            Locate();
        }
    }
}