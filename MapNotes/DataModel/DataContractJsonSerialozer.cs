using System;

namespace MapNotes.DataModel
{
    internal class DataContractJsonSerialozer
    {
        private Type type;

        public DataContractJsonSerialozer(Type type)
        {
            this.type = type;
        }
    }
}