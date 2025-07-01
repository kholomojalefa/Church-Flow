namespace ChurchFlowAPI.DTOs
{
    public class AuthResult
    {
        public bool Succeeded { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
