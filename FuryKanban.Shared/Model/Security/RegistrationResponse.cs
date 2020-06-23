using FuryKanban.Shared.Interfaces;

namespace FuryKanban.Shared.Model.Security
{
    public class RegistrationResponse : IErrorResult
    {
        //todo- to the base class with LoginResponce
        public string Token { set; get; }
        public bool HasError { set; get; } = false;
        public string ErrorMessage { set; get; }
    }
}