using System.Security.AccessControl;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args); // kestrel server is created

//we can configure our kestrel server here

var app = builder.Build(); // web application

app.Run(async (HttpContext context) =>
//working with http methods
{
    if (context.Request.Method == "GET")
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
        else if(context.Request.Path.StartsWithSegments("/workers"))
        {
            var workers = WorkersRepository.GetWorkers();
            foreach(var worker in workers)
            {
                await context.Response.WriteAsync($"{worker.Id} : {worker.Name} : {worker.Position} : {worker.Salary}\r\n");
            }
        }
    }
    else if (context.Request.Method == "POST")
    {
        if (context.Request.Path.StartsWithSegments("/workers"))
        {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            Console.WriteLine(body);
            var newWorker = JsonSerializer.Deserialize<Worker>(body);
            
            WorkersRepository.AddWorker(newWorker);
        }   
    }
});     // middleware pipeline component


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
    
    public static void AddWorker(Worker? worker)
    {
        if (worker is not null)
        {
            worker.Id = workers.Count + 1;
            workers.Add(worker);
        }
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