using System;

namespace Pospec.Saving
{
    [Serializable]
    public class SaveData
    {
        public int version;

        public SaveData(int version)
        {
            this.version = version;
        }
    }
}
