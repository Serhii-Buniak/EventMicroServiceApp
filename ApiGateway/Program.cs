using MMLib.SwaggerForOcelot.DependencyInjection;
using MMLib.Ocelot.Provider.AppConfiguration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.WebHost.ConfigureAppConfiguration((host, config) =>
{
    config
        .SetBasePath(host.HostingEnvironment.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.local.json", optional: true, reloadOnChange: true)
        .AddOcelotWithSwaggerSupport((o) =>
        {
            o.Folder = "Configuration";
        })
        .AddEnvironmentVariables();
});
var configuration = builder.Configuration;

builder.Services.AddOcelot(configuration)
    .AddAppConfiguration();
services.AddSwaggerForOcelot(configuration);

services.AddControllers();

var app = builder.Build();


if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSwagger();

app.UseStaticFiles();

app.UseSwaggerForOcelotUI();

await app.UseOcelot();

app.Run();