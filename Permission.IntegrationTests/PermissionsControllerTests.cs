using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Permission.Application.CommandService.Commands;
using Permission.Infrastructure.Dtos;
using System.Net;
using System.Text;
using Xunit;

namespace Permission.IntegrationTests
{
    public  class PermissionsControllerTests 
    {
        private readonly HttpClient _client;
        public PermissionsControllerTests()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _client = webAppFactory.CreateClient();
        }

        [Fact]
        public async Task GetPermissions_ReturnsOkResponse()
        {
            var response = await _client.GetAsync("/api/Permissions");
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var permissions = JsonConvert.DeserializeObject<List<PermissionDto>>(stringResponse);
            Assert.NotNull(permissions);
        }
        [Fact]
        public async Task CreatePermission_ReturnsOkResponse()
        {
            var command = new CreatePermissionCommand
            {
                EmployeeForename = "Test",
                EmployeeSurname = "Test",
                PermissionTypeId = 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/Permissions", content);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var permission = JsonConvert.DeserializeObject<PermissionDto>(stringResponse);
            Assert.NotNull(permission);
        }

        [Fact]
        public async Task UpdatePermission_ReturnsOkResponse()
        {
            var command = new UpdatePermissionCommand
            {
                Id = 1,
                EmployeeForename = "Test",
                EmployeeSurname = "Test",
                PermissionTypeId = 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/Permissions/{command.Id}", content);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var permission = JsonConvert.DeserializeObject<PermissionDto>(stringResponse);
            Assert.NotNull(permission);
        }
    }
}
