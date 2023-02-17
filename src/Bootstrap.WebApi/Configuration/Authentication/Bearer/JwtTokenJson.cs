namespace Bootstrap.WebApi.Configuration.Authentication.Bearer;

public record JwtTokenJson(string Token, DateTime Expiration);
