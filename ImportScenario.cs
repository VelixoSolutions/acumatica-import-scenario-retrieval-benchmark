namespace AcumaticaImportScenarioRetrievalBenchmark
{
    public class ImportScenario
    {
        public string ImportScenarioName { get; }
        public string ScreenID { get; }

        public ImportScenario(string importScenarioName, string screenId)
        {
            ImportScenarioName = importScenarioName;
            ScreenID = screenId;
        }
    }
}