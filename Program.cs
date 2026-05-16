using System;
using System.Threading;
using System.Windows.Forms;

namespace FoodOrderingSystem;

static class Program
{
    /// <summary>

    /// </summary>
    [STAThread]
    static void Main()
    {
        // global exception handlers
        Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

        ApplicationConfiguration.Initialize();
        Application.Run(new Forms.LoginForm());
    }

    private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
        ShowErrorMessage(e.Exception);
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            ShowErrorMessage(ex);
        }
    }

    private static void ShowErrorMessage(Exception ex)
    {
        MessageBox.Show($"An unexpected error occurred:\n\n{ex.Message}\n\nThe application will try to continue running.", 
            "Application Error", 
            MessageBoxButtons.OK, 
            MessageBoxIcon.Error);
    }
}