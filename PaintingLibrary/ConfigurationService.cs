using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace PaintingLibrary
{
    using Microsoft.Extensions.Configuration;
    using System.IO;

    public class ConfigurationService
    {
        public IConfigurationRoot Configuration { get; }

        public ConfigurationService()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }
    }

}
