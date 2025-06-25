var builder = WebApplication.CreateBuilder(args); // kestrel server is created

//we can configure our kestrel server here

var app = builder.Build(); // web application

app.Run(async (HttpContext context) =>
{
    if (context.Request.Path.StartsWithSegments("/"))
    {
        await context.Response.WriteAsync($"This method is :  {context.Request.Method}\r\n");
        await context.Response.WriteAsync($"This method is :  {context.Request.Path}\r\n");
        
        foreach( var key in context.Request.Headers.Keys )
        {
            await context.Response.WriteAsync($"{key} : {context.Request.Headers[key]}\r\n");
        }
    }
    else if(context.Request.Path.StartsWithSegments("/worker"))
    {
        var workers = WorkersRepository.GetWorkers();
        foreach(var worker in workers)
        {
            await context.Response.WriteAsync($"{worker.Id} : {worker.Name} : {worker.Position} : {worker.Salary}\r\n");
        }
    }
});     // middleware pipeline component

//working with http methods

app.Run();

static class WorkersRepository
{
    private static List<Worker> workers = new List<Worker>
    {
        new Worker(1, "John", "Developer", 10000),
        new Worker(2, "Mary", "Developer", 10000),
        new Worker(3, "Peter", "Developer", 10000),
        new Worker(4, "Jane", "Developer", 10000),
        new Worker(5, "Jill", "Developer", 10000),
    };
    
    public static List<Worker> GetWorkers()
    {
        return workers;
    }
}

public class Worker
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Position { get; set; }
    public double Salary { get; set; }
    
    public Worker(int id, string name, string position, double salary)
    {
        Id = id;
        Name = name;
        Position = position;
        Salary = salary;
    }
}