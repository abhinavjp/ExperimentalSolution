using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace TA.PracticeService.ComparerHelper
{
    public static class Comparer
    {
        public static List<ComparerResult> Compare<TSource, TTarget>(this TSource source, TTarget target, List<MapOptions<TSource, TTarget>> mapperOptions)
        {
            var sourceType = source.GetType();
            var targetType = target.GetType();

            var sourceProperties = sourceType.GetProperties();
            var targetProperties = targetType.GetProperties();

            var comparerResult = new List<ComparerResult>();

            Parallel.ForEach(sourceProperties, (sourceProperty) =>
            {
                var sourceCustomAttributes = sourceProperty.GetCustomAttributes(true);
                if (sourceCustomAttributes != null && sourceCustomAttributes.Any(c => c is IgnoreComparisionAttribute))
                    return;
                var sourcePropertyName = sourceProperty.Name;

                var sourceComparisionNameAttribute = sourceCustomAttributes.FirstOrDefault(f => f is ComparisionNameAttribute) as ComparisionNameAttribute;
                var sourcePropertyNameToDisplay = sourceComparisionNameAttribute?.ColumnName ?? sourcePropertyName;

                var targetCustomAttributes = sourceProperty.GetCustomAttributes(true);
                if (targetCustomAttributes != null && targetCustomAttributes.Any(c => c is IgnoreComparisionAttribute))
                    return;

                var targetComparisionNameAttribute = targetCustomAttributes.FirstOrDefault(f => f is ComparisionNameAttribute) as ComparisionNameAttribute;

                var targetProperty = targetProperties.FirstOrDefault(f => f.Name == sourcePropertyName ||
                       (mapperOptions != null && mapperOptions.Any(a => GetMemberName(a.SourcePropertyMapper) == sourcePropertyName
                           && GetMemberName(a.TargetPropertyMapper) == f.Name)));
                if (targetProperty == null)
                    return;
                var targetPropertyName = targetProperty.Name;
                var targetPropertyNameToDisplay = targetComparisionNameAttribute?.ColumnName ?? targetPropertyName;

                var sourceValue = sourceProperty.GetValue(source);
                var targetValue = targetProperty.GetValue(target);

                if ((sourceValue == null && targetValue != null)|| (sourceValue != null && targetValue == null) || !sourceValue.Equals(targetValue))
                {
                    comparerResult.Add(new ComparerResult(sourcePropertyNameToDisplay, targetPropertyNameToDisplay, sourceValue, targetValue));
                }

            });
            return comparerResult;
        }

        public static List<ComparerResult> Compare<TSource, TTarget>(this TSource source, TTarget target)
        {
            return source.Compare(target, null);
        }

        private static string GetMemberName(LambdaExpression lambdaExpression)
        {
            Expression expression = lambdaExpression;
            var flag = false;
            while (!flag)
            {
                var nodeType = expression.NodeType;
                if (nodeType != ExpressionType.Convert)
                {
                    if (nodeType != ExpressionType.Lambda)
                    {
                        if (nodeType != ExpressionType.MemberAccess)
                        {
                            flag = true;
                        }
                        else
                        {
                            var memberExpression = (MemberExpression)expression;
                            return memberExpression.Member.Name;
                        }
                    }
                    else
                    {
                        expression = ((LambdaExpression)expression).Body;
                    }
                }
                else
                {
                    expression = ((UnaryExpression)expression).Operand;
                }
            }
            return null;
        }
    }
}
