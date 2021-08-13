using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Common.Dto
{
    public class PermissionInfoDto
    {
        public PermissionInfoDto(bool isReadAccess, bool isWriteAccess)
        {
            IsReadAccess = isReadAccess;
            IsWriteAccess = isWriteAccess;
        }

        public bool IsReadAccess { get; set; }
        public bool IsWriteAccess { get; set; }
    }
}
