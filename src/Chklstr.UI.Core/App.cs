using Chklstr.Core.Services;
using Chklstr.Infra.Parser;
using Chklstr.UI.Core.ViewModels;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Serilog;

namespace Chklstr.UI.Core;

public class App : MvxApplication
{
    public override void Initialize()
    {
        Mvx.IoCProvider.RegisterType<IQRHParserService, QRHParserService>();
        Mvx.IoCProvider.ConstructAndRegisterSingleton<ApplicationViewModel, ApplicationViewModel>();

        RegisterAppStart<ApplicationViewModel>();
    }
    
    
}