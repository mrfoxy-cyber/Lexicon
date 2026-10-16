using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

//Inspired by: Zoran from PlurarSight

namespace Models.Common
{
    internal class ObjectFormatter<T>
    {
        private readonly IReadOnlyList<T> data;
        private readonly Func<T, object?> keySelector;

        public ObjectFormatter(IEnumerable<T> data, Func<T, object?> keySelector)
        {
            ArgumentNullException.ThrowIfNull(data, nameof(data));
            ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));
            this.data = data.ToList();
            this.keySelector = keySelector;
        }

        public IEnumerable<string> Format(int width, int gap)
        {
            var groupedData = this.data.GroupBy(item => GetKey(item, this.keySelector));
            foreach (var group in groupedData)
            {
                yield return $"Group: {group.Key}";
                foreach (string line in new GridFormatter<T>(group).Format(width, gap))
                {
                    yield return line;
                }
                yield return string.Empty; // Add an empty line between groups
            }
        }

        private static object? GetKey(T item, Func<T, object?> keySelector) =>
    keySelector(item);


    }
}
