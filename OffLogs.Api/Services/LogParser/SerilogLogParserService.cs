using System;
using System.Threading.Tasks;
using OffLogs.Api.Models.Request.Log.Serilog;
using OffLogs.Business.Db.Dao;

namespace OffLogs.Api.Services.LogParser
{
    public class SerilogLogParserService: ISerilogLogParserService
    {
        private readonly ICommonDao _commonDao;
        
        public SerilogLogParserService(ICommonDao commonDao)
        {
            _commonDao = commonDao;
        }

        public async Task SaveAsync(SerilogEventsRequestModel model)
        {
            throw new NotImplementedException();
        }
    }
}