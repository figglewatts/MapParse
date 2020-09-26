using System.Collections.Generic;

namespace MapParse.Types
{
    public class MapFile
    {
        public List<Entity> Entities { get; set; }

        public MapFile() { Entities = new List<Entity>(); }
    }
}
