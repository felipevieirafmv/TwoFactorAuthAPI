using Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<TwoFactorAuthAPIDbContext>();
builder.Services.AddTransient<IUserDataService, UserDataService>();
builder.Services.AddSingleton<ISecurityService, SecurityService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy",
        policy => {
            policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();