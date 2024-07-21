using RestSharp;

namespace Tests
{
    [TestFixture]
    public class APITest
    {
        private WebAppFactory _factory;
        private HttpClient _client;
        private RestClient client;
        
        [OneTimeSetUp]
        public void Setup()
        {
            Console.WriteLine("**** Starting the server ****");
            _factory = new WebAppFactory();
            /*
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseUrls("http://localhost:5045");
                builder.ConfigureKestrel(options =>
                {
                    options.Listen(IPAddress.Any, 5045, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                    });
                });
            });
            */
            Console.WriteLine("**** Server started ****");
            ///Thread.Sleep(5000);      
            //_client = _factory.CreateClient();


            Console.WriteLine("**** Setting up the client ****");
            client = new RestClient("http://localhost:5045");
        }

        [Test]
        public void Test_ClientCreation()
        {
            Assert.That(client, Is.Not.Null);
        }

        [Test]
        public void Test_GetRequest()
        {
            // Arrange
            var request = new RestRequest("/weatherforecast", Method.Get );

            // Act
            var response = client.Execute(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [Test]
        public void Test_ClientExecution()
        {
            // Arrange
            var request = new RestRequest("/weatherforecast", Method.Get);

            // Act
            RestResponse response = null;
            Assert.DoesNotThrow(() => response = client.Execute(request));

            // Assert
            //Console.WriteLine("**** This is the response **** "+ response.IsSuccessful); 
            Assert.That(response.IsSuccessful, Is.EqualTo(true));
            Assert.That(response.ContentLength, Is.Not.EqualTo(0) );
        }

        [Test]
        public void Test_ClientErrorHandling()
        {
            // Arrange
            var request = new RestRequest("/nonexistent", Method.Get);

            // Act
            var response = client.Execute(request);

            // Assert
            Assert.That(response.StatusCode, Is.Not.EqualTo(System.Net.HttpStatusCode.OK));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            client.Dispose();
            //_client.Dispose();
            Console.WriteLine("**** Stopping the server ****");
            _factory.Dispose();
            Console.WriteLine("**** Server stopped ****");
        }
    }

}