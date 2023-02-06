using System.Runtime.Serialization;

namespace RimTerritory;

public interface IReadOnlySet<T> : IReadOnlyCollection<T>, ISerializable, IDeserializationCallback
{
    bool Contains(T element);
    bool TryGetValue(T equalValue, out T actualValue);
}

public static partial class SetExtensions
{
    public static IReadOnlySet<T> AsReadonly<T>(this HashSet<T> set) => new ReadOnlyHashSet<T>(set);

    /// <summary>
    /// Safe proxy of <see cref="HashSet{T}"/>.
    /// </summary>
    private class ReadOnlyHashSet<T> : IReadOnlySet<T>
    {
        private readonly HashSet<T> set;
        public ReadOnlyHashSet(HashSet<T> set)
        {
            if (set is null) 
                throw new ArgumentNullException(nameof(set));
            this.set = set;
        }

        public bool Contains(T element) => set.Contains(element);

        public bool TryGetValue(T equalValue, out T actualValue) => set.TryGetValue(equalValue, out actualValue);

        public int Count => set.Count;

        public IEnumerator<T> GetEnumerator() => set.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => set.GetEnumerator();

        public override string ToString() => set.ToString();
        public override int GetHashCode() => set.GetHashCode();
        public override bool Equals(object obj) => set.Equals(obj);

        public void GetObjectData(SerializationInfo info, StreamingContext context) => set.GetObjectData(info, context);
        public void OnDeserialization(object sender) => throw new NotSupportedException("Deserialization of read-only set is not supported.");
    }
}
