using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;

        private string _supportEmail;
        public string SupportEmail
        {
            get => _supportEmail;
        }

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
            _supportEmail = _configuration.GetValue<string>("App:Email:Notification");
            if (_supportEmail == null)
            {
                throw new ArgumentNullException(nameof(_supportEmail));
            }
        }
    }
}
