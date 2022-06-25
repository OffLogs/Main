namespace OffLogs.Business.Common.Constants
{
    public class MainApiUrl
    {
        #region Public
        public const string Login = "user/login";
        public const string UserCheckIsLoggedIn = "user/checkIsLoggedIn";
        public const string LogGetSharedByToken = "log/getShared";
        public const string RegistrationStep1 = "user/registration/step1";
        public const string RegistrationStep2 = "user/registration/step2";
        #endregion

        #region Board
        public const string ApplicationList = "board/application/list";
        public const string ApplicationAdd = "board/application/add";
        public const string ApplicationGetOne = "board/application/get";
        public const string ApplicationGetSharedUser = "board/application/get-shared-users";
        public const string ApplicationUpdate = "board/application/update";
        public const string ApplicationDelete = "board/application/delete";

        public const string LogList = "board/log/list";
        public const string LogShare = "board/log/share";
        public const string LogGet = "board/log/get";
        public const string LogSetIsFavorite = "board/log/setFavorite";
        public const string LogGetStatisticForNow = "board/log/getStatisticForNow";
        
        public const string UserSearch = "board/user/search";
        
        public const string UserEmailsList = "board/user/email/list";
        public const string UserEmailsAdd = "board/user/email/add";
        public const string UserEmailsDelete = "board/user/email/delete";

        public const string PermissionAddAccess = "board/permission/addAccess";
        public const string PermissionRemoveAccess = "board/permission/removeAccess";
        
        public const string NotificationMessageTemplateSet = "board/notifications/message-templates/set";
        public const string NotificationMessageTemplateDelete = "board/notifications/message-templates/delete";
        public const string NotificationMessageTemplateList = "board/notifications/message-templates/list";
        public const string NotificationRulesSet = "board/notifications/rules/set";
        public const string NotificationRulesList = "board/notifications/rules/list";
        public const string NotificationRuleDelete = "board/notifications/rules/delete";
        
        public const string StatisticApplication = "board/statistic/application";
        #endregion
    }
}
