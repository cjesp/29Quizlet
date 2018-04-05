using _29Quizlet.Helpers;
using _29Quizlet.Models.QuizletTypes.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Repositories
{
    public class QuizletUserRepository
    {

        public async Task<User> GetUser(string UserId)
        {
            using (var _client = HttpClientHelper.GetAuthenticatedHttpClient())
            {
                var uri = $"https://api.quizlet.com/2.0/users/{UserId}?whitespace=1";
                var response = await _client.GetAsync(uri);
                var content = await response.Content.ReadAsStringAsync();

                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                var json = JsonConvert.DeserializeObject<User>(content, jsonSerializerSettings);
                return json;
            }
            //var _client = HttpClientHelper.GetAuthenticatedHttpClient();
        }


    }
}
