namespace JSONWebToken
{
    public interface IJsonWebToken
    {
        JsonWebToken Create(object payload, string secret);
    }
}