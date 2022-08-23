using Autofac;
using VectorViewerUI.Views;

namespace VectorViewerApp
{
    public static class Program
    {
        private static IContainer Container { get; } = ContainerBuilder.Build();

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(Container.Resolve<MainView>());
        }
    }
}