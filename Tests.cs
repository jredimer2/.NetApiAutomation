using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace ReqresInApiTests
{
    [TestFixture]
    public class ApiTests
    {
        private RestClient client = new RestClient();

        [SetUp]
        public void Setup()
        {
            // IConfigurationBuilder builder = (IConfigurationBuilder)new ConfigurationBuilder().AddUserSecrets<ApiTests>();
            // var configuration = builder.Build();
            // string username = configuration["Email"] ?? "";
            // string password = configuration["Password2"] ?? "";

            client = new RestClient("https://reqres.in/api");
        }

        [Test]
        public void GetListOfUsers_Returns200()
        {
                var request = new RestRequest("users", Method.Get);
                var response = client.Execute(request);

                Assert.AreEqual(200, (int)response.StatusCode);
        }

        [Test]
        public void GetSingleUser_Returns200()
        {
            var request = new RestRequest("users/2", Method.Get);
            var response = client.Execute(request);

            if (response.Content != null)
            {
                Assert.AreEqual(200, (int)response.StatusCode);

                var responseData = JsonSerializer.Deserialize<dynamic>(response.Content);
                var jsonObject = JObject.Parse(response.Content == null ? "" : response.Content);

                string? firstName = jsonObject?["data"]?["first_name"]?.ToString();
                string? lastName = jsonObject?["data"]?["last_name"]?.ToString();
                string? email = jsonObject?["data"]?["email"]?.ToString();

                // Assert that the values are correct 
                Assert.That(firstName, Is.EqualTo("Janet"));
                Assert.That(lastName, Is.EqualTo("Weaver"));
                Assert.That(email, Is.EqualTo("janet.weaver@reqres.in"));

            } else {
                Assert.Fail("Response content should not be null");
            }
        }

        [Test]
        public void CreateNewUser_Returns201()
        {
            var request = new RestRequest("users", Method.Post);
            request.AddJsonBody(new { first_name = "Dewitt", last_name = "Kemp" });
            var response = client.Execute(request);

            Assert.AreEqual(201, (int)response.StatusCode);
        }

        [Test]
        public void UpdateExistingUser_Returns200()
        {
            var request = new RestRequest("users/2", Method.Put);
            request.AddJsonBody(new { first_name = "Katelyn", last_name = "Phillips" });
            var response = client.Execute(request);

            Assert.AreEqual(200, (int)response.StatusCode);
        }

        [Test]
        public void UpdateExistingUserPatch_Returns200()
        {
            var request = new RestRequest("users/2", Method.Patch);
            request.AddJsonBody(new { first_name = "Amie", last_name = "Meadows" });
            var response = client.Execute(request);

            Assert.AreEqual(200, (int)response.StatusCode);
        }

        [Test]
        public void VerifyLindsayFergusonIsAUser_Returns200()
        {
            // Create the request
            var request = new RestRequest("users", Method.Get);

            // Execute the request
            var response = client.Execute(request);

            if (response.Content != null)
            {
                // Verify the response
                Assert.AreEqual(200, (int)response.StatusCode, "Status code should be 200 OK");
                Assert.IsNotNull(response.Content, "Response content should not be null");

                // Parse the JSON response
                JObject? jsonResponse = JObject.Parse(response.Content);

                // Verify that the expected name is present
                var expectedFirstName = "Lindsay";
                var expectedLastName = "Ferguson";
                JArray dataArray = (jsonResponse?["data"] as JArray) ?? new JArray();
                bool nameFound = false;

                if (dataArray != null)
                {
                    foreach (JToken dataObject in dataArray)
                    {
                        if (dataObject != null) {
                            if ((dataObject?["first_name"]?.Value<string>() ?? "") == expectedFirstName && (dataObject?["last_name"]?.Value<string>() ?? "") == expectedLastName)
                            {
                                nameFound = true;
                                break;
                            }
                        }
                    }
                }

                Assert.IsTrue(nameFound, $"JSON response does not contain '{expectedFirstName} {expectedLastName}' element");

            } else {
                Assert.Fail("Response content should not be null");        
            }
        }
    
        [Test]
        public void DeleteExistingUser_Returns204()
        {
            var request = new RestRequest("users/2", Method.Delete);
            var response = client.Execute(request);

            Assert.AreEqual(204, (int)response.StatusCode);
        }
    
        // Negative test
        [Test]
        public void GetNonExistingUser_Returns404()
        {
            var request = new RestRequest("users/999", Method.Get);
            var response = client.Execute(request);

            Assert.AreEqual(404, (int)response.StatusCode);
        }
    }

    internal class UsersResponse
    {
        public object? Data { get; internal set; }
    }
}
