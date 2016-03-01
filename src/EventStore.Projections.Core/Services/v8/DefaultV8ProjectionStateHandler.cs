using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using EventStore.Common.Utils;
using EventStore.Core.Util;
using System.Reflection;

namespace EventStore.Projections.Core.Services.v8
{
    public class DefaultV8ProjectionStateHandler : V8ProjectionStateHandler
    {
        private static readonly string _resourcePath = Locations.PreludeResourcesPath;

        public DefaultV8ProjectionStateHandler(
            string query, Action<string, object[]> logger, Action<int, Action> cancelCallbackFactory)
            : base("1Prelude", query, GetModuleSource, logger, cancelCallbackFactory)
        {
        }

        public static Tuple<string, string> GetModuleSource(string name)
        {
            var resourceName = string.Format("{0}.{1}.js", _resourcePath, name);
            var assembly = Assembly.GetAssembly(typeof(ProjectionManagerNode));
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, Helper.UTF8NoBom))
            {
                var result = reader.ReadToEnd();
                return Tuple.Create(result, resourceName);
            }
        }
    }
}
