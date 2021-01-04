using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RS3Bot.Abstractions.Model;
using RS3Bot.Cli;
using RS3Bot.Cli.Commands;
using RS3Bot.DAL;
using RS3Bot.Discord;
using RS3BotWeb.Discord;
using RS3BotWeb.Shared;
using RS3Bot.Abstractions.Interfaces;
using System.Drawing.Text;
using System.IO;
using System;
using RS3Bot.Abstractions.Extensions;
using RS3Bot.Abstractions.Interfaces;
using static RS3Bot.Cli.Widget.BankWidget;
using RS3Bot.Cli.Widget;
using static RS3Bot.Cli.Widget.LootWidget;
using static RS3Bot.Cli.Widget.EquipmentWidget;

namespace RS3BotWeb.Server
{
    public class Startup
    {
        private PrivateFontCollection _fontCollection;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddAuthentication();
            services.AddIdentity<ApplicationUser, IdentityRole<string>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddHttpClient();
            services.AddHostedService<BotService>();
            services.AddTransient<IContextFactory, DbContextFactory>();
            services.AddSingleton<IDiscordBot, DiscordBot>();
            services.AddSingleton<ICliParser, CliParser>();
            services.AddSingleton<IItemImageGrabber, ItemImageGrabber>();
            services.AddSingleton<IWidget<BankWidgetOptions>, BankWidget>();
            services.AddSingleton<IWidget<LootWidgetOptions>, LootWidget>();
            services.AddSingleton<IWidget<EquipmentWidgetOptions>, EquipmentWidget>();
            services.AddScoped<ICommand, RegisterCommand>();
            services.AddScoped<ICommand, DiceCommand>();
            services.AddScoped<ICommand, StatsCommand>();
            services.AddScoped<ICommand, GpCommand>();
            services.AddScoped<ICommand, BankCommand>();
            services.AddScoped<ICommand, GearCommand>();
            AddRsFonts(services);

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        private void AddRsFonts(IServiceCollection services)
        {
            _fontCollection = new PrivateFontCollection();

            byte[] fontdata = null;
            using (var fontStream = ResourceExtensions.GetStreamCopy(typeof(CliParser), "RS3Bot.Cli.Fonts.trajan.otf"))
            {
                fontdata = new byte[fontStream.Length];
                fontStream.Read(fontdata, 0, (int)fontStream.Length);
                fontStream.Close();
            }
            unsafe
            {
                fixed (byte* pFontData = fontdata)
                {
                    _fontCollection.AddMemoryFont((IntPtr)pFontData, fontdata.Length);
                }
            }

            services.AddSingleton(_fontCollection);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
