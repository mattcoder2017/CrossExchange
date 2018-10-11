using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XOProject.Authentication;

namespace XOProject
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
            services.AddDbContext<ExchangeContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IShareRepository, ShareRepository>();
            services.AddTransient<IPortfolioRepository, PortfolioRepository>();
            services.AddTransient<ITradeRepository, TradeRepository>();

            //Adding CORS to allow access from permitted origin
            services.AddCors(cfg =>
            {
                cfg.AddPolicy("permitone", policy =>
                {
                    policy.WithOrigins("http://localhost:21154"); //can be specific on the origin or referer to be permitted
                    //AllowAnyOrigin();  

                });
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHttpStatusCodeExceptionMiddleware();
            }
            else
            {
                app.UseHttpStatusCodeExceptionMiddleware();
                app.UseExceptionHandler();
            }
            //Adding CORS to allow access from permitted origin
            app.UseCors("permitone");
            //Remove security volunerability by adding authentication
            app.UseMiddleware<XOAuthenticationMiddleware>();
            
          
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
