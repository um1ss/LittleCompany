using System;

namespace SaveSystem
{
    public interface IStorageService
    {
        void SaveGameData(string key, GameData data, Action callback = null);
        void LoadGameData(string key, GameData defaultsData, Action<GameData> callback);
    }
}
