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
using System.IO;

namespace SquidEyes.TechAnalysis
{
    public class CsvEnumerator : IEnumerable<string[]>, IDisposable
    {
        private readonly StreamReader reader;
        private readonly int expectedFields;

        private bool disposed = false;

        private bool skipFirst;

        public CsvEnumerator(StreamReader reader, int expectedFields, bool skipFirst = false)
        {
            if (expectedFields <= 0)
                throw new ArgumentOutOfRangeException(nameof(expectedFields));

            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
            this.expectedFields = expectedFields;
            this.skipFirst = skipFirst;
        }

        public CsvEnumerator(Stream stream, int expectedFields, bool skipFirst = false)
            : this(new StreamReader(stream), expectedFields, skipFirst)
        {
        }

        public CsvEnumerator(string fileName, int expectedFields, bool skipFirst = false)
            : this(File.OpenRead(fileName), expectedFields, skipFirst)
        {
        }

        ~CsvEnumerator() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !disposed && reader != null)
                reader.Dispose();

            disposed = true;
        }

        public IEnumerator<string[]> GetEnumerator()
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if (skipFirst)
                {
                    skipFirst = false;

                    continue;
                }

                var fields = line.Split(',');

                if (fields.Length != expectedFields)
                {
                    throw new InvalidDataException(
                        $"{expectedFields} expected; {fields.Length} found");
                }

                yield return fields;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
