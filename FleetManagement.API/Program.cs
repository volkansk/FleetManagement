using Autofac;
using Autofac.Extensions.DependencyInjection;
using FleetManagement.API.Filters;
using FleetManagement.API.Middlewares;
using FleetManagement.API.Modules;
using FleetManagement.Repository;
using FleetManagement.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using FleetManagement.Core.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<BagDtoValidator>());

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Fleet Management API",
        Description = "An ASP.NET Core Web API for managing fleet items",
        Contact = new OpenApiContact
        {
            Name = "Volkan Þerif IÞIK",
            Url = new Uri("https://www.linkedin.com/in/volkanserif/"),
            Email = "volkanserifisik@gmail.com"
        },
        License = new OpenApiLicense
        {
            Name = "GitHub",
            Url = new Uri("https://github.com/volkansk")
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

//DbContext
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        option.MigrationsAssembly("FleetManagement.Repository");
    });
});

//Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    containerBuilder.RegisterModule(new RegisterModule()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) { }

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
