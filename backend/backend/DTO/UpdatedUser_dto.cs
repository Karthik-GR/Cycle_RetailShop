namespace backend.DTO
{
    public class UpdatedUser_dto
    {
        public IFormFile? ImageFile { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }

    }
}
