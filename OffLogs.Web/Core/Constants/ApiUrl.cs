namespace OffLogs.Web.Core.Constants
{
    public class ApiUrl
    {
        public static readonly string Login = "user/login";
        public static readonly string UserCheckIsLoggedIn = "user/checkIsLoggedIn";
        
        public static readonly string ApplicationList = "board/application/list";
        public static readonly string ApplicationAdd = "board/application/add";
        public static readonly string ApplicationUpdate = "board/application/update";
        
        public static readonly string LogList = "board/log/list";
        public static readonly string LogGet = "board/log/get";
        public static readonly string LogSetIsFavorite = "board/log/setFavorite";
    }
}