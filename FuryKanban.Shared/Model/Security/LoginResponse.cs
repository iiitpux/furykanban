using FuryKanban.Shared.Interfaces;

namespace FuryKanban.Shared.Model.Security
{
    public class LoginResponse : BaseError, IErrorResult
    {
        public string Token { set; get; }
    }
}