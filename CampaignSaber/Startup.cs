using System.IO;
using System.Linq;
using System.Net.Http;
using EntityGraphQL.Schema;
using CampaignSaber.Models;
using CampaignSaber.Services;
using CampaignSaber.Mutations;
using Microsoft.AspNetCore.Http;
using CampaignSaber.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using CampaignSaber.Models.Settings;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace CampaignSaber
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _hostingEnvironment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JWTSettings>(_configuration.GetSection(nameof(JWTSettings)));
            services.Configure<DiscordSettings>(_configuration.GetSection(nameof(DiscordSettings)));
            services.Configure<DatabaseSettings>(_configuration.GetSection(nameof(DatabaseSettings)));

            services.AddSingleton<IJWTSettings>(ii => ii.GetRequiredService<IOptions<JWTSettings>>().Value);
            services.AddSingleton<IDiscordSettings>(ii => ii.GetRequiredService<IOptions<DiscordSettings>>().Value);
            services.AddSingleton<IDatabaseSettings>(ii => ii.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            services.AddSingleton<HttpClient>();
            services.AddSingleton<DiscordService>();

            services.AddHttpContextAccessor();
            services.AddSingleton(_hostingEnvironment);
            services.AddDbContext<CampaignSaberContext>();

            // Number of elements per page
            int pec = 10;

            var schema = SchemaBuilder.FromObject<CampaignSaberContext>();
            schema.ReplaceField(
                "users",
                new { },
                (db, param) => db.Users.FirstOrDefault(), "");
            schema.ReplaceField(
                "campaigns",
                new
                {
                    page = (int?)null
                },
                (db, param) => db.Campaigns.Skip(param.page.GetValueOrDefault() * pec).Take(pec), "Get a page of campaigns");

            schema.AddField(
                "campaignsFromUser",
                new
                {
                    userId = ArgumentHelper.Required<string>(),
                    page = (int?)null
                },
                (db, param) => db.Campaigns.Where(c => c.UploaderId == param.userId.Value).Skip(param.page.GetValueOrDefault() * pec).Take(pec), "Get a page of campaigns made by a user");

            schema.AddField(
                "campaignsWithMap",
                new
                {
                    mapKey = ArgumentHelper.Required<string>(),
                    page = (int?)null
                },
                (db, param) => db.Campaigns.Where(c => c.Metadata.Challenges.Any(cm => cm.SongID.ToLower() == param.mapKey.Value.ToLower())).Skip(param.page.GetValueOrDefault() * pec).Take(pec), "Gets a page of campaigns that contain a specific beatmap");

            schema.AddMutationFrom(new CampaignMutations());
            services.AddSingleton(schema);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(opt =>
                {
                    opt.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            services.AddControllers().AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            Directory.CreateDirectory("files");
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(_hostingEnvironment.ContentRootPath, "files")),
                RequestPath = "/files"
            });

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseMiddleware<JWTValidator>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("CampaignSaber OK!");
                });
                endpoints.MapGet("/api", async context =>
                {
                    await context.Response.WriteAsync("CampaignSaber OK!");
                });
                endpoints.MapControllers();
                endpoints.MapFallback(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("Not Found");
                });
            });
        }
    }
}