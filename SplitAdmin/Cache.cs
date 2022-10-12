using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace SplitAdmin
{
    public class Cache<TKey, TValue> : Dictionary<TKey, TValue> where TKey : notnull
    {
        private static readonly string AppDataPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
        private static readonly string SplitPath = Path.Combine(AppDataPath, "Split.io");

        private readonly string _name;

        private Cache(string name) : base()
        {
            _name = name;
        }

        public static Cache<UKey, UValue> LoadFromCache<UKey, UValue>(string name) where UKey : notnull
        {
            var cache = new Cache<UKey, UValue>(name);

            if (!Directory.Exists(SplitPath))
            {
                Directory.CreateDirectory(SplitPath);
            }

            var cachePath = Path.Combine(SplitPath, $"{name}.json");

            if (!File.Exists(cachePath))
            {
                return cache;
            }

            var userCacheContents = File.ReadAllText(cachePath);

            var data = JsonConvert.DeserializeObject<Dictionary<UKey, UValue>>(userCacheContents);

            // If it was invalid, just delete and continue
            if (data == null)
            {
                File.Delete(cachePath);
                return cache;
            }


            foreach (var (key, value) in data)
            {
                cache.Add(key, value);
            }

            return cache;
        }

        public void Save()
        {
            if (!Directory.Exists(SplitPath))
            {
                Directory.CreateDirectory(SplitPath);
            }

            var cachePath = Path.Combine(SplitPath, $"{_name}.json");

            var result = JsonConvert.SerializeObject(this);

            File.WriteAllText(cachePath, result);
        }

    }
}