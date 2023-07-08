using System.Linq.Expressions;
using System.Reflection;
using static System.Linq.Expressions.Expression;

namespace ExpressionTree
{
    public class MyExpression
    {

        public static IQueryable<T> TextFilter<T>(IQueryable<T> source, string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return source;
            }
            // T是查询的元素类型的编译时占位符。
            Type elementType = typeof(T);
            //获取此特定类型的所有字符串属性。
            PropertyInfo[] stringProperties = elementType.GetProperties()
                .Where(x => x.PropertyType == typeof(string))
                .ToArray();
            if (!stringProperties.Any())
            {
                return source;
            }
            //获取正确的String.Contains重载
            MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
            

            //为表达式树创建一个参数：
            //'x=>x.PropertyName.Contains（“term”）'中的'x'
            //此参数的类型是查询的元素类型
            ParameterExpression prm = Parameter(elementType, "x");

            //将每个属性映射到表达式树节点
            IEnumerable<Expression> expressions = stringProperties.Select(prp =>
                //对于每个属性，我们必须构造一个表达式树节点，如x.PropertyName.Contains（“term”）
                Call(                                // .Contains(...)
                    Property(                // .PropertyName
                        prm,                        // x 
                        prp
                        ),
                    containsMethod,
                    Constant(term)    // "term"
                )
                );
            //使用组合所有结果表达式节点||
            Expression body = expressions
                .Aggregate(
                    (prev, current) => Or(prev, current)
                    );
            //在编译时类型化的lambda表达式中包装表达式正文
            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(body, prm);
            //因为lambda是编译时类型的（尽管有一个泛型参数），所以我们可以将它与Where方法一起使用
            return source.Where(lambda);
        }
    }
}
