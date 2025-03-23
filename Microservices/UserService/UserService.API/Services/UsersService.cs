using Google.Protobuf;
using Grpc.Core;
using UserService.Application.Services;
using UserService.Core.Enums;
using UserService.Core.Models;

namespace UserService.API.Services
{
    public class UsersService(UserActionsService actions, ILogger<UsersService> logger) : Users.UsersBase
    {
        public override async Task<CreateUserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            logger.LogDebug($"Start creating user id={request.Id}..");

            var response = await actions.CreateUserAsync(request.Id, request.Email);

            if (response.IsError)
            {
                var errors = string.Join(", ", response.Errors);

                logger.LogError(errors);
                throw new Exception(errors);
            }

            var user = response.Value;

            return new CreateUserReply
            {
                Email = user.Email,
                Id = user.Id.ToString(),
                Name = user.Name
            };
        }

        public async override Task<FindByNameUsersReply> FindByName(FindByNameUsersRequest request, ServerCallContext context)
        {
            var response = await actions.FindUsersByNameAsync(request.Name);

            if (response.IsError)
            {
                var errors = string.Join(", ", response.Errors);

                logger.LogError(errors);
                throw new Exception(errors);
            }

            var users = response.Value;

            var reply = new FindByNameUsersReply();

            foreach (var user in users)
            {
                reply.Users.Add(new UserShortInfo
                {
                    Id = user.Id.ToString(),
                    Name = user.Name,
                    ProfilePicture = new Picture
                    {
                        Byte = user.ProfilePicture.Bytes == null ? ByteString.Empty : ByteString.CopyFrom(user.ProfilePicture.Bytes),
                        Extension = user.ProfilePicture.Extension,
                        Name = user.ProfilePicture.Name
                    }
                });
            }

            return reply;
        }

        public override async Task<GetUserReply> GetUser(GetUserRequest request, ServerCallContext context)
        {
            var response = await actions.GetUserAsync(request.Id);

            if (response.IsError)
            {
                var errors = string.Join(", ", response.Errors);

                logger.LogError(errors);
                throw new Exception(errors);
            }

            var user = response.Value;

            return new GetUserReply
            {
                Info = new UserFullInfo
                {
                    Email = user.Email,
                    Gender = (int)user.Gender,
                    Id = user.Id.ToString(),
                    Name = user.Name,
                    PhoneNumber = user.Phone,
                    ProfilePicture = new Picture
                    {
                        Byte = user.ProfilePicture.Bytes == null? ByteString.Empty : ByteString.CopyFrom(user.ProfilePicture.Bytes),
                        Extension = user.ProfilePicture.Extension,
                        Name = user.ProfilePicture.Name
                    }
                }
            };
        }

        public async override Task<GetUsersReply> GetUsers(GetUsersRequest request, ServerCallContext context)
        {
            var response = await actions.GetUsersAsync(request.Id.ToArray());

            if (response.IsError)
            {
                var errors = string.Join(", ", response.Errors);

                logger.LogError(errors);
                throw new Exception(errors);
            }

            var users = response.Value;

            var reply = new GetUsersReply();

            foreach (var user in users)
            {
                reply.Users.Add(new UserShortInfo
                {
                    Id = user.Id.ToString(),
                    Name = user.Name,
                    ProfilePicture = new Picture
                    {
                        Byte = user.ProfilePicture.Bytes == null ? ByteString.Empty : ByteString.CopyFrom(user.ProfilePicture.Bytes),
                        Extension = user.ProfilePicture.Extension,
                        Name = user.ProfilePicture.Name
                    }
                });
            }

            return reply;
        }

        public override async Task<UpdateUserReply> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            var userInfo = request.Info;
            var updatingUser = UserModel.Create(userInfo.Id,
                                                userInfo.Name,
                                                userInfo.Email,
                                                userInfo.ProfilePicture == null ? new Core.ValueObjects.Picture() : new Core.ValueObjects.Picture
                                                {
                                                    Bytes = userInfo.ProfilePicture.Byte.ToByteArray(),
                                                    Name = userInfo.ProfilePicture.Name,
                                                    Extension = userInfo.ProfilePicture.Extension
                                                },
                                                userInfo.PhoneNumber,
                                                (GenderCode)userInfo.Gender);
            if (updatingUser.IsError)
            {
                var errors = string.Join(", ", updatingUser.Errors);

                logger.LogError(errors);
                throw new Exception(errors);
            }

            var response = await actions.UpdateUserAsync(updatingUser.Value);

            if (response.IsError)
            {
                var errors = string.Join(", ", response.Errors);

                logger.LogError(errors);
                throw new Exception(errors);
            }

            var user = response.Value;

            return new UpdateUserReply
            {
                Id = user.ToString(),
            };
        }
    }
}
