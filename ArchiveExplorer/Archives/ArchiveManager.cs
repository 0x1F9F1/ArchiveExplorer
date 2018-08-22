using System.Collections.Generic;

namespace Archive
{
    public class ArchiveManager
    {
        protected Dictionary<string, IArchiveType> types_ = new Dictionary<string, IArchiveType>();

        public ICollection<IArchiveType> Types => types_.Values;

        public void AddType(IArchiveType type)
        {
            types_.Add(type.Identifier, type);
        }

        public IArchiveType GetType(string key)
        {
            if (types_.TryGetValue(key, out IArchiveType type))
            {
                return type;
            }

            return null;
        }

        public IArchive Open(IFile file)
        {
            using (var stream = file.OpenRead())
            {
                foreach (var type in types_)
                {
                    if (type.Value.IsValid(file, stream))
                    {
                        return type.Value.GetArchive(file, stream);
                    }
                }
            }

            return null;
        }

        public IArchive Open(IFile file, string key)
        {
            var type = GetType(key);

            if (type != null)
            {
                using (var stream = file.OpenRead())
                {
                    return type.GetArchive(file, stream);
                }
            }

            return null;
        }
    }
}