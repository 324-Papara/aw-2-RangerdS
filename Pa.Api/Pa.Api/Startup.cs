using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Pa.Data.Context;
using Pa.Data.UnitOfWork;
using FluentValidation.AspNetCore;


namespace Pa.Api;

public class Startup
{
    public IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }


    public void ConfigureServices(IServiceCollection services)
    {

        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pa.Api", Version = "v1" });
        });

        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

        var msSqlConStr = Configuration.GetConnectionString("MsSqlConStr");
        services.AddDbContext<PaMsDbContext>(options => options.UseSqlServer(msSqlConStr));

        //var postgreSqlConStr = Configuration.GetConnectionString("PostgreSqlConStr");
        //services.AddDbContext<PaPostDbContext>(options => options.UseNpgsql(postgreSqlConStr));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Para.Api v1"));
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}