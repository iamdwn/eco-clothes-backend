using System.ComponentModel;

namespace DataAccess.Interfaces
{
    public interface ISoftDelete
    {
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedWhen { get; set; }
    }
}
