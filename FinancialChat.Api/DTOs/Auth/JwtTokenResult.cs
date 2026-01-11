namespace FinancialChat.Api.Contracts.Auth;

public record JwtTokenResult(
    string AccessToken,
    int ExpiresIn
);
