using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.PracticeService.ComparerHelper
{
    public sealed class IgnoreComparisionAttribute : Attribute
    { }

    public sealed class ComparisionNameAttribute : Attribute
    {
        public string ColumnName { get; set; }
        public ComparisionNameAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }
}
