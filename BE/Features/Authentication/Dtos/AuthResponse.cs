namespace BE.Features.Authentication.Dtos
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string ErrorMsg { get; set; }

        public AuthResponse ErrorRes(string errorMsg)
        {
            return new AuthResponse
            {
                ErrorMsg = errorMsg
            };
        }

        public AuthResponse SuccessResult(string token, int userId)
        {
            return new AuthResponse
            {
                Token = token,
                UserId = userId
            };
        }
    }
}