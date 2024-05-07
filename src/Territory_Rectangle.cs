namespace RimTerritory;

partial class Territory
{
    public partial class Rectangle : Territory
    {
        public Rectangle(Thing owner, float distance) : base(owner)
        {
            Adjust(Mathf.RoundToInt(distance));
        }
        public Rectangle(Thing owner, IntVec2? maxAdjustment = null, IntVec2? minAdjustment = null) : base(owner)
        {
            MaxAdjustment = maxAdjustment ?? IntVec2.Zero;
            MinAdjustment = minAdjustment ?? IntVec2.Zero;
        }

        public Rectangle(Map map, CellRect rect) : base(map)
        {
            Rect = rect;
        }

        /// <summary>
        /// Center of the rectangle.
        /// </summary>
        public override IntVec3 Position => Rect.CenterCell;

        public IntVec2 MaxAdjustment { get; protected set; }
        public IntVec2 MinAdjustment { get; protected set; }
        public void Adjust(int distance)
        {
            IntVec2 distanceVec = new(distance, distance);
            MaxAdjustment += distanceVec;
            MinAdjustment += distanceVec;
            Rect = GenerateRect();
        }
        public virtual IntVec2 Size => new (Rect.Width, Rect.Height);

        public virtual CellRect GenerateRect()
        {
            var position = Owner.Position.ToIntVec2;
            var size = Owner.def.size;
            CellRect newRect = new(position.x, position.z, size.x, size.z);
            newRect.minX -= MinAdjustment.x;
            newRect.minZ -= MinAdjustment.z;
            newRect.maxX += MaxAdjustment.x;
            newRect.maxZ += MaxAdjustment.z;
            return newRect;
        }
        public virtual CellRect Rect
        {
            get
            {
                if (rect.HasValue || Owner is null) return rect.Value;
                return rect ??= GenerateRect();
            }
            set
            {
                rect = value;
                UpdateCells();
            }
        }
        protected CellRect? rect;

        protected override IEnumerable<IntVec3> GetCells() => Rect.Cells;
    }
}