using HieuTM.ConsoleUI.Models;
using Newtonsoft.Json;
using System.Text;

namespace HieuTM.ConsoleUI.UI
{
    internal class SinhVienUIService
    {
        string baseUrl = "https://localhost:7283/";
        public async Task Get()
        {
            HttpClient clientGet = new();
            string urlGet = $"{baseUrl}api/SinhViens";
            HttpResponseMessage response = await clientGet.GetAsync(urlGet);

            //response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("[" + (int)response.StatusCode + "] " + response.StatusCode);
            }

            // Get data response
            string data = await response.Content.ReadAsStringAsync();
            var vms = JsonConvert.DeserializeObject<List<SinhVienVM>>(data);

            Console.WriteLine(data);
            foreach (var vm in vms)
            {
                vm.Show();
            }

            clientGet.Dispose();
        }

        public async Task Post()
        {
            HttpClient clientPost = new();
            string urlGet = $"{baseUrl}api/SinhViens";
            SinhVienCreateVM vm = new();

            string jsonRequest = JsonConvert.SerializeObject(vm);
            HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await clientPost.PostAsync(urlGet, content);

            Console.WriteLine("[" + (int)response.StatusCode + "] " + response.StatusCode);
            clientPost.Dispose();
        }
    }
}
