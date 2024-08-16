using EquipmentWatcherMAUI.Models;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace EquipmentWatcherMAUI
{
    internal class StaticReference
    {
        public static string API_URL {
            get
            {
#if DEBUG
                switch (DeviceInfo.DeviceType)
                {
                    case DeviceType.Virtual:
                        return "http://10.0.2.2:5120/api";
                    default:
                        return "http://192.168.1.103:5120/api";
                }
#else
                return "http://192.168.1.103:5120/api";
#endif
            }
        }
        public static ApiConnector ApiConnector { get; set; }
        public static UserModel CurrentUser { get; set; }
        public static PermissionModel[] CurrentPermissions { get; set; }
        public static AccessTokenModel CurrentAccessToken { get; set; }

        public async static void UpdateAccessToken()
        {
            var userData = await SecureStorage.GetAsync("userData");

            if (userData != null)
            {
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", userData);

                var checkResponse = await client.GetAsync(API_URL + "/AccessToken/own");

                if (checkResponse.IsSuccessStatusCode)
                {
                    var checkResponseString = await checkResponse.Content.ReadAsStringAsync();
                    var checkResponseObject = JsonSerializer.Deserialize<ResponseObject<AccessTokenModel>>(checkResponseString);

                    if (checkResponseObject != null && checkResponseObject.Success && checkResponseObject.Result.ExpiresOn > DateTime.UtcNow.AddMinutes(5))
                    {
                        CurrentAccessToken = checkResponseObject.Result;
                        return;
                    }
                }

                var updateResponse = await client.PutAsync(API_URL + $"/AccessToken?dateTime={UrlEncoder.Default.Encode(DateTime.UtcNow.AddMinutes(30).ToString())}", null);

                if (updateResponse.IsSuccessStatusCode)
                {
                    var responseString = await updateResponse.Content.ReadAsStringAsync();
                    var responseObject = JsonSerializer.Deserialize<ResponseObject<AccessTokenModel>>(responseString);

                    if (responseObject != null && responseObject.Success)
                    {
                        CurrentAccessToken = responseObject.Result;
                    }
                }
            }
        }
    }
}
