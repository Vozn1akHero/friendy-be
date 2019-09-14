namespace BE.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwt(string baseValue);
        bool ValidateJwt(string token);
    }
}