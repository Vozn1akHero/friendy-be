namespace JSONWebToken
{
    public class JwtHeader
    {
        public string Type { get;} = "jwt";
        public string Algorithm { get; set; }
    }
}