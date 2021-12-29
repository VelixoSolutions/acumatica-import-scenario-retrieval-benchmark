using System;
using System.ServiceModel;
using System.Threading.Tasks;
using AcumaticaSoap;

namespace AcumaticaImportScenarioRetrievalBenchmark
{
    public class SoapLoginService
    {
        public async Task<ScreenSoap> LoginAsync(
            string acumaticaUrl,
            string tenant,
            string username,
            string password)
        {
            var binding = new BasicHttpBinding
            {
                Name = "ScreenSoap",
                AllowCookies = true,
                MaxReceivedMessageSize = 2147483647,
                SendTimeout = TimeSpan.MaxValue,
                ReceiveTimeout = TimeSpan.MaxValue
            };

            if (acumaticaUrl.EndsWith("/") == false)
            {
                acumaticaUrl = $"{acumaticaUrl}/";
            }

            var address = new EndpointAddress(acumaticaUrl + "Soap/.asmx");
            if (address.Uri.Scheme == Uri.UriSchemeHttps)
            {
                binding.Security.Mode = BasicHttpSecurityMode.Transport;
            }

            var screen = new ScreenSoapClient(binding, address);

            await screen.LoginAsync($"{username}@{tenant}", password).ConfigureAwait(false);

            await screen.SetLocaleNameAsync("en-US").ConfigureAwait(false);
            return screen;
        }
    }
}