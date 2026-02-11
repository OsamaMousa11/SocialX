using Microsoft.AspNetCore.Builder;
using SocialX.Api.Extensions;

using SocialX.Api.Middlewares;
using SocialX.Core.Hubs;



var builder = WebApplication.CreateBuilder(args);


builder.Services.ServiceConfiguration(builder.Configuration);
builder.Services.AddSignalR();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    });
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors();



app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.MapHub<NotificationHub>("/hubs/notification");

app.Run();