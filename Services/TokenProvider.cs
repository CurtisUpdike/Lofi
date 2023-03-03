using MusicToken;

namespace Lofi.Services;

public class TokenProvider : ITokenProvider
{
	private string _teamId;
	private string _keyId;
	private string _authKey;

	public string Token
	{
		get
		{
			return TokenGenerator.GenerateToken(_authKey, _teamId, _keyId, new TimeSpan(2, 0, 0));
		}
	}

	public TokenProvider(IConfiguration config)
	{
		if (config["Lofi:TeamId"] == null) { throw new Exception("TeamId Configuration Not Found"); }
		if (config["Lofi:KeyId"] == null) { throw new Exception("KeyId Configuration Not Found"); }
		if (config["Lofi:AuthKey"] == null) { throw new Exception("AuthKey Configuration Not Found"); }

		_teamId = config["Lofi:TeamId"]!;
		_keyId = config["Lofi:KeyId"]!;
		_authKey = StripKey(config["Lofi:AuthKey"]!);
	}

	private string StripKey(string keyFromConfig)
	{
		return keyFromConfig
			.Replace("-----BEGIN PRIVATE KEY-----", "")
			.Replace("-----END PRIVATE KEY-----", "")
			.Replace("\n", "");
	}

}
