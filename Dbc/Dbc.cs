using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using DBFileReaderLib;
using CreatureScriptsParser.Dbc.Structures;
using CreatureScriptsParser.Properties;

namespace CreatureScriptsParser.Dbc
{
    public static class Dbc
    {
        private static bool loaded;
        public static Storage<SpellNameEntry> SpellName { get; set; }

        private static string GetDBCPath()
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "dbc", "enUS");
        }

        private static string GetDBCPath(string fileName)
        {
            return Path.Combine(GetDBCPath(), fileName);
        }

        public static bool IsLoaded()
        {
            return loaded;
        }

        public static void Load()
        {
            Parallel.ForEach(typeof(Dbc).GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic), dbc =>
            {
                Type type = dbc.PropertyType.GetGenericArguments()[0];

                if (!type.IsClass)
                    return;

                var startTime = DateTime.Now;
                var attr = type.GetCustomAttribute<DBFileAttribute>();
                if (attr == null)
                    return;

                string pathName = GetDBCPath(attr.FileName) + ".db2";
                var instanceType = typeof(Storage<>).MakeGenericType(type);
                var countGetter = instanceType.GetProperty("Count").GetGetMethod();
                dynamic instance = Activator.CreateInstance(instanceType, pathName);
                var recordCount = (int)countGetter.Invoke(instance, new object[] { });

                try
                {
                    var db2Reader = new DBReader($"{ GetDBCPath(attr.FileName) }.db2");

                    dbc.SetValue(dbc.GetValue(null), instance);
                }
                catch (TargetInvocationException tie)
                {
                    if (tie.InnerException is ArgumentException)
                        throw new ArgumentException($"Failed to load {attr.FileName}.db2: {tie.InnerException.Message}");
                    throw;
                }
            });

            loaded = true;
        }
    }
}
