using HieuTM.ConsoleUI.Models;
using Newtonsoft.Json;
using System.Text;

namespace HieuTM.ConsoleUI.UI
{
    internal class HttpClientSingleton
    {
        private static readonly HttpClient _instance = new HttpClient();

        static HttpClientSingleton()
        {
        }

        public static HttpClient Instance => _instance;
    }

    internal class SinhVienUIService
    {
        string baseUrl = "https://localhost:7283/";

        public async Task Get()
        {
            string urlGet = $"{baseUrl}api/SinhViens";
            HttpResponseMessage response = await HttpClientSingleton.Instance.GetAsync(urlGet);

            //response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("[" + (int)response.StatusCode + "] " + response.StatusCode);

                return;
            }

            // Get data response
            string data = await response.Content.ReadAsStringAsync();
            var vms = JsonConvert.DeserializeObject<List<SinhVienVM>>(data);

            Console.WriteLine(data);
            foreach (var vm in vms)
            {
                vm.Show();
            }
        }

        public async Task Post()
        {
            string urlGet = $"{baseUrl}api/SinhViens";
            SinhVienCreateVM vm = new();

            string jsonRequest = JsonConvert.SerializeObject(vm);
            HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await HttpClientSingleton.Instance.PostAsync(urlGet, content);

            Console.WriteLine("[" + (int)response.StatusCode + "] " + response.StatusCode);
        }
    }
}
