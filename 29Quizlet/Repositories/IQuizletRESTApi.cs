using _29Quizlet.Models;
using _29Quizlet.Models.QuizletTypes.Feeds;
using _29Quizlet.Models.QuizletTypes.Feeds.Home;
using _29Quizlet.Models.QuizletTypes.Search;
using _29Quizlet.Models.QuizletTypes.Uploading;
using _29Quizlet.Models.QuizletTypes.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Repositories
{
    public interface IQuizletRESTApi
    {
        Task<IEnumerable<Classes>> GetClasses(User user);
        Task<Classes> GetClass(int id);
        Task<IEnumerable<StudySession>> GetPublicStudySessions(User user);
        Task<StudySessionsFeed> GetFeedStudySessions();
        Task<IEnumerable<Set>> GetSets(User user);
        Task<IEnumerable<Set>> GetSets(Classes quizClass);
        Task<Set> GetPublicSet(long id);
        Task<Set> GetPrivateSet(long id);
        Task<Boolean> GetPrivateSet(long id, string password);
        Task<IEnumerable<Set>> GetSets(params long[] ids);
        Task<SearchResult> SearchUniversal(string term);
        Task<User> GetUser(string id);
        Task<bool> CreateNewSet(CreateSet set);
        Task<bool> DeleteSet(long id);
        Task<bool> UpdateSet(UpdateSet set, long id);
        Task<CreatedFeed> GetCreatedSets();
        Task<HomeFeed> GetHomeFeed();
        Task<int?> AddClass(CreateClass clazz);
        Task<bool> DeleteClass(int id);
        Task<bool> AddSetsToClass(int classId, params long[] ids);
    }
}
