// Copyright © 2021 by Louis S. Berman
// 
// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the “Software”),
// to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software
// is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
// IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;

namespace SquidEyes.TechAnalysis
{
    public class SlidingBuffer<T> : IEnumerable<T>
    {
        private readonly List<T> buffer = new();

        public SlidingBuffer(int size, bool reversed = false)
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
}
