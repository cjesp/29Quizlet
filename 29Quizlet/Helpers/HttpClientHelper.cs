using _29Quizlet.Services.SettingsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Helpers
{
    public static class HttpClientHelper
    {
        public static HttpClient GetAuthenticatedHttpClient()
        {
            var settingsService = SettingsService.Instance;
            var _client = new HttpClient();


            if (settingsService.UserSettings != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settingsService.UserSettings.Token);
            }
        
            return _client;
        }
    }
}
