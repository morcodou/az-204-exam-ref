namespace OAuth2Authentication.Constants
{
    public class Clients
    {
        public readonly static Client Client1 = new Client
        {
            Id = "123456",
            Secret = "abcdef",
            RedirectUrl = Paths.AuthorizeCodeCallBackPath
        };

        public readonly static Client Client2 = new Client
        {
            Id = "78901",
            Secret = "aasdasdef",
            RedirectUrl = Paths.ImplicitGrantCallBackPath
        };

    }

    public class Client
    {
        public string Id { get; set; }
        public string Secret { get; set; }
        public string RedirectUrl { get; set; }
    }
}