using AuthenticationService.Core.Common;
using System.Text.RegularExpressions;

namespace AuthenticationService.Core.Models
{
    public class UserModel
    {
        public AuthDataModel AuthData { get; }
        public Guid? Id { get; }


        private UserModel(Guid? id, AuthDataModel authData)
        {
            Id = id;
            AuthData = authData;
        }


        public static Result<UserModel> Create(Guid? id, string name, string phoneNumber, string email, string passwordHash)
        {
            var resultFactory = new ResultFactory<UserModel>();

            var authData = AuthDataModel.Create(email, passwordHash);
            if (authData.IsError)
                resultFactory.AddError(authData.Errors.ToArray());


            resultFactory.SetResult(new UserModel(id, authData.Value));

            return resultFactory.Create();
        }

    }
}
