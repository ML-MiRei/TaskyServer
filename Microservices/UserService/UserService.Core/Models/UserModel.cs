using System.Text.RegularExpressions;
using UserService.Core.Common;
using UserService.Core.Enums;
using UserService.Core.ValueObjects;

namespace UserService.Core.Models
{
    public class UserModel
    {
        public const int MAX_NAME_LENGTH = 50;

        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Phone { get; }
        public Picture ProfilePicture { get; private set; }
        public GenderCode Gender { get; }

        private UserModel(Guid id, string name, string email, Picture profilePicture, GenderCode gender)
        {
            Id = id;
            Name = name;
            Email = email;
            ProfilePicture = profilePicture;
            Gender = gender;
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            phoneNumber = phoneNumber.Replace(" ", "").Replace("-", "");

            string pattern = @"^[0-9]{7,15}$";

            return Regex.IsMatch(phoneNumber, pattern);
        }

        public static string FormatPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return phoneNumber;
            }

            phoneNumber = phoneNumber.Replace(" ", "").Replace("-", "");

            string formattedNumber = "";
            for (int i = 0; i < phoneNumber.Length; i++)
            {
                formattedNumber += phoneNumber[i];
                if ((i + 1) % 3 == 0 && i != phoneNumber.Length - 1)
                {
                    formattedNumber += "-";
                }
            }

            return formattedNumber;
        }


        public void ClearProfilePicture()
        {
            ProfilePicture = null;
        }


        public static Result<UserModel> Create(Guid id, string name, string email, Picture profilePicture = null, string phoneNumber = null, GenderCode gender = GenderCode.Unknown)
        {
            var resultFactory = new ResultFactory<UserModel>();

            name = name.Trim();
            email = email.Trim();

            if (string.IsNullOrEmpty(name))
                resultFactory.AddError("Имя не может быть пустым");

            if (name.Length > MAX_NAME_LENGTH)
                resultFactory.AddError($"Имя не может быть длиннее {MAX_NAME_LENGTH} символов");

            if (string.IsNullOrEmpty(name))
                resultFactory.AddError("Email не может быть пустым");


            if (phoneNumber != null)
            {
                if (IsValidPhoneNumber(phoneNumber))
                    phoneNumber = FormatPhoneNumber(phoneNumber);
                else
                    resultFactory.AddError("Номер телефона введён неверно");
            }

            var user = new UserModel(id, name, email, profilePicture, gender);

            resultFactory.SetResult(user);

            return resultFactory.Create();
        }


    }
}
