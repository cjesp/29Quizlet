using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _29Quizlet.Models.QuizletTypes.User;
using Windows.Storage;
using System.Runtime.Serialization.Json;
using System.IO;

namespace _29Quizlet.Repositories
{
    public class ClassesLocalStorageRepository : ILocalClassesStorage
    {
        private const string LOCAL_CLASSES_FOLDER = "CLASSES";
        private const string LOCAL_CLASSES_FILE = "CLASSES.json";
        private StorageFolder _localFolder = ApplicationData.Current.LocalFolder;
        private List<Classes> _inMemClasses;

        public ClassesLocalStorageRepository()
        {
            _inMemClasses = new List<Classes>();
        }

        public async Task<IEnumerable<Classes>> GetAllClasses()
        {
            await ReadFromStorage();
            return _inMemClasses;
        }

        public async Task<Classes> GetClassById(long id)
        {
            if (_inMemClasses.Count() == 0)
            {
                await ReadFromStorage();
            }

            var clazz = _inMemClasses.Where(x => x.Id == id)
                .SingleOrDefault();

            return clazz;
        }

        public async Task DeleteClasses(Classes clazz)
        {
            await ReadFromStorage();
            if (_inMemClasses.Any(x => x.Id == clazz.Id))
            {
                var classesForRemoval = _inMemClasses
                    .Where(x => x.Id == clazz.Id)
                    .SingleOrDefault();

                _inMemClasses.Remove(classesForRemoval);
                await SaveClasses();
            }
        }

        public async Task AddClasses(Classes clazz)
        {
            await ReadFromStorage();

            if (_inMemClasses.Any(x => x.Id == clazz.Id))
            {
                return;
            }

            _inMemClasses.Add(clazz);
        }

        public async Task UpdateClasses(Classes clazz)
        {
            await ReadFromStorage();

            if (_inMemClasses.Any(x => x.Id == clazz.Id))
            {
                var otherClazz = _inMemClasses
                    .Where(x => x.Id == clazz.Id)
                    .SingleOrDefault();

                _inMemClasses.Remove(otherClazz);
                _inMemClasses.Add(clazz);
            }
        }

        public async Task<bool> SaveClasses()
        {
            if (_inMemClasses.Count > 0)
            {
                var serializer = new DataContractJsonSerializer(typeof(IList<Classes>));
                var folderInit = await _localFolder.TryGetItemAsync(LOCAL_CLASSES_FOLDER);

                if (folderInit == null)
                {
                    await _localFolder.CreateFolderAsync(LOCAL_CLASSES_FOLDER, CreationCollisionOption.ReplaceExisting);
                }

                var classesFolder = await _localFolder.GetFolderAsync(LOCAL_CLASSES_FOLDER);
                using (var stream = await classesFolder.OpenStreamForWriteAsync(LOCAL_CLASSES_FILE, CreationCollisionOption.ReplaceExisting))
                {
                    try
                    {
                        serializer.WriteObject(stream, _inMemClasses);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

            }

            return true;
        }

        private async Task ReadFromStorage()
        {
            if (_inMemClasses.Count < 1)
            {
                var serializer = new DataContractJsonSerializer(typeof(IList<Classes>));
                var folderInit = await _localFolder.TryGetItemAsync(LOCAL_CLASSES_FOLDER);

                if (folderInit == null)
                {
                    await _localFolder.CreateFolderAsync(LOCAL_CLASSES_FOLDER, CreationCollisionOption.ReplaceExisting);
                }

                var setFolder = await _localFolder.GetFolderAsync(LOCAL_CLASSES_FOLDER);
                var setFileInit = await setFolder.TryGetItemAsync(LOCAL_CLASSES_FILE);

                // If there isn't a stored file then we stop here.
                if (setFileInit == null)
                    return;

                var setFile = await setFolder.GetFileAsync(LOCAL_CLASSES_FILE);

                using (var stream = await setFile.OpenStreamForReadAsync())
                {
                    var setSerialized = serializer.ReadObject(stream) as IList<Classes>;
                    _inMemClasses.AddRange(setSerialized);
                }
            }
        }

        public async Task AddClass(Classes clazz)
        {
            if (!_inMemClasses.Any(x => x.Id == clazz.Id))
            {
                _inMemClasses.Add(clazz);
                await SaveClasses();
            }
        }

        public async Task DeleteClass(Classes clazz)
        {
            if (_inMemClasses.Any(x => x.Id == clazz.Id))
            {
                var removeClass = _inMemClasses.Where(x => x.Id == clazz.Id).SingleOrDefault();
                if (removeClass != null)
                {
                    _inMemClasses.Remove(removeClass);
                    await SaveClasses();
                }
            }
        }

        public async Task UpdateClass(Classes clazz)
        {
            if (_inMemClasses.Any(x => x.Id == clazz.Id))
            {
                var removeClass = _inMemClasses.Where(x => x.Id == clazz.Id).SingleOrDefault();
                if (removeClass != null)
                {
                    _inMemClasses.Remove(removeClass);
                    _inMemClasses.Add(clazz);
                    await SaveClasses();
                }
            }
        }

        public async Task DeleteAllClasses()
        {
            _inMemClasses.Clear();
            await SaveClasses();
        }
    }
}
