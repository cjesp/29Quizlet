using _29Quizlet.Models.QuizletTypes.Feeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Repositories
{
    public interface ILocalRecentStudySessions
    {
        Task<IEnumerable<StudySessionItem>> GetAllSessions();
        Task AddStudySessionRange(IEnumerable<StudySessionItem> session);
        //Task DeleteStudySession(StudySession session);
        //Task UpdateStudySession(StudySession session);
        Task DeleteAllStudySessions();
        //Task<StudySession> GetStudySessionById(long id);
        Task<bool> SaveStudySessions();
    }
}
