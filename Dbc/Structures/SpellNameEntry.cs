using DBFileReaderLib.Attributes;

namespace CreatureScriptsParser.Dbc.Structures
{
    [DBFile("SpellName")]
    public sealed class SpellNameEntry
    {
        [Index(true)]
        public uint ID;
        public string Name;
    }
}
