using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Extensions;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Business.Services.Entities.Log;

public class LogAssembler : ILogAssembler
{
    public Task<LogEntity> AssembleLog(
        ApplicationEntity application,
        string message,
        LogLevel level,
        DateTime timestamp,
        IDictionary<string, object> properties = null,
        ICollection<string> traces = null
    )
    {
        var applicationEncryptor = AsymmetricEncryptor.FromPublicKeyBytes(application.PublicKey);
        var logSymmetricEncryptor = SymmetricEncryptor.GenerateKey();
        var encryptedSymmetricKey = applicationEncryptor.EncryptData(
            logSymmetricEncryptor.Key.GetKey()
        );

        var log = new LogEntity()
        {
            Application = application,
            EncryptedSymmetricKey = encryptedSymmetricKey,
            EncryptedMessage = logSymmetricEncryptor.EncryptData(message),
            Level = level,
            LogTime = timestamp,
            CreateTime = DateTime.UtcNow
        };
        if (properties != null)
        {
            foreach (var property in properties)
            {
                var encryptedKey = logSymmetricEncryptor.EncryptData(property.Key);
                var encryptedValue = logSymmetricEncryptor.EncryptData(
                    property.Value?.GetAsJson()
                );
                log.AddProperty(
                    new LogPropertyEntity(encryptedKey, encryptedValue)
                );
            }
        }

        if (traces != null)
        {
            foreach (var trace in traces)
            {
                var encryptedTrace = logSymmetricEncryptor.EncryptData(trace);
                log.AddTrace(new LogTraceEntity(encryptedTrace));
            }
        }

        return Task.FromResult(log);
    }
}
