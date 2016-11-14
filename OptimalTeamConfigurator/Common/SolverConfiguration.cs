using System;
using System.IO;
using YAXLib;

namespace Common
{
    public class SolverConfiguration<T>
    {
        private T configuration;

        public string Path
        {
            get; set;
        }

        public T Configuration
        {
            get
            {
                if (configuration == null)
                {
                    configuration = getConfigurationFromPath(Path);
                }

                return configuration;
            }
        }

        private T getConfigurationFromPath(string path)
        {
            var deserializer = new YAXSerializer(typeof(T), YAXExceptionHandlingPolicies.ThrowErrorsOnly, YAXExceptionTypes.Warning);
            object deserializedObject = null;

            deserializedObject = deserializer.Deserialize(File.ReadAllText(path));

            if (deserializer.ParsingErrors.ContainsAnyError)
            {
                Console.Error.WriteLine("Succeeded to deserialize, but these problems also happened:");
                Console.Error.WriteLine(deserializer.ParsingErrors.ToString());
            }

            return ((T)deserializedObject);
        }
    }
}
