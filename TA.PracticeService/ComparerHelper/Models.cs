using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TA.PracticeService.ComparerHelper
{
    public class ComparerResult
    {
        public ComparerResult(string columnName, object sourceValue, object targetValue) : this(columnName, columnName, sourceValue, targetValue)
        {
        }
        public ComparerResult(string sourceColumnName, string targetColumnName, object sourceValue, object targetValue)
        {
            SourceColumnName = sourceColumnName;
            TargetColumnName = targetColumnName;
            SourceValue = sourceValue;
            TargetValue = targetValue;
        }
        public string SourceColumnName { get; set; }
        public object SourceValue { get; set; }
        public string TargetColumnName { get; set; }
        public object TargetValue { get; set; }
        public bool HaveSameColumnNames => SourceColumnName.Equals(TargetColumnName, StringComparison.InvariantCultureIgnoreCase);
    }

    public class MapOptions<TSource, TTarget>
    {
        public MapOptions(Expression<Func<TSource, object>> sourcePropertyMapper, Expression<Func<TTarget, object>> targetPropertyMapper)
        {
            SourcePropertyMapper = sourcePropertyMapper;
            TargetPropertyMapper = targetPropertyMapper;
        }
        public Expression<Func<TSource, object>> SourcePropertyMapper { get; set; }
        public Expression<Func<TTarget, object>> TargetPropertyMapper { get; set; }
    }

    internal class MappingList
    {
        public PropertyInfo SourceProperty { get; set; }
        public PropertyInfo TargetProperty { get; set; }
    }
}
