using MeetingManagement.Application;
using MeetingManagement.Persistance;
using MeetingManagement.WebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistanceServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var origins = "origins";
var frontendUrl = builder.Configuration.GetSection("frontend").Value;
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: origins, builder =>
        {
            builder.WithOrigins(frontendUrl ?? "")
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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(origins);

app.MapControllers();

app.AddErrorHandler();

app.Run();