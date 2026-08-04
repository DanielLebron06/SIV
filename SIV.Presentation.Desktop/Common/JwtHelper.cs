using System;
using System.Text;
using System.Text.Json;

namespace SIV.Presentation.Desktop.Common
{
    public static class JwtHelper
    {
        public static string GetClaim(string token, string claimType)
        {
            if (string.IsNullOrWhiteSpace(token))
                return string.Empty;

            var parts = token.Split('.');
            if (parts.Length < 2)
                return string.Empty;

            var payload = parts[1];
            var padding = 4 - payload.Length % 4;
            if (padding < 4)
                payload += new string('=', padding);

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(payload.Replace('-', '+').Replace('_', '/')));
            using (var doc = JsonDocument.Parse(json))
            {
                if (doc.RootElement.TryGetProperty(claimType, out var claim))
                    return claim.GetString() ?? string.Empty;

                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    if (prop.Name.EndsWith(claimType, StringComparison.OrdinalIgnoreCase) ||
                        prop.Name.Contains(claimType))
                        return prop.Value.GetString() ?? string.Empty;
                }
            }

            return string.Empty;
        }

        public static string GetRole(string token)
        {
            var role = GetClaim(token, "role");
            if (!string.IsNullOrEmpty(role))
                return role;
            return GetClaim(token, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
        }

        public static string GetEmail(string token)
        {
            var email = GetClaim(token, "unique_name");
            if (!string.IsNullOrEmpty(email))
                return email;
            return GetClaim(token, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
        }
    }
}
