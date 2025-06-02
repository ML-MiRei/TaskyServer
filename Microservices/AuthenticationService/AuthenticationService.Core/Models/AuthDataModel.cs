using AuthenticationService.Core.Common;
using System.Text.RegularExpressions;

namespace AuthenticationService.Core.Models
{
    public class AuthDataModel
    {
        public string? UserId { get;} = null;
        public string Email { get; }
        public string PasswordHash { get; }
        public bool IsVerified {  get; }
        private AuthDataModel(string email, string passwordHash, string? userId, bool isVerified)
        {
            Email = email;
            PasswordHash = passwordHash;
            UserId = userId;
            IsVerified = isVerified;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            string pattern = @"^(?=.*[a-zA-Z])(?=.*\d)(?!.*\s).{10,}$";

            Regex regex = new Regex(pattern);
            return regex.IsMatch(password);
        }


        public static Result<AuthDataModel> Create(string email, string passwordHash, string? userId = null, bool isVerified = false)
        {
            var resultFactory = new ResultFactory<AuthDataModel>();

            if (!IsValidEmail(email))
                resultFactory.AddError("Неверное значение email");

            if (!IsValidPassword(passwordHash))
                resultFactory.AddError("Неверное значение пароля");

            resultFactory.SetResult(new AuthDataModel(email, passwordHash, userId, isVerified));

            return resultFactory.Create();
        }

    }
}
