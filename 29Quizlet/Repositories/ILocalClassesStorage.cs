using _29Quizlet.Models.QuizletTypes.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Repositories
{
    public interface ILocalClassesStorage
    {
        Task<IEnumerable<Classes>> GetAllClasses();
        Task AddClass(Classes clazz);
        Task DeleteClass(Classes clazz);
        Task UpdateClass(Classes clazz);
        Task DeleteAllClasses();
        Task<Classes> GetClassById(long id);
        Task<bool> SaveClasses();
    }
}
