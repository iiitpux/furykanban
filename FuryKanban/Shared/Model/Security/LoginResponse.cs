namespace FuryKanban.Shared.Model.Security
{
    public class LoginResponse
    {
        public string Token { set; get; }
        public bool HasError { set; get; } = false;
        public string ErrorMessage { set; get; }
    }
}