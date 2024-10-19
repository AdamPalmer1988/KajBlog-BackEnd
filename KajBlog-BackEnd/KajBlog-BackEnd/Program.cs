using KajBlog_BackEnd;
using KajBlog_BackEnd.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<KajBlogDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200", "https://kajblogfrontend.z13.web.core.windows.net") 
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddDbContext<KajBlogDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = "https://dev-blswfujpmjhb41yi.us.auth0.com/";
    options.Audience = "https://kajblog";
});

//AUTH ZERO
//This is so in swagger you can put the token in otherwise the endpoint
//would be blocked. Check the static Swagger Extension class for more details
builder.Services.AddCustomSwagger();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<KajBlogDbContext>();
    dbContext.Database.Migrate();  // Applies any pending migrations to the database
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

//make sure use authentication and use authorization are in this EXACT order
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler(errorApp => { errorApp.Run(async context => 
{ var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>(); 
    var exception = exceptionHandlerPathFeature?.Error; if (exception != null) 
    { var logger = context.RequestServices.GetRequiredService<ILogger<Program>>(); 
        logger.LogError(exception, "Unhandled exception occurred."); } context.Response.StatusCode = 500; 
    await context.Response.WriteAsync("An unexpected error occurred. Please try again later."); }); });

app.MapControllers();

app.Run();
