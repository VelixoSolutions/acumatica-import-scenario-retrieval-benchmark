using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcumaticaSoap;

namespace AcumaticaImportScenarioRetrievalBenchmark
{
    public class ImportScenarioService
    {
        private const string ImportScenariosScreenID = "SM206025";
        private const string ScenarioView = "Mappings";

        public async Task<IReadOnlyCollection<ImportScenario>> GetScenariosWithScreenApiAsync(
            string acumaticaUrl,
            string tenant,
            string username,
            string password)
        {
            var soapLoginService = new SoapLoginService();
            var soap = await soapLoginService.LoginAsync(acumaticaUrl, tenant, username, password);

            var commands = new Command[]
            {
                new Field { ObjectName = ScenarioView, FieldName = "Name" },
                new Field { ObjectName = ScenarioView, FieldName = "ScreenID" },
                new Field { ObjectName = ScenarioView, FieldName = "IsActive" },
            };

            var request = new ExportRequest(
                ImportScenariosScreenID,
                commands,
                filters: null,
                topCount: 0,
                includeHeaders: false,
                breakOnError: true);

            ExportResponse results = await soap.ExportAsync(request);

            await soap.LogoutAsync();

            return results.ExportResult
                .Where(line => string.Equals(line[2], bool.TrueString, System.StringComparison.OrdinalIgnoreCase))
                .Select(line => new ImportScenario(line[0], line[1])).ToArray();
        }
    }
}