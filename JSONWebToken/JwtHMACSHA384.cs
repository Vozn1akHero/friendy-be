namespace JSONWebToken
{
    public class JwtHMACSHA384 : IJsonWebToken //concrete product
    {
        public JsonWebToken Create(object payload, string secret)
        {
            return new JsonWebToken
            {
                JwtHeader = new JwtHeader()
                {
                    Algorithm = "SHA384"
                },
                Payload = payload,
                Secret = secret
            };
        }
    }
}