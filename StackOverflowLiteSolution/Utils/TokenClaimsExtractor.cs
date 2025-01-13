using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;
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
    
    public bool IsAdmin(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var realmAccessClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "realm_access")?.Value;

        if (string.IsNullOrEmpty(realmAccessClaim))
            return false;

        var realmAccess = JObject.Parse(realmAccessClaim);

        var roles = realmAccess["roles"]?.ToObject<List<string>>();
        return roles != null && roles.Contains("admin");
    }
}