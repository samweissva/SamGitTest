using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTrees
{
    class Program
    {
        static void Main(string[] args)
        {
            //20000000

            List<Model> list = new List<Model>();
            for (int i = 0; i < 20000000; i++)
            {
                var item = new Model() { sort = i };
                list.Add(item);
            }
            var ran = new Random();
            list = list.OrderBy(o => ran.Next()).ToList();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var SortOrder_Reflection = list.AsQueryable().SortOrder_Reflection<Model, int>("sort").ToList();//00:00:34.0143682
            var SortBy = list.AsQueryable().SortBy("sort asc").ToList();//00:00:24.2078218
            var ApplyOrder = list.AsQueryable().ApplyOrder("sort", true).ToList(); //00:00:26.4544241
            var normal = list.OrderBy(o => o.sort).ToList(); //00:00:23.7141314
            var SortBy_TwoGenParas = list.AsQueryable().SortBy_TwoGenParas<Model, int>("sort").ToList(); //00:00:25.5930269

            Trace.WriteLine(sw.Elapsed);
        }

        static void test1()
        {
            ParameterExpression parameter = ParameterExpression.Parameter(typeof(int));
            Expression cons3 = Expression.Constant(3);
            var expression = Expression.Lambda<Func<int, bool>>(Expression.GreaterThan(parameter, cons3), new ParameterExpression[] { parameter });

            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
            var result = typeof(Enumerable).GetMethods().FirstOrDefault(method => method.Name == "Where" && method.IsGenericMethodDefinition)
                .MakeGenericMethod(typeof(int))
                .Invoke(null, new object[] { numbers, expression.Compile() });

            var re = result as IEnumerable<int>;
            foreach (var item in re)
            {
                Console.WriteLine(item);
            }
        }

        static void simplify()
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7 };

            Expression<Func<int, bool>> expression = x => x < 5;

            var re = numbers.Where(expression.Compile()).ToList();

            foreach (var item in re)
            {
                Console.WriteLine(item);
            }
        }
    }

    public class Model
    {
        public int sort { get; set; }
    }

    public static class Extionsion
    {
        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, string field)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            string sortDirection = String.Empty;
            string propertyName = String.Empty;

            field = field.Trim();
            int spaceIndex = field.Trim().IndexOf(" ");
            if (spaceIndex < 0)
            {
                propertyName = field;
                sortDirection = "asc";
            }
            else
            {
                propertyName = field.Substring(0, spaceIndex);
                sortDirection = field.Substring(spaceIndex + 1).Trim();
            }

            if (String.IsNullOrEmpty(propertyName))
            {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, String.Empty);
            MemberExpression property = Expression.Property(parameter, propertyName);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = (sortDirection == "desc") ? "OrderByDescending" : "OrderBy";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                                new Type[] { source.ElementType, property.Type },
                                                source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }

        public static IOrderedQueryable<T> ApplyOrder<T>(this IQueryable<T> source, string property, bool isAscdening = false)
        {
            var type = typeof(T);
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            var pi = type.GetProperties().FirstOrDefault(c => c.Name.Equals(property, StringComparison.OrdinalIgnoreCase));
            if (pi == null)
            {
                throw new Exception();
            }
            expr = Expression.Property(expr, pi);
            type = pi.PropertyType;

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            object result;
            if (true == isAscdening)
            {
                result = typeof(Queryable).GetMethods().
                    Single(method => method.Name == "OrderBy" && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2 && method.GetParameters().Length == 2).
                    MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            }
            else
            {
                result = typeof(Queryable).GetMethods().
                    Single(method => method.Name == "OrderByDescending" && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2 && method.GetParameters().Length == 2).
                    MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            }
            return (IOrderedQueryable<T>)result;
        }


        public static IOrderedQueryable<T> SortOrder_Reflection<T, T2>(this IQueryable<T> query, string field)
        {
            Expression<Func<T, T2>> lambda = x => (T2)typeof(T).GetProperty(field).GetValue(x, null);
            return query.OrderBy(lambda);
        }

        public static IQueryable<T> SortBy_TwoGenParas<T, Tinner>(this IQueryable<T> source, string field)
        {
            var p3 = Expression.Parameter(typeof(T), "x");
            var lamda = Expression.Lambda<Func<T, Tinner>>(Expression.Property(p3, field), p3);  // 参数为引用，new出一个就可以
            return source.OrderBy(lamda);

        }

    }


    #region conclusion
    // 1 参数为引用，new出一个就可以
    // 2 source.Provider.CreateQuery<T>(methodCallExpression);
    // 3 Expression<Func<int, bool>> expression = x => x < 5;
    // 4 Expression<Func<int, bool>> expression = Expression.Lambda<Func<T, bool>> 用泛型lamda来转换泛型



    #endregion

}
