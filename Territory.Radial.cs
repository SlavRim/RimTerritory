namespace RimTerritory;

partial class Territory
{
    public class Radial : Territory
    {
        public Radial(Thing owner, float radius) : base(owner) 
        {
            Radius = radius;
        }
        public Radial(Map map, float radius, CellRect rect) : base(map, rect) 
        {
            Radius = radius;
        }

        public virtual float Radius { get; set; }

        protected override IEnumerable<IntVec3> GetCells()
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

        [Obsolete("Unused in this context.")]
        public override CellRect Rect => new();
    }
}