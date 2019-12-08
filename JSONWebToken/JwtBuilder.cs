using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace JSONWebToken
{
    public class JwtBuilder
    {
        private SignatureFactory SignatureFactory { get; set; }
        public JwtBuilder(ISignatureFactory factory)
        {
            SignatureFactory = (SignatureFactory)factory;
        }

        public string BuildHMACSHA384Signature(JsonWebToken jsonWebToken)
        {
            string unsignedToken = BuildUnsignedToken(jsonWebToken.JwtHeader, jsonWebToken.Payload);
            string signature = ((HMACSHA384Signature)SignatureFactory.CreateHMACSHA384Signature(unsignedToken, jsonWebToken.Secret)).Content;
            string signedToken = BuildSignedToken(unsignedToken, signature);
            return signedToken;
        }
        
        private string BuildUnsignedToken(JwtHeader jwtHeader, object payload)
        {
            string headerPart = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new
            {
                alg = jwtHeader.Algorithm,
                typ = jwtHeader.Type
            })));
            string payloadPart = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload)));
            return $"{headerPart}.{payloadPart}";
        }

        private string BuildSignedToken(string unsingedToken, string signature)
        {
            return unsingedToken + "." + signature;
        }
    }
}