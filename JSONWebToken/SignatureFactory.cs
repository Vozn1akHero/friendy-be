using System;
using System.Security.Cryptography;
using System.Text;

namespace JSONWebToken
{
    public class SignatureFactory : ISignatureFactory
    {
        public IHMACSHA384Signature CreateHMACSHA384Signature(string unsignedToken, string secret)
        {
            using (var hmacsha256 = new HMACSHA384(Encoding.UTF8.GetBytes(secret)))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(unsignedToken));
                return new HMACSHA384Signature
                {
                    Content = Convert.ToBase64String(hashmessage)
                };
            }
        }
    }
}