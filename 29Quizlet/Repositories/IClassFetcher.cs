using _29Quizlet.Models.QuizletTypes.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Repositories
{
    public interface IClassFetcher
    {
        Task<Classes> GetClass(int id);
    }
}
