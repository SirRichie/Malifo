using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DataPersistor
{
    public class Persistor<T>
    {
        public T LoadData(string path) {
            T objectToLoad;
            Stream stream = File.Open(path, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            objectToLoad = (T)bFormatter.Deserialize(stream);
            stream.Close();
            return objectToLoad;           
        }

        public void Save(T objectToSave, string path)
        {
            Stream stream = File.Open(path, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, objectToSave);
            stream.Close();
        }        
    }
}
