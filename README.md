### How to use
1. Clone this repo to your project.
2. Add RimTerritory.csproj as reference.

#### Before adding new features to your project, update repo(fetch and pull).

### Example of usage(May be outdated):

0. Create/use Thing Class.
```CSharp
public class MyThing : Thing
{
    public Territory Territory
    {
        get;
        protected set;
    }
    public Territory.Locator<Pawn>? PawnLocator;
}
```
1. Add handlers.
```CSharp
protected void PawnEnter(Pawn pawn)
{
    // Pawn enter
}

protected void PawnExit(Pawn pawn)
{
    // Pawn exit
}

protected void PawnStay(Pawn pawn)
{
    // Pawn stay
}
```
2. Add initialization of territory.
```CSharp
public override void SpawnSetup(Map map, bool respawningAfterLoad)
{
    base.SpawnSetup(map, respawningAfterLoad);

    // Initialization of territory
    Territory ??= new(this, Radius);
    var events = Territory.PawnEvents;
    events.OnEnter += PawnEnter;
    events.OnExit += PawnExit;
    events.OnStay += PawnStay;
    PawnLocator = new(Territory, ThingRequest.ForGroup(ThingRequestGroup.Pawn))
    {
        Predicate = pawn => !pawn.health.Dead
    };
}
```
3. Add finalization of territory.
```CSharp
public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
{
    base.DeSpawn(mode);

    Territory = null;
    PawnLocator = null;
}
```
4. Add ticking of locator.
```CSharp
public override void Tick()
{
    PawnLocator?.Tick();
}
```
5. Add ExposeData.
```CSharp
public override void ExposeData()
{
    Territory?.ExposeData();
}
```