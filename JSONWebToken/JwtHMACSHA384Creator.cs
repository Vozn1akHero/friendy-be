namespace JSONWebToken
{
    public class JwtHMACSHA384Creator : JwtCreator //concrete creator
    {
        protected override IJsonWebToken Method()
        {
            return new JwtHMACSHA384();
        }
    }
}