using Azure.Identity;
using BerkutPoiService.Interfaces;
using BerkutPoiService.Manager;
using BerkutPoiService.Options;
using BerkutPoiService.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(BerkutPoiService.Startup))]
namespace BerkutPoiService
{
    public class Startup : FunctionsStartup
    {
        private IConfigurationRoot _functionConfig;
        private readonly StorageServiceOptions _storageServiceOptions = new();
        public override void Configure(IFunctionsHostBuilder builder)
        {
            _functionConfig = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            builder.Services.Configure<StorageServiceOptions>(_functionConfig.GetSection(nameof(StorageServiceOptions)));

            _functionConfig.GetSection(nameof(StorageServiceOptions)).Bind(_storageServiceOptions);

            builder.Services.AddScoped<IGeoHashService, GeoHashService>();
            builder.Services.AddScoped<IStorageService, StorageService>();
            builder.Services.AddScoped<IRequestValidator, RequestValidator>();

            builder.Services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.UseCredential(new DefaultAzureCredential());
                clientBuilder.AddTableServiceClient(_storageServiceOptions.StorageAccountConnectionString);
            });
        }
    }
}
