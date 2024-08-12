using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public class Vector3SerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var V3 = (Vector3)obj;
            info.AddValue("X", V3.x);
            info.AddValue("Y", V3.y);
            info.AddValue("Z", V3.z);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var V3 = (Vector3)obj;
            V3.x = (float)info.GetValue("X", typeof(float));
            V3.y = (float)info.GetValue("Y", typeof(float));
            V3.z = (float)info.GetValue("Z", typeof(float));
            return V3;
        }
    }
}
