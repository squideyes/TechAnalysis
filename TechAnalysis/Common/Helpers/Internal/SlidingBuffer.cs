// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Collections;

namespace SquidEyes.TechAnalysis;

public class SlidingBuffer<T> : IEnumerable<T>
{
    private readonly List<T> buffer = new();

    internal SlidingBuffer(int size, bool reversed = false)
    {
        if (size < 1)
            throw new ArgumentOutOfRangeException(nameof(size));

        Size = size;
        Reversed = reversed;
    }

    public int Size { get; }

    public bool Reversed { get; }

    public int Count => buffer.Count;

    public T this[int index]
    {
        get => buffer[index];
        set => buffer[index] = value;
    }

    public bool IsPrimed => (Count >= Size);

    public T[] ToArray() => buffer.ToArray();

    public void Clear() => buffer.Clear();

    public void Add(T item)
    {
        if (Reversed)
        {
            if (buffer.Count == Size)
                buffer.RemoveAt(Size - 1);

            buffer.Insert(0, item);
        }
        else
        {
            if (buffer.Count == Size)
                buffer.RemoveAt(0);

            buffer.Add(item);
        }
    }

    public void AddRange(IEnumerable<T> items) => 
        items.ForEach(item => Add(item));

    public void Update(T item)
    {
        if (Reversed)
            buffer[0] = item;
        else
            buffer[Count - 1] = item;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T> GetEnumerator() => buffer.GetEnumerator();
}