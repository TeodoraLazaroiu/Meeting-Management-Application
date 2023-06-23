using MeetingManagement.Application;
using MeetingManagement.Application.DTOs.Mail;
using MeetingManagement.Persistence;
using MeetingManagement.Persistence.Context;
using MeetingManagement.WebApp.Middlewares;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();

var mailSettings = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettingsDTO>(mailSettings);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = builder.Configuration.GetRequiredSection("Frontend").Value;
string frontendUrl = config == null ? "" : config.ToString();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins(frontendUrl)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();