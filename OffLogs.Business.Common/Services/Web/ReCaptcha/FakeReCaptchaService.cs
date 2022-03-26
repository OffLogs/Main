using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OffLogs.Business.Common.Services.Web.ReCaptcha.Models;

namespace OffLogs.Business.Common.Services.Web.ReCaptcha;

public class FakeReCaptchaService: IReCaptchaService
{

    public Task<bool> ValidateAsync(string token) => Task.FromResult(true);
}
