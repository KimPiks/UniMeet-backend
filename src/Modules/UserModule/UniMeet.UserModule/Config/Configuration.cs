namespace UniMeet.UserModule.Config;

internal record Configuration(string ConnectionString, string WebsiteUrl, AuthConfiguration Auth);
internal record AuthConfiguration(string Secret, string Issuer, string Audience);