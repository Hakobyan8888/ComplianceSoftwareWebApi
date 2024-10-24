using Newtonsoft.Json;

namespace ComplianceSoftwareWebSite.Api_Clients
{
    public class CensusApiClient
    {
        private readonly HttpClient _client;

        public CensusApiClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<Dictionary<string, string>> GetStates()
        {
            string endpoint = $"?get=NAME&for=state:*";
            var counties = await FetchFromCensusApi(endpoint);

            Dictionary<string, string> states = new Dictionary<string, string>();
            foreach (var county in counties)
            {
                // Skip the header row and get county names
                if (county[0] != "NAME")
                {
                    states.Add(county[0], county[1]);
                }
            }
            return states;
        }

        public async Task<Dictionary<string, string>> GetCountiesByStateFips(string stateFips)
        {
            string endpoint = $"?get=NAME&for=county:*&in=state:{stateFips}";
            var counties = await FetchFromCensusApi(endpoint);

            var countyNames = new Dictionary<string, string>();
            foreach (var county in counties)
            {
                // Skip the header row and get county names
                if (county[0] != "NAME")
                {
                    countyNames.Add(county[0].Split(",")[0], county[2]);
                }
            }
            return countyNames;
        }

        public async Task<List<string>> GetCitiesByStateFips(string stateFips)
        {
            string endpoint = $"?get=NAME&for=place:*&in=state:{stateFips}";
            var cities = await FetchFromCensusApi(endpoint);

            List<string> cityNames = new List<string>();
            foreach (var city in cities)
            {
                // Skip the header row and get city names
                if (city[0] != "NAME")
                {
                    cityNames.Add(city[0]);
                }
            }
            return cityNames;
        }


        public async Task<Dictionary<string, string>> GetCitiesByStateAndCountyFips(string stateFips, string countyFips)
        {
            string endpoint = $"?get=NAME&for=place/balance%20(or%20part):*&in=state:{stateFips}%20county:{countyFips}";
            var cities = await FetchFromCensusApi(endpoint);

            Dictionary<string, string> cityNames = new Dictionary<string, string>();
            foreach (var city in cities)
            {
                // Skip the header row and get city names
                if (city[0] != "NAME")
                {
                    cityNames.Add(city[0].Split(",")[0], city[3]);
                }
            }
            return cityNames;
        }

        private async Task<List<List<string>>> FetchFromCensusApi(string url)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into a List of Lists of strings
                return JsonConvert.DeserializeObject<List<List<string>>>(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Request error: " + e.Message);
                return new List<List<string>>();
            }
        }
    }
}
