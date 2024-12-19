using System.IdentityModel.Tokens.Jwt;
using Stackoverflow_Lite.Utils.Interfaces;
using Stackoverflow_Lite.Utils.Interfaces;

namespace Stackoverflow_Lite.Utils;

public class TokenClaimsExtractor : ITokenClaimsExtractor
{
    public string ExtractClaim(string token, string claimName)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        return jwtToken?.Claims.FirstOrDefault(c => c.Type == claimName)?.Value;
    }
}