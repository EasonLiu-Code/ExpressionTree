
using System.Linq.Expressions;
using ExpressionTree;

var companyNames = new[] {
    "Consolidated Messenger", "Alpine Ski House", "Southridge Video",
    "City Power & Light", "Coho Winery", "Wide World Importers",
    "Graphic Design Institute", "Adventure Works", "Humongous Insurance",
    "Woodgrove Bank", "Margie's Travel", "Northwind Traders",
    "Blue Yonder Airlines", "Trey Research", "The Phone Company",
    "Wingtip Toys", "Lucerne Publishing", "Fourth Coffee"
};

// We're using an in-memory array as the data source, but the IQueryable could have come
// from anywhere -- an ORM backed by a database, a web request, or any other LINQ provider.
IQueryable<string> companyNamesSource = companyNames.AsQueryable();
var fixedQry = companyNames.OrderBy(x => x);

//给Person和Car随即添加一些数据
var people = new List<Person>();
var cars = new List<Car>();

people.Add(new Person("John", "Doe", new DateTime(1990, 1, 1)));
people.Add(new Person("aaa", "Smith", new DateTime(1992, 1, 1)));
people.Add(new Person("aaa", "aaa", new DateTime(1993, 1, 1)));
cars.Add(new Car("aaa", 2012));
cars.Add(new Car("aaa", 2016));
cars.Add(new Car("bbb", 2013));
cars.Add(new Car("ccc", 2014));

//Func<T,TResult>  T是参数，TResult是返回值
Expression<Func<string, bool>> expr = x => x.StartsWith("a");//是否从a开始
var qry = MyExpression.TextFilter(people.AsQueryable(), "aaa");
var qry2 = MyExpression.TextFilter(cars.AsQueryable(), "aaa").Where(x => x.Year == 2012);

foreach (var q in qry)
{
    Console.WriteLine(q);
}

foreach (var w in qry2)
{
    Console.WriteLine(w);
}


record Person(string LastName, string FirstName, DateTime DateOfBirth);
record Car(string Model, int Year);


