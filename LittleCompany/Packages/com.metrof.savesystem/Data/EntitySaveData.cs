using System;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public class EntitySaveData
    {
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }

        public EntitySaveData(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}
