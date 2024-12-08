namespace Stackoverflow_Lite.Utils.Interfaces;

public interface ITokenClaimsExtractor
{ 
    string ExtractClaim(string token, string claimName);
}