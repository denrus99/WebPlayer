using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebPlayer.DataBase.AlbumDB;
using WebPlayer.DataBase.PlaylistDB;
using WebPlayer.DataBase.Settings;
using WebPlayer.DataBase.SingerDB;
using WebPlayer.DataBase.TrackDB;
using WebPlayer.DataBase.UserDB;

namespace WebPlayer
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
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Populate;
            });
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
            ConfigureDB(services);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Authentication/Login");
                });
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/build"; });
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
        public void ConfigureDB(IServiceCollection services)
        {
            //Настройка конфигурации UserRepository из appsettings.json
            services.Configure<UsersDataBaseSettings>(
                Configuration.GetSection(nameof(UsersDataBaseSettings)));
            services.AddSingleton<UsersDataBaseSettings>(sp =>
                sp.GetRequiredService<IOptions<UsersDataBaseSettings>>().Value);
            //Подключение MongoUserRepository
            services.AddSingleton<MongoUserRepository>();
            
            //Настройка конфигурации AlbumRepository из appsettings.json
            services.Configure<AlbumsDataBaseSettings>(
                Configuration.GetSection(nameof(AlbumsDataBaseSettings)));
            services.AddSingleton<AlbumsDataBaseSettings>(sp =>
                sp.GetRequiredService<IOptions<AlbumsDataBaseSettings>>().Value);
            //Подключение MongoAlbumRepository
            services.AddSingleton<MongoAlbumRepository>();
            
            //Настройка конфигурации PlaylistRepository из appsettings.json
            services.Configure<PlaylistsDataBaseSettings>(
                Configuration.GetSection(nameof(PlaylistsDataBaseSettings)));
            services.AddSingleton<PlaylistsDataBaseSettings>(sp =>
                sp.GetRequiredService<IOptions<PlaylistsDataBaseSettings>>().Value);
            //Подключение MongoPlaylistRepository
            services.AddSingleton<MongoPlaylistRepository>();
            
            //Настройка конфигурации TrackRepository из appsettings.json
            services.Configure<TracksDataBaseSettings>(
                Configuration.GetSection(nameof(TracksDataBaseSettings)));
            services.AddSingleton<TracksDataBaseSettings>(sp =>
                sp.GetRequiredService<IOptions<TracksDataBaseSettings>>().Value);
            //Подключение MongoTrackRepository
            services.AddSingleton<MongoTrackRepository>();
            //Настройка конфигурации TrackRepository из appsettings.json
            services.Configure<SingersDataBaseSettings>(
                Configuration.GetSection(nameof(SingersDataBaseSettings)));
            services.AddSingleton<SingersDataBaseSettings>(sp =>
                sp.GetRequiredService<IOptions<SingersDataBaseSettings>>().Value);
            //Подключение MongoTrackRepository
            services.AddSingleton<MongoSingerRepository>();
        }
    }
}