using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree
{
    public class CreateExpressionTree
    {
        public static void Test()
        {
            var one = Expression.Constant(1, typeof(int));
            var two = Expression.Constant(2, typeof(int));
            var add = Expression.Add( one, two);
            var lambda= Expression.Lambda<Func<int>>(add);
            //简化版本
            var lambda2 = Expression.Lambda(
                Expression.Add(
                    Expression.Constant(1,typeof(int)),
                    Expression.Constant(2,typeof(int))
                    )
                    );


            //构建表达式树
            Expression<Func<double,double,double>> distanceCalc=(x,y)=>Math.Sqrt(x*x+y*y);
            double distanceResult = distanceCalc.Compile()(3,4);
     

            //手动实现Lambda表达式
            var xParameter = Expression.Parameter(typeof(double), "x");
            var yParameter = Expression.Parameter(typeof(double), "y");

            var xSquared = Expression.Multiply(xParameter, xParameter);
            var ySquared = Expression.Multiply(yParameter, yParameter);
            var sum = Expression.Add(xSquared, ySquared);

            var sqrtMethod = typeof(Math).GetMethod("sqrt",new []{typeof(double)}) ?? throw new InvalidOperationException("Math.Sqrt not found!");
            var distance = Expression.Call(sqrtMethod, sum);
            var distanceLambda = Expression.Lambda(
                distance,
                xParameter,
                yParameter);
        }

    }
}
