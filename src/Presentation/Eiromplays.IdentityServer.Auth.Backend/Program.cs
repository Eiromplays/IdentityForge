using Eiromplays.IdentityServer.Application.Identity;
using Eiromplays.IdentityServer.Application.Identity.DTOs.User;
using Eiromplays.IdentityServer.Auth.Backend.Filters;
using Eiromplays.IdentityServer.Infrastructure.Extensions;
using Eiromplays.IdentityServer.Infrastructure.Identity;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllersWithViews(options =>
        options.Filters.Add<ApiExceptionFilterAttribute>())
    .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

// Customise default API behaviour
builder.Services.Configure<ApiBehaviorOptions>(options =>
    options.SuppressModelStateInvalidFilter = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpointDefinitions(typeof(UserDto));

var app = builder.Build();

//await app.Services.ApplyMigrationsAsync();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.UseEndpointDefinitions();

await app.RunAsync();