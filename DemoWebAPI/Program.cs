using DemoWebAPI;
using Grok.AspNetCore.Microsoft.Extensions;

var builder = WebApplication.CreateBuilder(args);

await builder.AddApplicationAsync<DemoWebApiModule>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
