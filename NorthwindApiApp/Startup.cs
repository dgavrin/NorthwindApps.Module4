using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Northwind.DataAccess.SqlServer;
using Northwind.DataAccess.SqlServer.Employees;
using Northwind.DataAccess.SqlServer.Products;
using Northwind.Services.Blogging;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore;
using Northwind.Services.EntityFrameworkCore.Blogging;
using Northwind.Services.EntityFrameworkCore.Blogging.Context;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.Products;

namespace NorthwindApiApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var northwindConnectionString = this.Configuration.GetConnectionString("NorthwindConnection");
            services.AddDbContext<NorthwindContext>(o => o.UseSqlServer(northwindConnectionString));
            var bloggingConnectionString = this.Configuration.GetConnectionString("BloggingConnection");
            services.AddDbContext<BloggingContext>(o => o.UseSqlServer(bloggingConnectionString));

            services.AddTransient<IProductManagementService, ProductManagementService>();
            services.AddTransient<IProductCategoryManagementService, ProductCategoryManagementService>();
            services.AddTransient<IProductCategoryPicturesService, ProductCategoryPicturesService>();
            services.AddTransient<IEmployeeManagementService, EmployeeManagementService>();
            services.AddTransient<IBloggingService, BloggingService>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NorthwindApiApp", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NorthwindApiApp v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
