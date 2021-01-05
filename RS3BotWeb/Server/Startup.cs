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
using RS3Bot.Abstractions;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using RS3Bot.Cli.Options;
using RS3Bot.Cli.Skills;

namespace RS3BotWeb.Server
{
    public class Startup
    {
        private PrivateFontCollection _fontCollection;
        public ILifetimeScope AutofacContainer { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {

            builder.RegisterType<RegisterCommand>().As<ICommand>();
            builder.RegisterType<DiceCommand>().As<ICommand>();
            builder.RegisterType<StatsCommand>().As<ICommand>();
            builder.RegisterType<GpCommand>().As<ICommand>();
            builder.RegisterType<StatusCommand>().As<ICommand>();
            builder.RegisterType<GearCommand>().As<ICommand>();
            builder.RegisterType<FishCommand>().As<ICommand>();
            builder.RegisterType<BankCommand>().As<ICommand>();
            builder.RegisterType<DbContextFactory>().As<IContextFactory>().SingleInstance();
            builder.RegisterType<TaskHandler>().As<ITaskHandler>().As<IStartable>().SingleInstance();

            builder.RegisterType<FishingSimulator>().As<ISimulator<FishOption>>().As<IStartable>().SingleInstance();

            builder.RegisterType<DiscordBot>().As<IDiscordBot>().SingleInstance();
            builder.RegisterType<CliParser>().As<ICliParser>().SingleInstance();
            builder.RegisterType<ItemImageGrabber>().As<IItemImageGrabber>().SingleInstance();
            builder.RegisterType<BankWidget>().As< IWidget<BankWidgetOptions>>().SingleInstance();
            builder.RegisterType<LootWidget>().As< IWidget<LootWidgetOptions>>().SingleInstance();
            builder.RegisterType<ReplyAwaiter>().As<IReplyAwaiter>().SingleInstance();
            builder.RegisterType<EquipmentWidget>().As< IWidget<EquipmentWidgetOptions>>().SingleInstance();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddMemoryCache();

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
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();
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
