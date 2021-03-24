using System;

namespace CreatureScriptsParser.Dbc.Structures
{
    public sealed class DBFileAttribute : Attribute
    {
        public string FileName { get; }

        public DBFileAttribute(string fileName)
        {
            FileName = fileName;
        }
    }
}
