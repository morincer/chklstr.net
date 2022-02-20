using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Chklstr.Core.Services.Voice;
using Chklstr.UI.Core.ViewModels;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.ViewModels;
using Serilog;
using static MvvmCross.Core.MvxSetupExtensions;
using ILogger = Serilog.ILogger;
using MvxApplication = MvvmCross.Platforms.Wpf.Views.MvxApplication;

namespace Chklstr.UI.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MvxApplication
    {
        public App()
        {
            this.RegisterSetupType<Setup>();
            this.DispatcherUnhandledException += OnUnhandledException;
        }

        

        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Logger.Fatal(e.Exception, e.Exception.Message);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var applicationViewModel = Mvx.IoCProvider.Resolve<ApplicationViewModel>();
            applicationViewModel.Exit();
            Process.GetCurrentProcess().Kill();
        }
    }
}