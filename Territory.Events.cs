namespace RimTerritory;

partial class Territory
{
    public delegate void EventDelegate<in T>(T thing) where T : Thing;

    public virtual void CallEvents(EventType type, Thing thing)
    {
        void Execute<T>(Events<T> events) where T : Thing => events.Invoke(type, thing);
        Execute(ThingEvents);
        Execute(PawnEvents);
#if DEBUG
        if (thing is not Pawn pawn || type is EventType.Stay) return;
        var typeString = ("" + type).ToLower();
        Log.Message($"{pawn.NameFullColored} {typeString} {Position}");
#endif
    }

    public virtual Events<Thing> ThingEvents { get; } = new();
    public virtual Events<Pawn> PawnEvents { get; } = new();

    public class Events<ThingType>
        where ThingType : Thing
    {
        public event EventDelegate<ThingType> OnEnter, OnStay, OnExit;
        public EventDelegate<ThingType> Get(EventType type) => type switch
        {
            EventType.Enter => OnEnter,
            EventType.Stay => OnStay,
            _ => OnExit
        };
        public void Invoke(EventType type, Thing thing) => InvokeEventSafely(Get(type), thing);
        private static void InvokeEventSafely(EventDelegate<ThingType> @event, Thing thing)
        {
            if (thing is not ThingType typedThing) return;
            try {
                @event.Invoke(typedThing);
            }
            catch { }
        }
    }
}
