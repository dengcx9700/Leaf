using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Leaf.Data;
using Leaf.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Leaf.Web
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
            services.AddDbContext<LeafDbContext>(builder =>
            {
                var type = Configuration["DbContext:Type"];
                void AddMemory()
                {
                    var memoryDbName = Configuration["DbContext:Memory:Name"];
                    builder.UseInMemoryDatabase(memoryDbName);
                }
                void AddMysql()
                {
                    var str = Configuration["DbContext:Mysql:Configition"];
                    builder.UseMySql(str);
                }
                switch (type)
                {
                    case "Memory":
                        AddMemory();
                        break;
                    case "Mysql":
                        AddMysql();
                        break;
                    default:
                        throw new NotSupportedException(type);
                }
            })
                .AddIdentity<LUser, LRole>()
                .AddEntityFrameworkStores<LeafDbContext>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            using (var db=new LeafDbContext())
            {
                db.Database.EnsureCreated();
            }
        }
    }
}
