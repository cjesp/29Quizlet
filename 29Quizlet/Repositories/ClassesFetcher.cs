using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _29Quizlet.Models.QuizletTypes.User;

namespace _29Quizlet.Repositories
{
    public class ClassesFetcher : IClassFetcher
    {
        private readonly IQuizletRESTApi _quizletRESTApi;

        public ClassesFetcher(IQuizletRESTApi quizletRESTApi)
        {
            _quizletRESTApi = quizletRESTApi;
        }

        public async Task<Classes> GetClass(int id)
        {
            var classes = await _quizletRESTApi.GetClass(id);
            return classes;
        }
    }
}
