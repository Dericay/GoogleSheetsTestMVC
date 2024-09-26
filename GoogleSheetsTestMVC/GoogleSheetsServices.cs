using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GoogleSheetsTest.Services
{
    public class GoogleSheetsService
    {
        private readonly IConfiguration _configuration;
        private readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private readonly string ApplicationName = "GoogleSheetsTest";
        private readonly string SpreadsheetId;
        private readonly string ServiceAccountKeyPath;
        private SheetsService _service;

        public GoogleSheetsService(IConfiguration configuration)
        {
            _configuration = configuration;

            
            SpreadsheetId = _configuration["GoogleSheets:SpreadsheetId"];
            ServiceAccountKeyPath = _configuration["GoogleSheets:ServiceAccountKeyPath"];

            // Load Google Credentials
            GoogleCredential credential;
            using (var stream = new FileStream(ServiceAccountKeyPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }

            _service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        public IList<IList<object>> ReadEntries(string range)
        {
            try
            {
                var request = _service.Spreadsheets.Values.Get(SpreadsheetId, range);
                var response = request.Execute();
                return response.Values;
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading data from Google Sheets: " + ex.Message);
            }
        }
    }
}
