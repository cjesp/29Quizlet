using _29Quizlet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Repositories
{
    public interface ISetsLocalStorage
    {
        Task<IEnumerable<Set>> GetAllSets();
        Task<Set> GetSetById(long id);
        Task AddSet(Set set);
        Task UpdateSet(Set set);
        Task DeleteSet(Set set);
        Task PurgeAndAllNewSets(IEnumerable<Set> sets);
        Task DeleteAllSets();
        Task<bool> SaveSets();
    }
}
