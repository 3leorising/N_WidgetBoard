using Microsoft.Extensions.Logging;
using WidgetBoard.Pages;
using WidgetBoard.ViewModels;

namespace WidgetBoard
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddTransient<BoardDetailsPage>();
            builder.Services.AddTransient<FixedBoardPage>();
            builder.Services.AddTransient<AppShellViewModel>();
            builder.Services.AddTransient<BoardDetailsPageViewModel>();
            builder.Services.AddTransient<FixedBoardPageViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}