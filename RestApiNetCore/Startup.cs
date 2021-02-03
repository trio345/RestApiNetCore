using Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NLog;
using RestApiNetCore.Models;
using RestApiNetCore.Services;
using System;
using System.IO;

namespace RestApiNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddMvc(option => option.EnableEndpointRouting = false)
                             .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                             .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddSingleton<ITodoRepository, TodoRepository>();

            

            services.Configure<BookStoreDatabaseSettings>(
                            Configuration.GetSection(nameof(BookStoreDatabaseSettings)));

            services.AddSingleton<IBookStoreDatabaseSettings>(
                            sp => sp.GetRequiredService<IOptions<BookStoreDatabaseSettings>>().Value);

            services.AddSingleton<BookService>();

            // logger config
            services.AddSingleton<ILoggerManager, LoggerManager>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestApiNetCore", Version = "v1" });
            });

            services.AddDbContext<TodoContext>(
                     opt => opt.UseSqlServer(Configuration.GetConnectionString("TodoList")));


        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestApiNetCore v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller=Todo}/{id?}"
                    );
            });
        }
    }
}
