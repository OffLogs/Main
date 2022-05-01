using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using OffLogs.Business.Common.Resources;

namespace OffLogs.Business.Common.Constants.Notificatiions;

public enum LogicOperatorType
{
    [Display(Name = "LogicOperatorType_Disjunction", ResourceType = typeof(EnumResources))]
    Disjunction = 1,
    
    [Display(Name = "LogicOperatorType_Conjunction", ResourceType = typeof(EnumResources))]
    Conjunction = 2
}
