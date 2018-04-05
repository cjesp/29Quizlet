using _29Quizlet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Repositories
{
    public interface ISetFetcher
    {
        Task<Set> GetSet(long id);
        Task<Set> GetPrivateSet(long id, string password);
    }
}
