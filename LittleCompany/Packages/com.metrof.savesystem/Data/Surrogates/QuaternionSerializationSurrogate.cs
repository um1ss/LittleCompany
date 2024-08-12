using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public class QuaternionSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var Quat = (Quaternion)obj;
            info.AddValue("X", Quat.x);
            info.AddValue("Y", Quat.y);
            info.AddValue("Z", Quat.z);
            info.AddValue("W", Quat.w);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var Quat = (Quaternion)obj;
            Quat.x = (float)info.GetValue("X", typeof(float));
            Quat.y = (float)info.GetValue("Y", typeof(float));
            Quat.z = (float)info.GetValue("Z", typeof(float));
            Quat.w = (float)info.GetValue("W", typeof(float));
            return Quat;
        }
    }
}
