namespace OffLogs.Business.Common.Constants
{
    public class MainApiUrl
    {
        public const string Login = "/user/login";
        public const string UserCheckIsLoggedIn = "/user/checkIsLoggedIn";
        
        public const string ApplicationList = "/board/application/list";
        public const string ApplicationAdd = "/board/application/add";
        public const string ApplicationGetOne = "/board/application/get";
        public const string ApplicationUpdate = "/board/application/update";
        
        public const string LogList = "/board/log/list";
        public const string LogGet = "/board/log/get";
        public const string LogSetIsFavorite = "/board/log/setFavorite";
        public const string LogGetStatisticForNow = "/board/log/getStatisticForNow";

        public const string UserSearch = "/board/user/search";

        public const string PermissionAddAccess = "/board/permission/addAccess";
        public const string PermissionRemoveAccess = "/board/permission/removeAccess";

    }
}