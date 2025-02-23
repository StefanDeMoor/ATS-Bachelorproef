using ATS.ViewModels;
using ATS.Views;
using Microsoft.Extensions.Logging;

namespace ATS
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

#if DEBUG
    		builder.Logging.AddDebug();

            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<HomePage>();
            
            builder.Services.AddSingleton<LoginPageViewModel>(); 
#endif

            return builder.Build();
        }
    }
}
