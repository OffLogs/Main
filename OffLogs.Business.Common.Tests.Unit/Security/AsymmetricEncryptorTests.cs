using System.Text;
using OffLogs.Business.Common.Encryption;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Xunit;
using AsymmetricEncryptor = OffLogs.Business.Common.Security.AsymmetricEncryptor;

namespace OffLogs.Business.Common.Tests.Unit.Encryption
{
    public class RsaEncryptionServiceTests
    {
        private AsymmetricEncryptor _encryptor;

        public RsaEncryptionServiceTests()
        {
            _encryptor = AsymmetricEncryptor.GenerateKeyPair();
        }

        #region Common
        [Fact]
        public void ShouldCreateKeyPair()
        {
            Assert.NotNull(_encryptor.PrivateKey);
            Assert.NotNull(_encryptor.PublicKey);
            Assert.NotNull(_encryptor.PrivateKey);
        }
        
        [Fact]
        public void ShouldEncryptDataWithPublicKey()
        {
            var data = new byte[] { 1, 2, 3, 4, 5 };
            var encrypted = _encryptor.EncryptData(data);
            Assert.NotNull(encrypted);
            Assert.True(encrypted.Length > 0);
        }
        
        [Fact]
        public void ShouldDecryptData()
        {
            var dataString = "some data";
            var dataBytes = Encoding.UTF8.GetBytes(dataString);
            var encrypted = _encryptor.EncryptData(dataBytes);

            var decrypted = _encryptor.DecryptData(encrypted);
            var decryptedString = Encoding.UTF8.GetString(decrypted);
            Assert.Equal(decryptedString, dataString);
        }
        
        [Fact]
        public void ShouldThrowExceptionIfDecryptionDataLengthIsIncorrect()
        {
            var dataString = "some data";
            var dataBytes = Encoding.UTF8.GetBytes(dataString);
            var encrypted = _encryptor.EncryptData(dataBytes);

            Assert.Throws<InvalidCipherTextException>(() =>
            {
                _encryptor.DecryptData(new byte[]{ 1, 2 });
            });
        }
        
        [Fact]
        public void ShouldThrowExceptionIfDecryptionDataIsIncorrect()
        {
            var dataString = "some data";
            var dataBytes = Encoding.UTF8.GetBytes(dataString);
            var encrypted = _encryptor.EncryptData(dataBytes);

            Assert.Throws<InvalidCipherTextException>(() =>
            {
                var encryptor2 = AsymmetricEncryptor.GenerateKeyPair();
                encryptor2.DecryptData(encrypted);
            });
        }
        
        [Fact]
        public void ShouldNotDecryptData()
        {
            var dataString = "some data";
            var dataBytes = Encoding.UTF8.GetBytes(dataString);
            var encrypted = _encryptor.EncryptData(dataBytes);

            Assert.Throws<InvalidCipherTextException>(() =>
            {
                _encryptor.DecryptData(new byte[]{ 1, 2 });
            });
        }
        
        [Fact]
        public void ShouldSignDataAndValidateThisSign()
        {
            var dataString = "some data";
            var dataBytes = Encoding.UTF8.GetBytes(dataString);
            var sign = _encryptor.SignData(dataBytes);
            var isValidSign = _encryptor.VerifySign(dataBytes, sign);
            Assert.True(isValidSign);
        }
        
        [Fact]
        public void SignShouldNotBeCorrect()
        {
            var dataString = "some data";
            var dataBytes = Encoding.UTF8.GetBytes(dataString);
            var sign = _encryptor.SignData(dataBytes);
            
            var encryptor2 = AsymmetricEncryptor.GenerateKeyPair();
            var isValidSign = encryptor2.VerifySign(dataBytes, sign);
            Assert.False(isValidSign);
        }
        #endregion

        #region PEM

        [Fact]
        public void ShouldGeneratePemFile()
        {
            var pem = _encryptor.CreatePem("some password");
            Assert.NotEmpty(pem);
            Assert.Contains("PUBLIC KEY", pem);
            Assert.Contains("PRIVATE KEY", pem);
        }

        [Fact]
        public void ShouldReadPemFile()
        {
            var actualPassword = "some password";
            
            var actualPem = _encryptor.CreatePem(actualPassword);
            var actualEncryptor = AsymmetricEncryptor.ReadFromPem(actualPem, actualPassword);
            Assert.NotNull(actualEncryptor.PrivateKey);
            Assert.NotNull(actualEncryptor.PublicKey);
            
            Assert.Equal(_encryptor.PrivateKey, actualEncryptor.PrivateKey);
            Assert.Equal(_encryptor.PublicKey, actualEncryptor.PublicKey);
        }
        
        [Fact]
        public void ShouldNotReadPemFileIfPasswordIncorrect()
        {
            var actualPem = _encryptor.CreatePem("some password");
            Assert.Throws<PemException>(() =>
            {
                AsymmetricEncryptor.ReadFromPem(actualPem, "wrong password");
            });
        }
        
        #endregion

        #region Binary Conversion

        [Fact]
        public void ShouldReceivePrivateKeyBytes()
        {
            var bytes = _encryptor.GetPrivateKeyBytes();
            Assert.NotNull(bytes);
            Assert.NotEmpty(bytes);
        }
        
        [Fact]
        public void ShouldReceivePublicKeyBytes()
        {
            var bytes = _encryptor.GetPublicKeyBytes();
            Assert.NotNull(bytes);
            Assert.NotEmpty(bytes);
        }

        [Fact]
        public void ShouldExportAndImportPrivateKey()
        {
            var privateKey = _encryptor.GetPrivateKeyBytes();

            var newEncryptor = AsymmetricEncryptor.FromPrivateKeyBytes(privateKey);
            
            Assert.Equal(newEncryptor.PrivateKey, _encryptor.PrivateKey);
            Assert.Equal(newEncryptor.PublicKey, _encryptor.PublicKey);
        }
        
        [Fact]
        public void ShouldExportPrivateKeyImportPrivateKeyAndValidateSign()
        {
            var dataString = "some data";
            var dataBytes = Encoding.UTF8.GetBytes(dataString);
            
            var privateKey = _encryptor.GetPrivateKeyBytes();

            var signEncryptor = AsymmetricEncryptor.FromPrivateKeyBytes(privateKey);
            var signChecker = AsymmetricEncryptor.FromPrivateKeyBytes(privateKey);
            
            var sign = signEncryptor.SignData(dataBytes);
            
            Assert.True(signChecker.VerifySign(dataBytes, sign));
        }
        
        [Fact]
        public void ShouldExportPrivateAndPublicKeyImportTheirAndValidateSign()
        {
            var dataString = "some data";
            var dataBytes = Encoding.UTF8.GetBytes(dataString);
            
            var publicKey = _encryptor.GetPublicKeyBytes();
            var privateKey = _encryptor.GetPrivateKeyBytes();

            var signEncryptor = AsymmetricEncryptor.FromPrivateKeyBytes(privateKey);
            var signChecker = AsymmetricEncryptor.FromPublicKeyBytes(publicKey);
            
            var sign = signEncryptor.SignData(dataBytes);
            
            Assert.True(signChecker.VerifySign(dataBytes, sign));
        }
        
        #endregion
    }
}