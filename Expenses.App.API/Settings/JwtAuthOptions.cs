namespace Expenses.App.API.Settings;

public sealed class JwtAuthOptions
{
    // GET and SET

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string Key { get; set; }

    // GET and INIT

    public int ExpiredInMinutes { get; init; }

    public int RefreshTokenExpirationDays { get; init; }
}
