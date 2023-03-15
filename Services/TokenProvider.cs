using AppleDeveloperToken;

namespace Lofi.Services;

public class TokenProvider : ITokenProvider
{
	private readonly TokenGenerator _tokenGenerator;

	public string Token
	{
		get
		{
			return _tokenGenerator.Generate(TimeSpan.FromDays(30));
		}
	}

	public TokenProvider(IConfiguration config)
	{
		var privateKey = config["Apple:PrivateKey"] ?? string.Empty;
		var teamId = config["Apple:TeamId"] ?? string.Empty;
		var keyId = config["Apple:KeyId"] ?? string.Empty;

		_tokenGenerator = new TokenGenerator(privateKey, teamId, keyId);
	}
}
