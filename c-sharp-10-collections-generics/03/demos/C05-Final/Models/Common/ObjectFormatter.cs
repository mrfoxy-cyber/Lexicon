using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Inspired by: Zoran from PlurarSight

namespace Models.Common
{
    internal class ObjectFormatter
    {
        private readonly IReadOnlyList<T> data;

        public ObjectFormatter(IEnumerable<T> data)
        {
            ArgumentNullException.ThrowIfNull(data, nameof(data));
            this.data = data.ToList();
        }
    }
}
