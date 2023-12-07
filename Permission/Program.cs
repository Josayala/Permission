using MediatR;
using Microsoft.EntityFrameworkCore;
using Permission.Api.Middleware;
using Permission.Application;
using Permission.Application.Abstractions;
using Permission.Application.AutoMapper;
using Permission.Application.ElasticSearchServices;
using Permission.Application.KafkaService;
using Permission.Application.PermissionService;
using Permission.Domain.Interfaces;
using Permission.Infrastructure;
using Permission.Infrastructure.EF.Repositories;

const string APPLICATION_ASSEMBLY_NAME = "Permission.Application";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connection = builder.Configuration.GetConnectionString("DBConnectionString") ?? throw new Exception("DBConnectionString is null");

builder.Services.AddDbContext<PermissionDbContext>(options =>
options.UseSqlServer(connection,
    x => {
        x.MigrationsAssembly("Permission.EFMigration");
        x.ExecutionStrategy((dependencies) => new CustomExecutionStrategy(dependencies, 3, TimeSpan.FromSeconds(5), new List<int>()));
    }));

var applicationAssembly = AppDomain.CurrentDomain.Load(APPLICATION_ASSEMBLY_NAME);


builder.Services.AddControllers();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddSingleton(AutoMapperConfig.Initialize());
builder.Services.AddScoped<IElasticSearchService, ElasticSearchService>();

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
builder.Services.AddScoped<IPermissionOperationService, PermissionOperationService>(); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMediatR(applicationAssembly);
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IPermissionRepository, PermissionRepository>();
var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
