using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bootstrap.Domain;
using Bootstrap.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Bootstrap.WebApi.Configuration.Authentication.Bearer;

public class JwtTokenGenerator
{
    private readonly IClock _clock;
    private readonly JwtTokenOptions _options;

    public JwtTokenGenerator(IOptions<JwtTokenOptions> options, IClock clock)
    {
        _clock = clock;
        _options = options.Value;
    }

    public JwtTokenJson Generate(UserId userId, string email)
    {
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.Value.ToString()),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Name, email)
        };
        var token = GetToken(authClaims);
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return new JwtTokenJson(tokenValue, token.ValidTo);
    }

    private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));

        return new JwtSecurityToken(
            _options.ValidIssuer,
            _options.ValidAudience,
            expires: _clock.Now().Add(_options.Validity),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
    }
}
