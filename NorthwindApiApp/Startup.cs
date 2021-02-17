using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Northwind.DataAccess.SqlServer;
using Northwind.Services.EntityFrameworkCore;
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
            services.AddScoped((service) =>
            {
                var sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("NorthwindConnection"));
                return sqlConnection;
            });

            services.AddTransient<NorthwindDataAccessFactory, SqlServerDataAccessFactory>();
            services.AddTransient<IProductManagementService, ProductManagementDataAccessService>();
            services.AddTransient<IProductCategoryManagementService, ProductCategoriesManagementDataAccessService>();
            services.AddTransient<IProductCategoryPicturesService, ProductCategoryPicturesManagementDataAccessService>();
            services.AddDbContext<NorthwindContext>(opt => opt.UseInMemoryDatabase("Northwind"));
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
