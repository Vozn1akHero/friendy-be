namespace JSONWebToken
{
    public interface ISignatureFactory
    {
        IHMACSHA384Signature CreateHMACSHA384Signature(string unsignedToken, string secret);
    }
}