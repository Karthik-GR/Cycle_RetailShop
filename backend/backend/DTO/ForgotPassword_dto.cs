namespace backend.DTO
{
    public class ForgotPassword_dto
    {
        public string Email { get; set; }
    }

    public class ResetPassword_dto
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
