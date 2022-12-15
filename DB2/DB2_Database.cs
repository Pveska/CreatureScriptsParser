using DB2.Structures;
using DB2Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace DB2
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HotfixAttribute : Attribute
    {
        public HotfixAttribute(string pTableName)
        {
            TableName = pTableName;
        }

        public string TableName { get; set; }
    }

    public static class Db2
    {
        public static MySqlStorage<SpellName>                       SpellName { get; set; }

        public static void Load()
        {
            List<String> lFailedDb2 = new List<string>();
            Parallel.ForEach(
                typeof(Db2).GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic), db2 =>
                {
                    if (!db2.PropertyType.IsGenericType ||
                        (db2.PropertyType.GetGenericTypeDefinition() != typeof(MySqlStorage<>) &&
                        db2.PropertyType.GetGenericTypeDefinition() != typeof(MySqlWorldStorage<>)))
                        return;

                    var name = db2.Name;

                    try
                    {
                        db2.SetValue(db2.GetValue(null), Activator.CreateInstance(db2.PropertyType));
                    }
                    catch (DirectoryNotFoundException)
                    {
                        lFailedDb2.Add(name + ".db2");
                    }
                });

            if (lFailedDb2.Count != 0)
            {
                StreamWriter lErrorLog = new StreamWriter(new FileStream("error.log", FileMode.CreateNew));
                foreach (var db2 in lFailedDb2)
                    lErrorLog.WriteLine(db2);
                lErrorLog.Flush();
                lErrorLog.Close();
                return;
            }
        }
    }
}
