using Getaway.Core.Contracts.Boards;
using Getaway.Core.Contracts.BoardTasks;
using Getaway.Core.Contracts.Comments;
using Getaway.Core.Contracts.Executions;
using Getaway.Core.Contracts.Members;
using Getaway.Core.Contracts.ProjectBoards;
using Getaway.Core.Contracts.Projects;
using Getaway.Core.Contracts.Sprints;
using Getaway.Core.Contracts.Stages;
using Getaway.Core.Contracts.Tasks;
using Getaway.Core.Contracts.Users;
using Grpc.Net.Client;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Gateaway.Core.Common
{
    public class Connections
    {
        private ILogger<Connections> _logger;
        private IOptions<ConnectionOptions> _options;

        private Projects.ProjectsClient _projectsServiceClient;
        private ProjectBoards.ProjectBoardsClient _projectBoardsServiceClient;
        private Members.MembersClient _membersServiceClient;

        private Users.UsersClient _usersServiceClient;

        private Boards.BoardsClient _boardsServiceClient;
        private Sprints.SprintsClient _sprintsServiceClient;
        private Stages.StagesClient _stagesServiceClient;
        private BoardTasks.BoardTasksClient _boardTasksServiceClient;

        private Tasks.TasksClient _tasksServiceClient;
        private Comments.CommentsClient _commentsServiceClient;
        private Executions.ExecutionsClient _executionsServiceClient;

        public Connections(ILogger<Connections> logger, IOptions<ConnectionOptions> options)
        {
            _logger = logger;
            _options = options;
        }

        public Projects.ProjectsClient ProjectServiceClient
        {
            get
            {
                if (_projectsServiceClient == null)
                {
                    try
                    {
                        var httpHandler = new HttpClientHandler();
                        httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                        var channel = GrpcChannel.ForAddress(_options.Value.ProjectsService, new GrpcChannelOptions { HttpHandler = httpHandler});
                        _projectsServiceClient = new Projects.ProjectsClient(channel);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                        throw new Exception("Ошибка подключения к сервису проектов");
                    }

                }
                return _projectsServiceClient;
            }
        }
        public ProjectBoards.ProjectBoardsClient ProjectBoardsServiceClient
        {
            get
            {
                if (_projectBoardsServiceClient == null)
                {
                    try
                    {
                        var channel = GrpcChannel.ForAddress(_options.Value.ProjectsService);
                        _projectBoardsServiceClient = new ProjectBoards.ProjectBoardsClient(channel);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw new Exception("Ошибка подключения к сервису проектов");
                    }

                }
                return _projectBoardsServiceClient;
            }
        }
        public Members.MembersClient MembersServiceClient
        {
            get
            {
                if (_membersServiceClient == null)
                {
                    try
                    {
                        var channel = GrpcChannel.ForAddress(_options.Value.ProjectsService);
                        _membersServiceClient = new Members.MembersClient(channel);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw new Exception("Ошибка подключения к сервису проектов");
                    }

                }
                return _membersServiceClient;
            }
        }

        public Users.UsersClient UsersServiceClient
        {
            get
            {
                if (_usersServiceClient == null)
                {
                    try
                    {
                        var channel = GrpcChannel.ForAddress(_options.Value.UsersService);
                        _usersServiceClient = new Users.UsersClient(channel);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw new Exception("Ошибка подключения к сервису пользователей");
                    }

                }
                return _usersServiceClient;
            }
        }

        public Boards.BoardsClient BoardsServiceClient
        {
            get
            {
                if (_boardsServiceClient == null)
                {
                    try
                    {
                        var channel = GrpcChannel.ForAddress(_options.Value.BoardsService);
                        _boardsServiceClient = new Boards.BoardsClient(channel);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw new Exception("Ошибка подключения к сервису досок");
                    }

                }
                return _boardsServiceClient;
            }
        }
        public Sprints.SprintsClient SprintsServiceClient
        {
            get
            {
                if (_sprintsServiceClient == null)
                {
                    try
                    {
                        var channel = GrpcChannel.ForAddress(_options.Value.BoardsService);
                        _sprintsServiceClient = new Sprints.SprintsClient(channel);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw new Exception("Ошибка подключения к сервису досок");
                    }

                }
                return _sprintsServiceClient;
            }
        }
        public Stages.StagesClient StagesServiceClient
        {
            get
            {
                if (_stagesServiceClient == null)
                {
                    try
                    {
                        var channel = GrpcChannel.ForAddress(_options.Value.BoardsService);
                        _stagesServiceClient = new Stages.StagesClient(channel);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw new Exception("Ошибка подключения к сервису досок");
                    }

                }
                return _stagesServiceClient;
            }
        }
        public BoardTasks.BoardTasksClient BoardTasksServiceClient
        {
            get
            {
                if (_boardTasksServiceClient == null)
                {
                    try
                    {
                        var channel = GrpcChannel.ForAddress(_options.Value.BoardsService);
                        _boardTasksServiceClient = new BoardTasks.BoardTasksClient(channel);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw new Exception("Ошибка подключения к сервису досок");
                    }

                }
                return _boardTasksServiceClient;
            }
        }

        public Tasks.TasksClient TasksServiceClient
        {
            get
            {
                if (_tasksServiceClient == null)
                {
                    try
                    {
                        var channel = GrpcChannel.ForAddress(_options.Value.TasksService);
                        _tasksServiceClient = new Tasks.TasksClient(channel);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw new Exception("Ошибка подключения к сервису заданий");
                    }

                }
                return _tasksServiceClient;
            }
        }
        public Comments.CommentsClient CommentsServiceClient
        {
            get
            {
                if (_commentsServiceClient == null)
                {
                    try
                    {
                        var channel = GrpcChannel.ForAddress(_options.Value.TasksService);
                        _commentsServiceClient = new Comments.CommentsClient(channel);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw new Exception("Ошибка подключения к сервису заданий");
                    }

                }
                return _commentsServiceClient;
            }
        }
        public Executions.ExecutionsClient ExecutionsServiceClient
        {
            get
            {
                if (_executionsServiceClient == null)
                {
                    try
                    {
                        var channel = GrpcChannel.ForAddress(_options.Value.TasksService);
                        _executionsServiceClient = new Executions.ExecutionsClient(channel);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw new Exception("Ошибка подключения к сервису заданий");
                    }

                }
                return _executionsServiceClient;
            }
        }

    }
}
