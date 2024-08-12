using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;

namespace SaveSystem
{
    public class BinaryToFileStorageService : IStorageService
    {
        private BinaryFormatter _binaryFormatter;

        private bool _isSave;
        public BinaryToFileStorageService()
        {
            InitBinaryFormatter();
        }
        private void InitBinaryFormatter()
        {
            _binaryFormatter = new BinaryFormatter();
            var selector = new SurrogateSelector();

            selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), new Vector3SerializationSurrogate());
            selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), new QuaternionSerializationSurrogate());

            _binaryFormatter.SurrogateSelector = selector;
        }

        public void LoadGameData(string key, GameData defaultsData, Action<GameData> callback)
        {
            if (!File.Exists(key))
            {
                if (defaultsData != null)
                {
                    SaveGameData(key, defaultsData);
                    callback?.Invoke(defaultsData);
                    return;
                }
            }
            using (FileStream fs = new(key, FileMode.OpenOrCreate))
            {
                var data = (GameData)_binaryFormatter.Deserialize(fs);
                callback?.Invoke(data);
            }
        }

        public void SaveGameData(string key, GameData data, Action callback = null)
        {
            if (_isSave) return;
            using (FileStream fs = new(key, FileMode.OpenOrCreate))
            {
                _isSave = true;
                _binaryFormatter.Serialize(fs, data);
            }
            _isSave = false;
            callback?.Invoke();
        }
    }
}
