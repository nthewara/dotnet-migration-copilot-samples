using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Graph;
using Microsoft.Graph.Me.CheckMemberGroups;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace ContosoUniversity.Helpers
{
    public static class GraphAuthorizationHelper
    {
        private sealed class BearerTokenAuthenticationProvider : IAuthenticationProvider
        {
            private readonly string accessToken;

            public BearerTokenAuthenticationProvider(string accessToken)
            {
                this.accessToken = accessToken;
            }

            public Task AuthenticateRequestAsync(RequestInformation request, Dictionary<string, object> additionalAuthenticationContext = null, CancellationToken cancellationToken = default(CancellationToken))
            {
                request.Headers.Add("Authorization", new AuthenticationHeaderValue("Bearer", accessToken).ToString());
                request.Headers.Add("ConsistencyLevel", "eventual");
                return Task.FromResult(0);
            }
        }

        public static string GetUserObjectId(ClaimsPrincipal user)
        {
            var identity = user == null ? null : user.Identity as ClaimsIdentity;
            return identity == null
                ? null
                : identity.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value
                    ?? identity.FindFirst("oid")?.Value;
        }

        public static string GetUserAccessToken()
        {
            return HttpContext.Current?.Session?["AccessToken"] as string;
        }

        public static GraphServiceClient GetGraphServiceClient(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new InvalidOperationException("A Microsoft Graph access token is required for group authorization checks.");
            }

            return new GraphServiceClient(new BearerTokenAuthenticationProvider(accessToken));
        }

        public static async Task<bool> IsUserInRequiredGroupAsync(string accessToken)
        {
            var requiredGroupId = ConfigurationManager.AppSettings["RequiredGroupId"];
            if (string.IsNullOrWhiteSpace(requiredGroupId) || requiredGroupId == "YOUR_ADMIN_GROUP_ID")
            {
                return false;
            }

            var graphClient = GetGraphServiceClient(accessToken);
            var requestBody = new CheckMemberGroupsPostRequestBody
            {
                GroupIds = new List<string> { requiredGroupId }
            };

            var result = await graphClient.Me.CheckMemberGroups.PostAsCheckMemberGroupsPostResponseAsync(requestBody);
            return result?.Value != null && result.Value.Contains(requiredGroupId);
        }
    }
}
