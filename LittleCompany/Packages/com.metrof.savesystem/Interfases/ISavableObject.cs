using UnityEngine;

namespace SaveSystem
{
    public interface ISavableObject
    {
        public int ID { get; }

        public void SetID(int ID);

        public Vector3 GetPosition();
        public Quaternion GetRotation();
    }
}
