using Prescripto.Models;
using Prescripto.Repositories;
using Prescripto.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowExtensoinOrigin",
//            builder => builder
//            .AllowAnyOrigin()
//            .AllowAnyHeader()
//            .AllowAnyMethod());
//});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowFrontend",
//            builder => builder
//            .AllowAnyOrigin()
//            .AllowAnyHeader()
//            .AllowAnyMethod()         
//            );
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder
            .WithOrigins("https://pharmacy.local","https://website1.com", "https://website2.ir")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
    );
});


builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddScoped<IPharmcyService, PharmcyService>();
builder.Services.AddScoped<IPharmcyRepository, PharmcyRepository>();
builder.Services.AddScoped<ITaminService, TaminService>();
builder.Services.AddScoped<ITaminRepository, TaminRepository>();
builder.Services.AddScoped<ITTACAriaService, TTACAriaService>();
builder.Services.AddScoped<ITTACAriaRepository, TTACAriaRepository>();

builder.Services.AddScoped<DbConnectionInfo>();


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(443, listenOptions =>
    {
        listenOptions.UseHttps(@"C:\Certs\pharmacy.pfx", "1");
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();



app.Run();
