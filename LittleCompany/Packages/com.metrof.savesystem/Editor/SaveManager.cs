using System.IO;
using UnityEngine;
using System;

namespace SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        public Action OnDataSaved;
        public Action<GameData> OnDataLoaded;

        private readonly string _filePath;
        private readonly IStorageService _storageService;

        public SaveManager(IStorageService storageService)
        {
            _storageService = storageService;

            var directory = Application.persistentDataPath + "/saves";
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            _filePath = directory + "/GameSave.save";
        }
        public void SaveGame(GameData gameData)
        {
            _storageService.SaveGameData(_filePath, gameData, OnDataSaved);
        }
        public void LoadGame(GameData gameData)
        {
            _storageService.LoadGameData(_filePath, gameData, OnDataLoaded);
        }
    }
}
