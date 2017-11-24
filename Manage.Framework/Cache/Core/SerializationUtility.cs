using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Manage.Framework.Cache.Core
{
    /// <summary>
    /// 序列化辅助类
    /// </summary>
    public static class SerializationUtility
    {
        /// <summary>
        /// 将对象转换成二进制数据
        /// </summary>
        /// <param name="value">要转换的对象实例</param>
        /// <returns>序列化后的二进制数据</returns>
        public static byte[] ToBytes(object value)
        {
            if (value == null)
            {
                return null;
            }

            byte[] inMemoryBytes;
            using (MemoryStream inMemoryData = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Binder = new Binder();
                binaryFormatter.Serialize(inMemoryData, value);
                inMemoryBytes = inMemoryData.ToArray();
            }

            return inMemoryBytes;
        }

        /// <summary>
        /// 将二进制数据转换成对象实例
        /// </summary>
        /// <param name="serializedObject">要转换的二进制数据</param>
        /// <returns>序列化后的对象实例</returns>
        public static object ToObject(byte[] serializedObject)
        {
            if (serializedObject == null)
            {
                return null;
            }

            using (MemoryStream dataInMemory = new MemoryStream(serializedObject))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Binder = new Binder();
                return binaryFormatter.Deserialize(dataInMemory);
            }
        }
    }
}