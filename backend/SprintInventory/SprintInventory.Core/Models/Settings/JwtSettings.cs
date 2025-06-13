namespace SprintInventory.Core.Models.Settings;

public record JwtSettings(string Audience, string Issuer, string Secret, int ExpirationHours);