using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace Billi.Backend.CrossCutting.Utilities
{
    public class EncryptedStringConverter(IConfiguration configuration) 
        : ValueConverter<string, string>(
            v => CryptoHelper.Encrypt(v, configuration),
            v => CryptoHelper.Decrypt(v, configuration)
        )
    { }
}
