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
