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
		var privateKey = config["Lofi:AuthKey"] ?? string.Empty;
		var teamId = config["Lofi:TeamId"] ?? string.Empty;
		var keyId = config["Lofi:KeyId"] ?? string.Empty;

		_tokenGenerator = new TokenGenerator(privateKey, teamId, keyId);
	}
}
