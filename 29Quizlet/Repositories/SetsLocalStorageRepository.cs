using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _29Quizlet.Models;
using Windows.Storage;
using System.Runtime.Serialization.Json;
using System.IO;
using _29Quizlet.Models.QuizletTypes.User;

namespace _29Quizlet.Repositories
{
    public class SetsLocalStorageRepository : ISetsLocalStorage
    {
        private const string LOCAL_SET_FOLDER = "SETS";
        private const string LOCAL_SET_FILE = "SETS.json";
        private StorageFolder _localFolder = ApplicationData.Current.LocalFolder;
        private bool _hasRead;
        private List<Set> _inMemSets;
        
        public SetsLocalStorageRepository()
        {
            _inMemSets = new List<Set>();
            _hasRead = false;
        }

        public async Task<IEnumerable<Set>> GetAllSets()
        {
            await ReadFromStorage();

            return _inMemSets;
        }

        public async Task<Set> GetSetById(long id)
        {
            await ReadFromStorage();

            var set = _inMemSets
                .Where(x => x.Id == id)
                .SingleOrDefault();

            return set;
        }

        //public async Task<bool> Save()
        //{
        //    if (_setsInMem.Count > 0)
        //    {
        //        var serializer = new DataContractJsonSerializer(typeof(IList<Set>));
        //        var folderInit = await _localFolder.TryGetItemAsync(LOCAL_SET_FOLDER);

        //        if (folderInit == null)
        //        {
        //            await _localFolder.CreateFolderAsync(LOCAL_SET_FOLDER, CreationCollisionOption.ReplaceExisting);
        //        }

        //        var setFolder = await _localFolder.GetFolderAsync(LOCAL_SET_FOLDER);
        //        using (var stream = await setFolder.OpenStreamForWriteAsync(LOCAL_SET_FILE, CreationCollisionOption.ReplaceExisting))
        //        {
        //            try
        //            {
        //                serializer.WriteObject(stream, _setsInMem);
        //            }
        //            catch (Exception)
        //            {
        //                return false;
        //            }
        //        }

        //    }

        //    return true;
        //}

        //public async Task Update(Set set)
        //{
        //    await ReadFromStorage();

        //    if (_setsInMem.Any(x => x.Id == set.Id))
        //    {
        //        var otherSet = _setsInMem
        //            .Where(x => x.Id == set.Id)
        //            .SingleOrDefault();

        //        _setsInMem.Remove(otherSet);
        //        _setsInMem.Add(set);
        //    }
        //}

        //public async Task Add(Set set)
        //{
        //    await ReadFromStorage();

        //    if (_setsInMem.Any(x => x.Id == set.Id))
        //    {
        //        return;
        //    }

        //    _setsInMem.Add(set);
        //}

        //public async Task Delete(Set set)
        //{
        //    await ReadFromStorage();
        //    if (_setsInMem.Any(x => x.Id == set.Id))
        //    {
        //        var setForRemoval = _setsInMem
        //            .Where(x => x.Id == set.Id)
        //            .SingleOrDefault();

        //        _setsInMem.Remove(setForRemoval);
        //        await Save();
        //    }
        //}

        //private async Task ReadFromStorage()
        //{
        //    // If already loaded we skip reading from storage.
        //    if (_setsInMem.Count < 1)
        //    {
        //        var serializer = new DataContractJsonSerializer(typeof(IList<Set>));
        //        var folderInit = await _localFolder.TryGetItemAsync(LOCAL_SET_FOLDER);

        //        if (folderInit == null)
        //        {
        //            await _localFolder.CreateFolderAsync(LOCAL_SET_FOLDER, CreationCollisionOption.ReplaceExisting);
        //        }

        //        var setFolder = await _localFolder.GetFolderAsync(LOCAL_SET_FOLDER);
        //        var setFileInit = await setFolder.TryGetItemAsync(LOCAL_SET_FILE);

        //        // If there isn't a stored file then we stop here.
        //        if (setFileInit == null)
        //            return;

        //        var setFile = await setFolder.GetFileAsync(LOCAL_SET_FILE);

        //        using (var stream = await setFile.OpenStreamForReadAsync())
        //        {
        //            var setSerialized = serializer.ReadObject(stream) as IList<Set>;
        //            _setsInMem.AddRange(setSerialized);
        //        }

        //    }

        //}

        private async Task ReadFromStorage()
        {
            // If already loaded we skip reading from storage.
            if (_inMemSets.Count() < 1)
            {
                var serializer = new DataContractJsonSerializer(typeof(IList<Set>));
                var folderInit = await _localFolder.TryGetItemAsync(LOCAL_SET_FOLDER);

                if (folderInit == null)
                {
                    await _localFolder.CreateFolderAsync(LOCAL_SET_FOLDER, CreationCollisionOption.ReplaceExisting);
                }

                var setFolder = await _localFolder.GetFolderAsync(LOCAL_SET_FOLDER);
                var setFileInit = await setFolder.TryGetItemAsync(LOCAL_SET_FILE);

                // If there isn't a stored file then we stop here.
                if (setFileInit == null)
                    return;

                var setFile = await setFolder.GetFileAsync(LOCAL_SET_FILE);

                using (var stream = await setFile.OpenStreamForReadAsync())
                {
                    var setSerialized = serializer.ReadObject(stream) as IList<Set>;
                    _inMemSets.AddRange(setSerialized);
                }

            }
            await Task.CompletedTask;
        }

        public async Task DeleteSet(Set set)
        {
            await ReadFromStorage();

            if (_inMemSets.Any(x => x.Id == set.Id))
            {
                var setForRemoval = _inMemSets
                    .Where(x => x.Id == set.Id)
                    .SingleOrDefault();

                _inMemSets.Remove(setForRemoval);
                await SaveSets();
            }
        }

        public async Task AddSet(Set set)
        {
            await ReadFromStorage();

            if (_inMemSets.Any(x => x.Id == set.Id))
            {
                return;
            }

            _inMemSets.Add(set);
        }

        public async Task UpdateSet(Set set)
        {
            await ReadFromStorage();

            if (_inMemSets.Any(x => x.Id == set.Id))
            {
                var otherSet = _inMemSets
                    .Where(x => x.Id == set.Id)
                    .SingleOrDefault();

                _inMemSets.Remove(otherSet);
                _inMemSets.Add(set);
            }
        }

        public async Task<bool> SaveSets()
        {
            
            var serializer = new DataContractJsonSerializer(typeof(IList<Set>));
            //await _localFolder.DeleteAsync(StorageDeleteOption.Default);
            var folderInit = await _localFolder.TryGetItemAsync(LOCAL_SET_FOLDER);

            if (folderInit == null)
            {
                await _localFolder.CreateFolderAsync(LOCAL_SET_FOLDER, CreationCollisionOption.ReplaceExisting);
            }

            var setFolder = await _localFolder.GetFolderAsync(LOCAL_SET_FOLDER);
            var files = await setFolder.GetFilesAsync();

            foreach (var file in files)
            {
                await file.DeleteAsync();
            }

            using (var stream = await setFolder.OpenStreamForWriteAsync(LOCAL_SET_FILE, CreationCollisionOption.ReplaceExisting))
            {
                try
                {
                    serializer.WriteObject(stream, _inMemSets);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            //}

            return true;
        }

        

        public async Task PurgeAndAllNewSets(IEnumerable<Set> sets)
        {
            _inMemSets.Clear();
            _inMemSets.AddRange(sets);
            await SaveSets();
        }

        public async Task DeleteAllSets()
        {
            await ReadFromStorage();
            _inMemSets.Clear();
            await SaveSets();
        }
    }
}
