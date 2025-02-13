using AuthenticationService.Core.Common;
using System.Text.RegularExpressions;

namespace AuthenticationService.Core.Models
{
    public class UserModel
    {
        public AuthDataModel AuthData { get; }
        public Guid? Id { get; }
        public string Name { get; }
        public string PhoneNumber { get; }


        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return false;

            string pattern = @"^(\+7|7)?\d{10}$";

            Regex regex = new Regex(pattern);
            return regex.IsMatch(phoneNumber);
        }

        private UserModel(Guid? id, string name, string phoneNumber, AuthDataModel authData)
        {
            Id = id;
            Name = name;
            PhoneNumber = phoneNumber;
            AuthData = authData;
        }


        public static Result<UserModel> Create(Guid? id, string name, string phoneNumber, string email, string passwordHash)
        {
            var resultFactory = new ResultFactory<UserModel>();

            var authData = AuthDataModel.Create(email, passwordHash);
            if (authData.IsError)
                resultFactory.AddError(authData.Errors.ToArray());

            if (!IsValidPhoneNumber(phoneNumber))
                resultFactory.AddError("Номер телефона введён неверно");

            if (string.IsNullOrEmpty(name) || name.Length < 4)
                resultFactory.AddError("Имя введено неверно");

            resultFactory.SetResult(new UserModel(id, name, phoneNumber, authData.Value));

            return resultFactory.Create();
        }

    }
}
