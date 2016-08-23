using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Specialized;

namespace PredicateBuilder_linq
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < 20; i++)
            {
                list.Add(i);
            }
            var ran = new Random();
            list = list.OrderBy(o => ran.Next()).ToList();
            var p = MyPredicateBuilder.True<int>();
            p = p.And(o => 1 <= o && o <= 5);
            p = p.Or(o => o >= 10);
            p = p.And(o => o == 13);
            list = list.AsQueryable().Where(p).OrderBy(o=>o).ToList();
        }
    }


    public static class MyPredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>()
        {
            return param => true;
        }

        public static Expression<Func<T, bool>> False<T>()
        {
            return param => false;
        }

        public static Expression<Func<T, bool>> Create<T>(
            Expression<Func<T, bool>> predicate)
        {
            return predicate;
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            var secondBody= RewriteParameterOfSecondBody(first, second);
            var lambda = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(first.Body, secondBody), first.Parameters.FirstOrDefault());
            return lambda;
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            var secondBody = RewriteParameterOfSecondBody(first, second);
            var lambda = Expression.Lambda<Func<T, bool>>(Expression.OrElse(first.Body, secondBody), first.Parameters.FirstOrDefault());
            return lambda;
        }

        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            return Expression.Lambda<Func<T, bool>>(Expression.Not(expression.Body), expression.Parameters);
        }

        public static Expression RewriteParameterOfSecondBody<T>( Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            var map = first.Parameters
               .Select((item, index) => new { item1 = second.Parameters[index], item2 = item })
               .ToDictionary(o => o.item1, o => o.item2);

            var Substitute = new MyExpressionVisitor();
            Substitute.SetParameterMap(map);

            var secondBody = Substitute.Visit(second.Body);
            return secondBody;
        }

        class MyExpressionVisitor : ExpressionVisitor
        {
            private Dictionary<ParameterExpression, ParameterExpression> _map;

            public void SetParameterMap(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;

                if (_map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }

                return base.VisitParameter(p);
            }
        }
    }
}

namespace otherNameSpace
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>()
        {
            return param => true;
        }

        public static Expression<Func<T, bool>> False<T>()
        {
            return param => false;
        }

        public static Expression<Func<T, bool>> Create<T>(
            Expression<Func<T, bool>> predicate)
        {
            return predicate;
        }

        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        public static Expression<Func<T, bool>> Not<T>(
            this Expression<Func<T, bool>> expression)
        {
            var negated = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }

        static Expression<T> Compose<T>(
            this Expression<T> first, Expression<T> second, Func<Expression,
            Expression, Expression> merge)
        {
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        class ParameterRebinder : ExpressionVisitor
        {
            readonly Dictionary<ParameterExpression, ParameterExpression> _map;

            ParameterRebinder(Dictionary<ParameterExpression,
                ParameterExpression> map)
            {
                _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            public static Expression ReplaceParameters(
                Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;

                if (_map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }

                return base.VisitParameter(p);
            }
        }
    }

}
