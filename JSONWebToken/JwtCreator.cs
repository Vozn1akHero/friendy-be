namespace JSONWebToken
{
    public abstract class JwtCreator
    {
        protected abstract IJsonWebToken Method();

        public JsonWebToken Create(object payload, string secret)
        {
            var jwt = Method();
            return jwt.Create(payload, secret);
        }
    }
}