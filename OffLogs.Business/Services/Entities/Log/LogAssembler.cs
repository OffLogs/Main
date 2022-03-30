using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Extensions;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Business.Services.Entities.Log;

public class LogAssembler : ILogAssembler
{
    public Task<LogEntity> AssembleEncryptedLogAsync(
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

        var log = new LogEntity
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
    
    public Task<LogEntity> AssembleDecryptedLogAsync(LogEntity log, byte[] privateKey)
    {
        var userEncryptor = AsymmetricEncryptor.FromPrivateKeyBytes(privateKey);
        var appPrivateKey = userEncryptor.DecryptData(log.Application.EncryptedPrivateKey);
        var appEncryptor = AsymmetricEncryptor.FromPrivateKeyBytes(appPrivateKey);

        var symmetricKey = appEncryptor.DecryptData(log.EncryptedSymmetricKey);
        var logSymmetricEncryptor = new SymmetricEncryptor(symmetricKey);
        
        log.Message = logSymmetricEncryptor.DecryptData(log.EncryptedMessage).GetString();
        if (log.Properties != null)
        {
            foreach (var property in log.Properties)
            {
                var decryptedKey = logSymmetricEncryptor.DecryptData(property.EncryptedKey);
                var decryptedValue = logSymmetricEncryptor.DecryptData(
                    property.EncryptedValue
                );
                property.Key = decryptedKey.GetString();
                property.Value = decryptedValue.GetString();
            }
        }

        if (log.Traces != null)
        {
            foreach (var trace in log.Traces)
            {
                var decryptedTrace = logSymmetricEncryptor.DecryptData(trace.EncryptedTrace);
                trace.Trace = decryptedTrace.GetString();
            }
        }

        return Task.FromResult(log);
    }
}
