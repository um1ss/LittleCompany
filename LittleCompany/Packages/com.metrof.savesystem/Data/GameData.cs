using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public class GameData
    {
        private readonly Dictionary<int, EntitySaveData> _entities = new();

        public EntitySaveData GetEntityData(int id)
        {
            if (_entities.ContainsKey(id))
            {
                return _entities[id];
            }
            return new EntitySaveData(Vector3.zero, Quaternion.identity);
        }
        public void SaveEntities(List<ISavableObject> entities)
        {
            _entities.Clear();
            foreach (var obj in entities)
            {
                Vector3 objPos = obj.GetPosition();

                Quaternion objQuaternion = obj.GetRotation();

                _entities.Add(obj.ID, new(objPos, objQuaternion));
            }
        }
    }
}
