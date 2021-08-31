using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using HortaIoT.Repository;
using HortaIoT.Controllers;
using uPLibrary.Networking.M2Mqtt;

namespace HortaIoT
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
            services.AddSingleton<IDbContext>(new DbContext(Configuration.GetConnectionString("Postgre")));
            services.AddSingleton<IDataRepository, DataRepository>();
            services.AddSingleton<ICultivationRepository, CultivationRepository>();
            services.AddSingleton<MqttClient>(
                new MqttClient(Configuration["MqttBroker:Url"], int.Parse(Configuration["MqttBroker:Port"]), true, null, null, MqttSslProtocols.None));
            services.AddSingleton<MqttController>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HortaIoT", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HortaIoT v1"));
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
