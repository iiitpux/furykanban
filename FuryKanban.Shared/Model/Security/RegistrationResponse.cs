using FuryKanban.Shared.Interfaces;

namespace FuryKanban.Shared.Model.Security
{
    public class RegistrationResponse : BaseError, IErrorResult
    {
        public string Token { set; get; }
    }
}