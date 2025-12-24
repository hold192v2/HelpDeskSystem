using DTOs;
using Flurl.Http;
using Infrastracture.Application.DTOs;
using Keycloak.Net;
using Keycloak.Net.Models.Roles;
using MassTransit;

namespace AuthService.API.RabbitMq;

public class ChangeRoleConsumer : IConsumer<ChangeRoleRequestDto>
{
    public async Task Consume(ConsumeContext<ChangeRoleRequestDto> context)
    {
        var tokenResponse = await "https://auth.alpha-helpdesk.ru/realms/HelpDeskKeycloak/protocol/openid-connect/token"
            .PostUrlEncodedAsync(new
            {
                grant_type = "client_credentials",
                client_id = "auth-adminAPI-client",
                client_secret = "rFX2eRscaWRsqLMA6mq3z1bCNqqXjOhJ"
            })
            .ReceiveJson<SessionDTO>();

        var kc = new KeycloakClient(
            "https://auth.alpha-helpdesk.ru",
            () => tokenResponse.AccessToken
        );
        var currentRoles = await kc.GetRealmRoleMappingsForUserAsync("HelpDeskKeycloak", context.Message.UserId.ToString());
        var oldRole = currentRoles
            .FirstOrDefault(r => context.Message.RolesNameDelete.Contains(r.Name));
        if (oldRole != null)
        {
            await kc.DeleteRealmRoleMappingsFromUserAsync(
                "HelpDeskKeycloak",
                context.Message.UserId.ToString(),
                new[]
                {
                    new Role
                    {
                        Id = oldRole.Id,
                        Name = oldRole.Name
                    }
                });
        }
        var newRole = await kc.GetRoleByNameAsync("HelpDeskKeycloak", context.Message.RoleNameAssign);
        await kc.AddRealmRoleMappingsToUserAsync(
            "HelpDeskKeycloak",
            context.Message.UserId.ToString(),
            new[]
            {
                new Role
                {
                    Id = newRole.Id,
                    Name = newRole.Name
                }
            });
        var newCurrentRoles = await kc.GetRealmRoleMappingsForUserAsync("HelpDeskKeycloak", context.Message.UserId.ToString());
        await context.RespondAsync(new ChangeRoleResponse(newCurrentRoles.Any(r => r.Name == context.Message.RoleNameAssign)));
    }
}