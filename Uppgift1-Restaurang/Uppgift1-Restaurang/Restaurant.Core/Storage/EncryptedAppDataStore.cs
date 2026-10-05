using System.Security.Cryptography;
using System.Text.Json;
using Restaurant.Core.Models;

namespace Restaurant.Core.Storage;

public sealed class EncryptedAppDataStore
{
    private const int SaltSize = 16;
    private const int NonceSize = 12;
    private const int TagSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 600_000;

    private readonly string _filePath;

    public EncryptedAppDataStore(string filePath)
    {
        _filePath = filePath;
    }

    public async Task SaveAsync(
        AppData data,
        string storagePassword)
    {
        byte[] plainData = JsonSerializer.SerializeToUtf8Bytes(data);
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] nonce = RandomNumberGenerator.GetBytes(NonceSize);
        byte[] key = Rfc2898DeriveBytes.Pbkdf2(
            storagePassword,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            KeySize);
        byte[] encryptedData = new byte[plainData.Length];
        byte[] tag = new byte[TagSize];
        string? temporaryFilePath = null;

        try
        {
            using var aes = new AesGcm(key, TagSize);
            aes.Encrypt(nonce, plainData, encryptedData, tag);

            byte[] fileData = [.. salt, .. nonce, .. tag, .. encryptedData];
            string fullFilePath = Path.GetFullPath(_filePath);
            string directoryPath = Path.GetDirectoryName(fullFilePath)!;
            temporaryFilePath = Path.Combine(
                directoryPath,
                $".{Path.GetFileName(fullFilePath)}.{Guid.NewGuid():N}.tmp");

            await File.WriteAllBytesAsync(temporaryFilePath, fileData);
            File.Move(temporaryFilePath, fullFilePath, overwrite: true);
            temporaryFilePath = null;
        }
        finally
        {
            if (temporaryFilePath is not null && File.Exists(temporaryFilePath))
                File.Delete(temporaryFilePath);

            CryptographicOperations.ZeroMemory(key);
            CryptographicOperations.ZeroMemory(plainData);
        }
    }

    public async Task<AppData> LoadAsync(string storagePassword)
    {
        if (!File.Exists(_filePath))
            return new AppData();

        byte[] fileData = await File.ReadAllBytesAsync(_filePath);
        int headerSize = SaltSize + NonceSize + TagSize;

        if (fileData.Length < headerSize)
            throw new InvalidDataException("The data file is invalid.");

        byte[] salt = fileData[..SaltSize];
        byte[] nonce = fileData[SaltSize..(SaltSize + NonceSize)];
        byte[] tag = fileData[(SaltSize + NonceSize)..headerSize];
        byte[] encryptedData = fileData[headerSize..];
        byte[] key = Rfc2898DeriveBytes.Pbkdf2(
            storagePassword,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            KeySize);
        byte[] plainData = new byte[encryptedData.Length];

        try
        {
            using var aes = new AesGcm(key, TagSize);
            aes.Decrypt(nonce, encryptedData, tag, plainData);

            return JsonSerializer.Deserialize<AppData>(plainData) ?? new AppData();
        }
        catch (CryptographicException)
        {
            throw new InvalidOperationException(
                "Incorrect storage password or damaged data file.");
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException(
                "The data file contains invalid application data.",
                exception);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(key);
            CryptographicOperations.ZeroMemory(plainData);
        }
    }
}
