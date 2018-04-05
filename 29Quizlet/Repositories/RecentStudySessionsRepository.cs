using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _29Quizlet.Models.QuizletTypes.Feeds;
using Windows.Storage;
using System.Runtime.Serialization.Json;
using System.IO;

namespace _29Quizlet.Repositories
{
    public class RecentStudySessionsRepository : ILocalRecentStudySessions
    {
        private const string LOCAL_STUDY_SESSIONS_FOLDER = "STUDYSESSIONS";
        private const string LOCAL_STUDY_SESSIONS_FILE = "STUDYSESSIONS.json";
        private StorageFolder _localFolder = ApplicationData.Current.LocalFolder;
        private List<StudySessionItem> _inMemSessions;
        private bool _clearAllSessions = false;

        public RecentStudySessionsRepository()
        {
            _inMemSessions = new List<StudySessionItem>();
        }

        public async Task AddStudySessionRange(IEnumerable<StudySessionItem> sessions)
        {
            _inMemSessions.Clear();

            if (sessions.Any())
            {
                foreach (var ses in sessions)
                {
                    _inMemSessions.Add(ses);
                }
                await SaveStudySessions();
            }

        }

        //public async Task AddStudySession(StudySessionItem session)
        //{
        //    var sessionInMem = _inMemSessions.Where(x => x.Id == session.Id)
        //                        .SingleOrDefault();
        //    if (sessionInMem == null)
        //    {
        //        _inMemSessions.Add(session);
        //        await SaveStudySessions();
        //    }
        //}

        public async Task DeleteAllStudySessions()
        {
            _clearAllSessions = true;
            _inMemSessions.Clear();
            await SaveStudySessions();
        }

        //public async Task DeleteStudySession(StudySessionItem session)
        //{
        //    var sesInMem = _inMemSessions.Where(x => x.Id == session.Id)
        //                    .SingleOrDefault();
        //    if (sesInMem != null)
        //    {
        //        _inMemSessions.Remove(sesInMem);
        //        await SaveStudySessions();
        //    }
        //}

        public async Task<IEnumerable<StudySessionItem>> GetAllSessions()
        {
            await ReadFromStorage();
            return _inMemSessions;
        }

        //public async Task<StudySessionItem> GetStudySessionById(long id)
        //{
        //    var sesInMem = _inMemSessions.Where(x => x.Id == id)
        //        .SingleOrDefault();

        //    return sesInMem;
        //}

        public async Task<bool> SaveStudySessions()
        {
            if (_inMemSessions.Count > 0 || _clearAllSessions)
            {
                _clearAllSessions = false;
                var serializer = new DataContractJsonSerializer(typeof(IList<StudySessionItem>));
                var folderInit = await _localFolder.TryGetItemAsync(LOCAL_STUDY_SESSIONS_FOLDER);

                if (folderInit == null)
                {
                    await _localFolder.CreateFolderAsync(LOCAL_STUDY_SESSIONS_FOLDER, CreationCollisionOption.ReplaceExisting);
                }

                var classesFolder = await _localFolder.GetFolderAsync(LOCAL_STUDY_SESSIONS_FOLDER);
                using (var stream = await classesFolder.OpenStreamForWriteAsync(LOCAL_STUDY_SESSIONS_FILE, CreationCollisionOption.ReplaceExisting))
                {
                    try
                    {
                        serializer.WriteObject(stream, _inMemSessions);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

            }

            return true;
        }

        //public async Task UpdateStudySession(StudySessionItem session)
        //{
        //    var sesInMem = _inMemSessions.Where(x => x.Id == session.Id)
        //        .SingleOrDefault();
        //    if (sesInMem != null)
        //    {
        //        _inMemSessions.Remove(sesInMem);
        //        _inMemSessions.Add(session);
        //        await SaveStudySessions();
        //    }
        //}

        private async Task ReadFromStorage()
        {
            if (_inMemSessions.Count < 1)
            {
                var serializer = new DataContractJsonSerializer(typeof(IList<StudySessionItem>));
                var folderInit = await _localFolder.TryGetItemAsync(LOCAL_STUDY_SESSIONS_FOLDER);

                if (folderInit == null)
                {
                    await _localFolder.CreateFolderAsync(LOCAL_STUDY_SESSIONS_FOLDER, CreationCollisionOption.ReplaceExisting);
                }

                var setFolder = await _localFolder.GetFolderAsync(LOCAL_STUDY_SESSIONS_FOLDER);
                var setFileInit = await setFolder.TryGetItemAsync(LOCAL_STUDY_SESSIONS_FILE);

                // If there isn't a stored file then we stop here.
                if (setFileInit == null)
                    return;

                var setFile = await setFolder.GetFileAsync(LOCAL_STUDY_SESSIONS_FILE);

                using (var stream = await setFile.OpenStreamForReadAsync())
                {
                    var setSerialized = serializer.ReadObject(stream) as IList<StudySessionItem>;
                    _inMemSessions.AddRange(setSerialized);
                }
            }
        }
    }
}
