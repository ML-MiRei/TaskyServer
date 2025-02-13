using AuthenticationService.Core.Common;
using System.Text.RegularExpressions;

namespace AuthenticationService.Core.Models
{
    public class AuthDataModel
    {
        public string Email { get; }
        public string PasswordHash { get; }

        private AuthDataModel(string email, string passwordHash)
        {
            Email = email;
            PasswordHash = passwordHash;
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


        public static Result<AuthDataModel> Create(string email, string passwordHash)
        {
            var resultFactory = new ResultFactory<AuthDataModel>();

            if (!IsValidEmail(email))
                resultFactory.AddError("Неверное значение email");

            if (!IsValidPassword(passwordHash))
                resultFactory.AddError("Неверное значение пароля");

            resultFactory.SetResult(new AuthDataModel(email, passwordHash));

            return resultFactory.Create();
        }

    }
}
