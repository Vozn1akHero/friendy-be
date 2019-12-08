namespace JSONWebToken
{
    public class JsonWebToken
    {
        public JwtHeader JwtHeader { get; set; }
        public object Payload { get; set; }
        public string Secret { get; set; }
    }
}