using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using ShopsRus.Application.Discounts;
using ShopsRus.Application.Invoices;
using ShopsRus.Domain.Core;
using ShopsRus.EntityFramework;
using ShopsRus.EntityFramework.Repositories;

namespace ShopsRus.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ShopsRusDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "ShopsRusDb"));
            
            services.AddTransient(typeof(IRepository<,>), typeof(EntityFrameworkInMemoryRepository<,>));
            services.AddTransient(typeof(IRepository<>), typeof(EntityFrameworkInMemoryRepository<>));
            services.AddTransient<IDiscountCalculatorService, DiscountCalculatorService>();
            services.AddTransient<IInvoiceService, InvoiceService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShopsRus.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShopsRus.Api v1"));
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
}
