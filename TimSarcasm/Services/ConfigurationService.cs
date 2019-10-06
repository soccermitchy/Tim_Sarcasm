using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TimSarcasm.Services
{
    public class ConfigurationService
    {
        public Configuration Config { get; }
        public ConfigurationService()
        {
            Config = Configuration.FromJson(File.ReadAllText("config.json"));
        }
    }
}
