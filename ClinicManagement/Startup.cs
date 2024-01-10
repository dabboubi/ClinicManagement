using ClinicManagement.Models;
using ClinicManagement.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement
{
    public class Startup
    {
        private IConfiguration _config;
        public Startup(IConfiguration configuration)
        {
            _config  = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*UseSqlServer() extension method is used to configure our application specific DbContext 
             * class to use Microsoft SQL Server as the database.
                To connect to a database, we need the database connection string which is provided as 
                a parameter to UseSqlServer() extension method*/
            services.AddDbContextPool<AppDbContext>(
            options => options.UseSqlServer(_config.GetConnectionString("ClinicccDBConnection")));
            //set up dependency injection to provide instances of the ICityRepository
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IPatientRepository, SQLPatientRepository>();
            services.AddScoped<IAppointmentRepository, SQLAppointmentRepository>();
            services.AddScoped<ISpecializationRepository, SpecializationRepository>();
            services.AddScoped<IDoctorRepository, SQLDoctorRepository>();
            //To read connection string from appsettings.json file we use
            //IConfiguration service GetConnectionString() method.
            services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
