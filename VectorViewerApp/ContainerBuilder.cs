using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VectorViewerLibrary.DataReading;
using VectorViewerLibrary.DataReading.Converters;
using VectorViewerUI;
using VectorViewerUI.Views;
using ColorConverter = VectorViewerLibrary.DataReading.Converters.ColorConverter;

namespace VectorViewerApp
{
    public static class ContainerBuilder
    {
        public static IContainer Build()
        {
            var builder = new Autofac.ContainerBuilder();

            builder.RegisterType<MainView>();

            builder.RegisterType<EditorView>();

            builder.RegisterType<GraphicsRenderer>();

            builder.RegisterType<DataProvider>()
                .As<IDataProvider>()
                .InstancePerLifetimeScope();

            builder.RegisterType<JsonFileParser>()
                .Keyed<IFileParser>("JSON")
                .InstancePerLifetimeScope();

            builder.RegisterType<DxfFileParser>()
                .Keyed<IFileParser>("DXF")
                .InstancePerLifetimeScope();

            builder.Register(CreateJsonSerializerSettings)
                .SingleInstance();

            return builder.Build();
        }

        private static JsonSerializerSettings CreateJsonSerializerSettings(IComponentContext ctx)
        {
            return new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy { OverrideSpecifiedNames = false }
                },
                Converters = new List<JsonConverter>
                {
                    new ColorConverter(),
                    new PointFConverter()
                }
            };
        }
    }
}
