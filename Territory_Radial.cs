namespace RimTerritory;

partial class Territory
{
    public partial class Radial : Territory
    {
        public Radial(Thing owner, float radius) : base(owner) 
        {
            Radius = radius;
        }
        public Radial(Map map, IntVec2 size, float radius) : base(map) 
        {
            Radius = radius;
            this.size = size;
        }

        public virtual float Radius { get; set; }
        IntVec2 size;
        /// <summary>
        /// The size of center.
        /// </summary>
        public virtual IntVec2 Size => Owner?.def.Size ?? size;

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
    }
}