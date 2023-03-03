using MusicToken;

namespace Lofi.Services;

public class TokenProvider : ITokenProvider
{
    private string? _teamId;
    private string? _keyId;
    private string? _authKey;

    public string Token
    {
        get
        {
            return TokenGenerator.GenerateToken(_authKey, _teamId, _keyId, new TimeSpan(2, 0, 0));
        }
    }

    public TokenProvider(IConfiguration config)
    {
        _teamId = config["Lofi:TeamId"];
        _keyId = config["Lofi:KeyId"];
        _authKey = config["Lofi:AuthKey"];
    }
}
