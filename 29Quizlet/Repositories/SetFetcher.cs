using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _29Quizlet.Models;

namespace _29Quizlet.Repositories
{
    public class SetFetcher : ISetFetcher
    {
        private readonly ISetsLocalStorage _setsLocalStorage;
        private readonly IQuizletRESTApi _quizletRESTApi;

        public SetFetcher(
            ISetsLocalStorage setsLocalStorage,
            IQuizletRESTApi quizletRESTApi)
        {
            this._setsLocalStorage = setsLocalStorage;
            this._quizletRESTApi = quizletRESTApi;
        }

        public async Task<Set> GetPrivateSet(long id, string password)
        {
            var ok = await _quizletRESTApi.GetPrivateSet(id, password);
            if (ok)
            {
                var set = await _quizletRESTApi.GetPrivateSet(id);
                await _setsLocalStorage.AddSet(set);
                await _setsLocalStorage.SaveSets();
                return set;
            }

            return null;

        }

        public async Task<Set> GetSet(long id)
        {
            var local = await _setsLocalStorage.GetSetById(id);
            if (local != null)
            {
                if (local.Terms == null)
                {
                    Set remoteSet = null;

                    if (SetIsPrivate(local.Visibility))
                    {
                        remoteSet = await _quizletRESTApi.GetPrivateSet(local.Id);
                    }
                    else
                    {
                        remoteSet = await _quizletRESTApi.GetPublicSet(local.Id);
                    }
                }
                return local;
            }
            else
            {
                var remoteSet = await _quizletRESTApi.GetPublicSet(id);
                return remoteSet;
            }

        }

        private bool SetIsPrivate(string visibility)
        {
            return visibility == "password" || visibility == "class";
        }
    }
}
