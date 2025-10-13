using Expenses.App.API;
using Expenses.App.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

//Service Collection from DependencyInjection.cs ----------------

builder.AddCorsPolicy()
       .AddServiceIntegrationSupports()
       .AddObservability()
       .AddDatabase()
       .AddAuthenticationServices()
       .AddSwagger()
       .AddApplicationServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.ApplyMigrationsAsync();

    await app.SeedInitialDataAsync();
}

// Middleware

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
