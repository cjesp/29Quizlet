using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _29Quizlet.Models;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.IO;
using _29Quizlet.Models.QuizletTypes.Search;
using _29Quizlet.Helpers;
using Newtonsoft.Json;
using _29Quizlet.Models.QuizletTypes.User;
using _29Quizlet.Repositories.Exceptions;
using _29Quizlet.Models.QuizletTypes.Uploading;
using _29Quizlet.Models.QuizletTypes.Feeds;
using _29Quizlet.Models.QuizletTypes.Feeds.Home;
using _29Quizlet.Models.QuizletTypes.Uploading.ClassResponse;

namespace _29Quizlet.Repositories
{
    public class QuizletRemoteRESTApi : IQuizletRESTApi
    {
        private const string GET_PUBLIC_SINGLE_SET = "https://api.quizlet.com/2.0/sets/{0}?client_id=&whitespace=1";
        private const string GET_PRIVATE_SINGLE_SET = "https://api.quizlet.com/2.0/sets/{0}";
        private const string GET_PRIVATE_SET = "https://api.quizlet.com/2.0/sets/{0}/password?client_id=XXX&whitespace=1";
        private const string GET_USER_BASE = "https://api.quizlet.com/2.0/users/{0}/{1}?client_id=XXX&whitespace=1";
        private const string GET_CLASS_SETS = "https://api.quizlet.com/2.0/classes/{0}/sets?client_id=XXX&whitespace=1";
        private const string GET_CLASS_BASE = "https://api.quizlet.com/2.0/classes/{0}?client_id=XXX&whitespace=1";
        private const string GET_USER_FEED_SETS = "https://api.quizlet.com/2.0/feed/created";
        private const string GET_USER_FEED_STUDY_SESSIONS = "https://api.quizlet.com/2.0/feed/studied";
        private const string CREATE_SET = "https://api.quizlet.com/2.0/sets";
        private const string CREATE_CLASS = "https://api.quizlet.com/2.0/classes";
        private const string DELETE_UPDATE_CLASS = "https://api.quizlet.com/2.0/classes/{0}";
        private const string ADD_REMOVE_SET_TO_CLASS = "https://api.quizlet.com/2.0/classes/{0}/sets/{1}";
        private const string DELETE_UPDATE_SET = "https://api.quizlet.com/2.0/sets/{0}";
        private const string GET_HOME_FEED = "https://api.quizlet.com/2.0/feed/home";



        public async Task<Classes> GetClass(int id)
        {
            using (var client = HttpClientHelper.GetAuthenticatedHttpClient())
            {
                var requestUri = new Uri(String.Format(GET_CLASS_BASE, $"{id}"));
                var response = await client.GetAsync(requestUri);
                var responseText = await response.Content.ReadAsStringAsync();
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                var quizClass = JsonConvert.DeserializeObject<Classes>(responseText, jsonSerializerSettings);
                return quizClass;
            }
        }

        public async Task<IEnumerable<Set>> GetSets(Classes quizClass)
        {
            using (var client = HttpClientHelper.GetAuthenticatedHttpClient())
            {
                var requestUri = new Uri(String.Format(GET_CLASS_SETS, quizClass.Id));
                var response = await client.GetAsync(requestUri);
                var responseText = await response.Content.ReadAsStringAsync();
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                var sets = JsonConvert.DeserializeObject<List<Set>>(responseText, jsonSerializerSettings);
                return sets;
            }
        }

        public async Task<IEnumerable<Classes>> GetClasses(User user)
        {
            using (var client = HttpClientHelper.GetAuthenticatedHttpClient())
            {
                var requestUri = new Uri(String.Format(GET_USER_BASE, user.Username, "classes"));
                var response = await client.GetAsync(requestUri);
                var responseText = await response.Content.ReadAsStringAsync();
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                var classes = JsonConvert.DeserializeObject<List<Classes>>(responseText, jsonSerializerSettings);
                return classes;
            }
        }

        public async Task<IEnumerable<StudySession>> GetPublicStudySessions(User user)
        {
            using (var client = HttpClientHelper.GetAuthenticatedHttpClient())
            {
                var requestUri = new Uri(String.Format(GET_USER_BASE, user.Username, "studied"));
                var response = await client.GetAsync(requestUri);
                var responseText = await response.Content.ReadAsStringAsync();
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                var sessions = JsonConvert.DeserializeObject<List<StudySession>>(responseText, jsonSerializerSettings);
                return sessions;
            }
        }

        public async Task<StudySessionsFeed> GetFeedStudySessions()
        {
            using (var client = HttpClientHelper.GetAuthenticatedHttpClient())
            {
                var requestUri = new Uri(GET_USER_FEED_STUDY_SESSIONS);
                var response = await client.GetAsync(requestUri);
                var responseText = await response.Content.ReadAsStringAsync();
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                var sessions = JsonConvert.DeserializeObject<StudySessionsFeed>(responseText, jsonSerializerSettings);
                return sessions;
            }
        }

        public async Task<IEnumerable<Set>> GetSets(User user)
        {
            using (var client = HttpClientHelper.GetAuthenticatedHttpClient())
            {
                var requestUri = new Uri(String.Format(GET_USER_BASE, user.Username, "sets"));
                var response = await client.GetAsync(requestUri);
                var responseText = await response.Content.ReadAsStringAsync();
                var jsonSerializerSettings = new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var sets = JsonConvert.DeserializeObject<IEnumerable<Set>>(responseText, jsonSerializerSettings);
                return sets;
            }
        }

        public async Task<Set> GetPublicSet(long id)
        {
            var requestUri = new Uri(String.Format(GET_PUBLIC_SINGLE_SET, id));
            using (var _client = HttpClientHelper.GetAuthenticatedHttpClient())
            {
                var response = await _client.GetAsync(requestUri);
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    throw new SetIsPrivateException();
                }
                var responseText = await response.Content.ReadAsStringAsync();
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                var set = JsonConvert.DeserializeObject<Set>(responseText, jsonSerializerSettings);
                return set;
                
            }
        }

        public async Task<Set> GetPrivateSet(long id)
        {
            var requestUri = new Uri(String.Format(GET_PRIVATE_SINGLE_SET, id));
            using (var _client = HttpClientHelper.GetAuthenticatedHttpClient())
            {
                var response = await _client.GetAsync(requestUri);
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    throw new SetIsPrivateException();
                }
                var responseText = await response.Content.ReadAsStringAsync();
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                var set = JsonConvert.DeserializeObject<Set>(responseText, jsonSerializerSettings);
                return set;

            }
        }

        public async Task<bool> GetPrivateSet(long id, string password)
        {
            var requestUri = new Uri(String.Format(GET_PRIVATE_SET, id));
            using (var _client = HttpClientHelper.GetAuthenticatedHttpClient())
            {
                //var content = new StringContent($"password: {password}");
                var formContent = new FormUrlEncodedContent(new Dictionary<string, string> { { "password", password }, { "whitespace", "true" } });

                var response = await _client.PostAsync(requestUri, formContent);
                var responseText = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || !response.IsSuccessStatusCode)
                {
                    return false;
                }
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }

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
        }

        public async Task<IEnumerable<Set>> GetSets(params long[] ids)
        {
            var setList = new List<Set>();

            foreach (var id in ids)
            {
                var set = await GetPublicSet(id);
                setList.Add(set);
            }

            return setList;
        }

        public async Task<SearchResult> SearchUniversal(string term)
        {
            var pageSize = 100;
            var searchString = $"https://api.quizlet.com/2.0/search/universal?q={term}&per_page={pageSize}";

            using (var _client = HttpClientHelper.GetAuthenticatedHttpClient())
            {
                var response = await _client.GetAsync(searchString);
                if (!response.IsSuccessStatusCode)
                {
                    return SearchResult.NoResults();
                }

                //else when we received a success status code!

                var responseText = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<SearchResult>(responseText, new JsonSearchConverter());
                result.Success = true;
                return result;
            }

        }

        public async Task<bool> CreateNewSet(CreateSet set)
        {
            var setJson = JsonConvert.SerializeObject(set);
            using (var _client = HttpClientHelper.GetAuthenticatedHttpClient())
            {
                var content = new StringContent(setJson, Encoding.UTF8, "application/json");

                var result = await _client.PostAsync(CREATE_SET, content);

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        public async Task<bool> DeleteSet(long id)
        {
            var uri = new Uri(string.Format(DELETE_UPDATE_SET, id));
            using (var _client = HttpClientHelper.GetAuthenticatedHttpClient())
            {
                var result = await _client.DeleteAsync(uri);
                return result.IsSuccessStatusCode;
            }
        }

        public async Task<bool> UpdateSet(UpdateSet set, long id)
        {
            var setJson = JsonConvert.SerializeObject(set);
            var uri = new Uri(string.Format(DELETE_UPDATE_SET, id));
            using (var _client = HttpClientHelper.GetAuthenticatedHttpClient())
            {
                var content = new StringContent(setJson, Encoding.UTF8, "application/json");

                var result = await _client.PutAsync(uri, content);

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        public async Task<CreatedFeed> GetCreatedSets()
        {
            using (var client = HttpClientHelper.GetAuthenticatedHttpClient())
            {
                //var requestUri = new Uri(String.Format(GET_USER_BASE, user.Username, "studied"));
                var uri = new Uri(GET_USER_FEED_SETS);
                var response = await client.GetAsync(uri);
                var responseText = await response.Content.ReadAsStringAsync();
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                var feeds = JsonConvert.DeserializeObject<CreatedFeed>(responseText, jsonSerializerSettings);
                return feeds;
            }
        }

        public async Task<HomeFeed> GetHomeFeed()
        {
            //var pageSize = 100;
            var uri = new Uri(GET_HOME_FEED);

            using (var _client = HttpClientHelper.GetAuthenticatedHttpClient())
            {
                var response = await _client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                {
                    //return SearchResult.NoResults();
                    return null;
                }

                //else when we received a success status code!

                var responseText = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<HomeFeed>(responseText, new JsonHomeFeedConverter());

                return result;
            }
        }

        public async Task<int?> AddClass(CreateClass clazz)
        {
            var classJson = JsonConvert.SerializeObject(clazz);
            using (var _client = HttpClientHelper.GetAuthenticatedHttpClient())
            {
                var content = new StringContent(classJson, Encoding.UTF8, "application/json");

                var result = await _client.PostAsync(CREATE_CLASS, content);
                

                if (result.IsSuccessStatusCode)
                {
                    var responseText = await result.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<ClassCreatedResponse>(responseText);
                    return res.ClassId;
                }
                else
                {
                    return null;
                }

            }
        }

        public async Task<bool> AddSetsToClass(int classId, params long[] ids)
        {
            using (var _client = HttpClientHelper.GetAuthenticatedHttpClient())
            {
                foreach (var id in ids)
                {
                    var uri = new Uri(string.Format(ADD_REMOVE_SET_TO_CLASS, classId, id));
                    var response = await _client.PutAsync(uri, null);

                    if (!response.IsSuccessStatusCode)
                    {
                        return false;
                    }

                }
            }

            return true;
        }

        public async Task<bool> DeleteClass(int id)
        {
            var uri = new Uri(string.Format(DELETE_UPDATE_CLASS, id));

            using (var _client = HttpClientHelper.GetAuthenticatedHttpClient())
            {
                var res = await _client.DeleteAsync(uri);
                return res.IsSuccessStatusCode;
            }
        }
    }
}
