using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

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
			return CreateToken(LoadPrivateKey(), new TimeSpan(2, 0, 0)); ;
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

	private ECDsa LoadPrivateKey()
	{
		var privateKey = Convert.FromBase64String(_authKey);
		var privateKeyECDsa = ECDsa.Create();
		privateKeyECDsa.ImportPkcs8PrivateKey(privateKey, out _);
		return privateKeyECDsa;
	}

	private string CreateToken(ECDsa ecdsa, TimeSpan timeSpan)
	{
		var now = DateTime.UtcNow;
		var tokenHandler = new JwtSecurityTokenHandler();

		var token = tokenHandler.CreateJwtSecurityToken(
			issuer: _teamId,
			subject: null,
			notBefore: now,
			expires: now.Add(timeSpan),
			issuedAt: now,
			signingCredentials: new SigningCredentials(
				new ECDsaSecurityKey(ecdsa) { KeyId = _keyId },
				SecurityAlgorithms.EcdsaSha256));

		return tokenHandler.WriteToken(token);
	}
}
