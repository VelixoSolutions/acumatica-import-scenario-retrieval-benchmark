using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AcumaticaImportScenarioRetrievalBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This console app measures time to retrieve import scenarios from an Acumatica screen-based SOAP API.");
            Console.WriteLine();

            Console.Write("Please type in URL to Acumatica instance: ");
            string acumaticaUrl = Console.ReadLine();

            Console.Write("Tenant: ");
            string tenant = Console.ReadLine();

            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Running benchmark...");
            Console.WriteLine();

            RunBenchmark(acumaticaUrl, tenant, username, password)
                .GetAwaiter().GetResult();

            Console.WriteLine();
            Console.WriteLine("Finished!");
            Console.ReadKey();
        }

        private static async Task RunBenchmark(
            string acumaticaUrl,
            string tenant,
            string username,
            string password)
        {
            BenchmarkInfo firstBenchmark = await BenchmarkRetrievalOfImportScenariosAsync(
                acumaticaUrl,
                tenant, 
                username, 
                password);

            Console.WriteLine(
                $"First attempt: {ConvertToSeconds(firstBenchmark.Milliseconds)} sec. " +
                $"Retrieved import scenarios: {firstBenchmark.ImportScenarios.Count}");

            BenchmarkInfo secondBenchmark = await BenchmarkRetrievalOfImportScenariosAsync(
                acumaticaUrl,
                tenant, 
                username, 
                password);

            Console.WriteLine(
                $"Second attempt: {ConvertToSeconds(secondBenchmark.Milliseconds)} sec. " +
                $"Retrieved import scenarios: {secondBenchmark.ImportScenarios.Count}");
        }

        private static int ConvertToSeconds(long milliseconds) => (int)Math.Round(milliseconds / (decimal)1000);

        private static async Task<BenchmarkInfo> BenchmarkRetrievalOfImportScenariosAsync(
            string acumaticaUrl,
            string tenant,
            string username,
            string password)
        {
            var stopwatch = Stopwatch.StartNew();

            var importScenarioService = new ImportScenarioService();
            var importScenarios = await importScenarioService
                .GetScenariosWithScreenApiAsync(acumaticaUrl, tenant, username, password)
                .ConfigureAwait(false);

            stopwatch.Stop();
            return new BenchmarkInfo(stopwatch.ElapsedMilliseconds, importScenarios);
        }

        private class BenchmarkInfo
        {
            public long Milliseconds { get; }
            public IReadOnlyCollection<ImportScenario> ImportScenarios { get; }

            public BenchmarkInfo(long milliseconds, IReadOnlyCollection<ImportScenario> importScenarios)
            {
                Milliseconds = milliseconds;
                ImportScenarios = importScenarios;
            }
        }
    }
}
