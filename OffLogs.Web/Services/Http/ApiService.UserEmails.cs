using OffLogs.Business.Common.Constants;
using OffLogs.Web.Core.Exceptions;
using System.Threading.Tasks;
using System.Web;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.UserEmail;

namespace OffLogs.Web.Services.Http
{
    public partial class ApiService
    {
        public async Task<UserEmailsListDto> UserEmailsGetList()
        {
            var response = await PostAuthorizedAsync<UserEmailsListDto>(
                MainApiUrl.UserEmailsList, 
                new GetListRequest()
            );
            if (response == null)
            {
                throw new ServerErrorException();
            }
            return response;
        }
        
        public async Task<UserEmailDto> UserEmailAddAsync(string email)
        {
            var response = await PostAuthorizedAsync<UserEmailDto>(
                MainApiUrl.UserEmailsAdd,
                new AddRequest()
                {
                    Email = email
                }
            );
            if (response == null)
            {
                throw new ServerErrorException();
            }

            return response;
        }

        public async Task<ApplicationDto> UserEmailDeleteAsync(long id)
        {
            var response = await PostAuthorizedAsync<ApplicationDto>(
                MainApiUrl.UserEmailsDelete,
                new DeleteRequest()
                {
                    Id = id
                }
            );
            return response;
        }

        public async Task UserEmailVerifyAsync(string token)
        {
            await GetAsync<object>(
                MainApiUrl.UserEmailsVerify + HttpUtility.UrlPathEncode(token)
            );
        }
    }
}
