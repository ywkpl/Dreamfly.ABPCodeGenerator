using System;
using System.Collections.Generic;

#nullable disable

namespace Dreamfly.JavaEstateCodeGenerator.MariadbModels
{
    public partial class SysCode
    {
        public SysCode()
        {
            InversePidNavigation = new HashSet<SysCode>();
        }

        public long Id { get; set; }
        public DateTime? CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public ulong? IsDeleted { get; set; }
        public string Code { get; set; }
        public long? CompanyId { get; set; }
        public ulong? IsDefault { get; set; }
        public string Name { get; set; }
        public int? Ord { get; set; }
        public long? TenantId { get; set; }
        public long? Pid { get; set; }
        public int? Rank { get; set; }

        public virtual SysCode PidNavigation { get; set; }
        public virtual ICollection<SysCode> InversePidNavigation { get; set; }
    }
}
