var builder = WebApplication.CreateBuilder(args); // kestrel server is created

//we can configure our kestrel server here

var app = builder.Build(); // web application

app.Run(async (HttpContext context) =>
{
    await context.Response.WriteAsync($"This method is :  {context.Request.Method}\r\n");
    await context.Response.WriteAsync($"This method is :  {context.Request.Path}\r\n");
    
    foreach( var key in context.Request.Headers.Keys )
    {
        await context.Response.WriteAsync($"{key} : {context.Request.Headers[key]}\r\n");
    }
});     // middleware pipeline component

//working with http methods

app.Run();