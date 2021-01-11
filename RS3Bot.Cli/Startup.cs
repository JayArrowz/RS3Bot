using Autofac;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RS3Bot.Abstractions.Extensions;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.Abstractions.Model;
using RS3Bot.Cli;
using RS3Bot.Cli.Commands;
using RS3Bot.Cli.Commands.Impl;
using RS3Bot.Cli.Options;
using RS3Bot.Cli.Skills;
using RS3Bot.Cli.Widget;
using RS3Bot.DAL;
using RS3BotWeb.Shared;
using System;
using System.Drawing.Text;
using static RS3Bot.Cli.Widget.BankWidget;
using static RS3Bot.Cli.Widget.EquipmentWidget;
using static RS3Bot.Cli.Widget.LootWidget;
using static RS3Bot.Cli.Widget.ShopWidget;

namespace RS3BotWeb.Cli
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
            builder.RegisterType<SkillCommand>().As<ICommand>();
            builder.RegisterType<BankCommand>().As<ICommand>();
            builder.RegisterType<ShopViewCommand>().As<ICommand>();
            builder.RegisterType<ShopBuyCommand>().As<ICommand>();
            builder.RegisterType<DbContextFactory>().As<IContextFactory>().SingleInstance();
            builder.RegisterType<TaskHandler>().As<ITaskHandler>().As<IStartable>().SingleInstance();

            builder.RegisterType<SkillSimulator>().As<ISimulator<SkillOption>>().As<IStartable>().SingleInstance();
            builder.RegisterType<ShopManager>().AsSelf().As<IStartable>().SingleInstance();

            builder.RegisterType<DiscordBot>().As<IDiscordBot>().SingleInstance();
            builder.RegisterType<CliParser>().As<ICliParser>().SingleInstance();
            builder.RegisterType<ItemImageGrabber>().As<IItemImageGrabber>().SingleInstance();
            builder.RegisterType<BankWidget>().As<IWidget<BankWidgetOptions>>().SingleInstance();
            builder.RegisterType<LootWidget>().As<IWidget<LootWidgetOptions>>().SingleInstance();
            builder.RegisterType<ShopWidget>().As<IWidget<ShopWidgetOptions>>().SingleInstance();
            builder.RegisterType<ReplyAwaiter>().As<IReplyAwaiter>().SingleInstance();
            builder.RegisterType<EquipmentWidget>().As<IWidget<EquipmentWidgetOptions>>().SingleInstance();
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

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection")));
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
            AddRsFonts(services);
        }
    }
}
