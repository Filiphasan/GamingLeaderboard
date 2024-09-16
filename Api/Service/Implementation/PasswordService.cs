using System.Security.Cryptography;
using System.Text;
using Api.Service.Interface;

namespace Api.Service.Implementation;

public class PasswordService : IPasswordService
{
    public async Task<string> HashPasswordAsync(string password)
    {
        byte[] valueBytes = Encoding.UTF8.GetBytes(password);

        using var sha256Service = SHA256.Create();

        await using Stream stream = new MemoryStream(valueBytes);

        byte[] hashBytes = await sha256Service.ComputeHashAsync(stream);

        return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
    }

    public async Task<bool> VerifyPasswordAsync(string password, string hashedPassword)
    {
        return string.Equals(await HashPasswordAsync(password), hashedPassword, StringComparison.Ordinal);
    }
}