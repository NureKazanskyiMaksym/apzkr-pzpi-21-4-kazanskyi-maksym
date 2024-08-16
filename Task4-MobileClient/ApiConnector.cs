using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EquipmentWatcherMAUI
{
    public class ApiConnector
    {
        private HttpClient client = new HttpClient();

        public ApiConnector(string userData)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", userData);
        }

        public void Get<T>(string path, Action<ResponseObject<T>?, string?> action) where T : class
        {
            string? error = null;
            var response = client.GetAsync(StaticReference.API_URL + path, HttpCompletionOption.ResponseHeadersRead).Result;

            var responseString = response.Content.ReadAsStringAsync().Result;

            if (responseString == "")
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    error = $"Wrong login or password";
                }
                else
                {
                    error = $"Wrong response: {responseString}";
                }
                action(null, error);
                return;
            }

            var responseObject = JsonSerializer.Deserialize<ResponseObject<T>>(responseString);

            if (responseObject == null)
            {
                error = $"Could not convert response object: {responseString}";
            }

            action(responseObject, error);
        }

        public void Post<T>(string path, T data, Action<ResponseObject<T>?, string?> action) where T : class
        {
            string? error = null;
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = client.PostAsync(StaticReference.API_URL + path, content).Result;

            var responseString = response.Content.ReadAsStringAsync().Result;

            if (responseString == "")
            {
                error = $"Wrong response: {responseString}";
                action(null, error);
                return;
            }

            var responseObject = JsonSerializer.Deserialize<ResponseObject<T>>(responseString);

            if (responseObject == null)
            {
                error = $"Could not convert response object: {responseString}";
            }

            action(responseObject, error);
        }

        public void Delete<T>(string path, Action<ResponseObject<T>> action) where T : class
        {
            var response = client.DeleteAsync(StaticReference.API_URL + path).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;
                var responseObject = JsonSerializer.Deserialize<ResponseObject<T>>(responseString);

                action(responseObject);
            }
        }
    }
}
