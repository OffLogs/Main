using OffLogs.Business.Common.Constants;
using OffLogs.Web.Core.Exceptions;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.UserEmail;
using GetListRequest = OffLogs.Api.Common.Dto.RequestsAndResponses.Board.UserEmail.GetListRequest;

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
    }
}
