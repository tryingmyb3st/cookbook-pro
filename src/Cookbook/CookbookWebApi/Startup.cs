using CookbookDB;
using CookbookDB.Repositories;
using CookbookFileStorage;
using CookbookTheMealDB;
using CookbookWebApi.Mapping;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Minio;
using MinioConfig = CookbookFileStorage.MinioConfig;

namespace CookbookWebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("ReactApp", policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:5173",
                        "http://localhost:3000",
                        "http://localhost:5144"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            services.AddDbContext<CookbookDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<RecipeRepository>();
            services.AddScoped<IngredientRepository>();

            ConfigureMinio(services);

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 20 * 1024 * 1024;
            });

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new RecipeProfile());
                cfg.AddProfile(new IngredientProfile());
            });

            services.AddControllers();
            services.AddControllersWithViews();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Cookbook API",
                    Version = "v1",
                    Description = "API для кулинарных рецептов"
                });

                c.OperationFilter<MinioFileOperationFilter>();
            });
        }

        private void ConfigureMinio(IServiceCollection services)
        {
            var minioConfig = new MinioConfig();
            Configuration.GetSection("Minio").Bind(minioConfig);
            services.AddSingleton(minioConfig);

            services.AddSingleton(serviceProvider =>
            {
                var config = serviceProvider.GetRequiredService<MinioConfig>();

                var minioClient = new MinioClient()
                    .WithEndpoint(config.Endpoint)
                    .WithCredentials(config.AccessKey, config.SecretKey)
                    .WithSSL(config.UseSsl);

                if (!string.IsNullOrEmpty(config.Region))
                {
                    minioClient = minioClient.WithRegion(config.Region);
                }

                if (config.TimeoutSeconds > 0)
                {
                    minioClient = minioClient.WithTimeout(config.TimeoutSeconds * 1000);
                }

                return minioClient.Build();
            });

            services.AddScoped<IFileService, MinioService>();

            services.AddHttpClient<IMealDBService, MealDBService>(client =>
            {
                client.BaseAddress = new Uri("https://www.themealdb.com/api/json/v1/1/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = TimeSpan.FromSeconds(30);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cookbook API V1");
                    c.RoutePrefix = "swagger";
                });
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("ReactApp");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            InitializeMinio(app);
        }

        private static async void InitializeMinio(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var minioService = scope.ServiceProvider.GetRequiredService<IFileService>();
            var config = scope.ServiceProvider.GetRequiredService<MinioConfig>();

            await minioService.CreateBucketIfNotExistsAsync(config.BucketName);
        }
    }
}