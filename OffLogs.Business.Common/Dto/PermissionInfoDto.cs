using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Common.Dto
{
    public class PermissionInfoDto
    {
        public PermissionInfoDto(bool isHasReadAccess, bool isHasWriteAccess)
        {
            IsHasReadAccess = isHasReadAccess;
            IsHasWriteAccess = isHasWriteAccess;
        }

        public bool IsHasReadAccess { get; set; }
        public bool IsHasWriteAccess { get; set; }
    }
}
