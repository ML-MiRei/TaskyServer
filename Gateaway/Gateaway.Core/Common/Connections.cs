using Grpc.Net.Client;
using Gateaway.Core.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gateaway.Core.Common
{
    public class Connections
    {
        private static ILogger<Connections> _logger;
        private static IOptions<ConnectionOptions> _options;
        
        private static Sender.SenderClient _senderServiceClient;

        private static Projects.ProjectsClient _projectServiceClient;
        private static Tasks.TasksClient _projectTaskServiceClient;
        private static Sprints.SprintsClient _sprintServiceClient;
        private static Members.MembersClient _membersServiceClient;

        public Connections(ILogger<Connections> logger, IOptions<ConnectionOptions> options)
        {
            _logger = logger;
            _options = options;
        }

        public static Sender.SenderClient SenderServiceClient
        {
            get
            {
                if (_senderServiceClient == null)
                {
                    try
                    {
                        var channel = GrpcChannel.ForAddress(_options.Value.SenderServiceConnectionString);
                        _senderServiceClient = new Sender.SenderClient(channel);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw new Exception("Ошибка подключения к сервису отправки");
                    }

                }
                return _senderServiceClient;
            }
        }
        public static Projects.ProjectsClient ProjectServiceClient
        {
            get
            {
                if (_projectServiceClient == null)
                {
                    try
                    {
                        var channel = GrpcChannel.ForAddress(_options.Value.ProjectsServiceConnectionString);
                        _projectServiceClient = new Projects.ProjectsClient(channel);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw new Exception("Ошибка подключения к сервису проектов");
                    }

                }
                return _projectServiceClient;
            }
        }
        public static Tasks.TasksClient TeamServiceClient
        {
            get
            {
                if (_projectTaskServiceClient == null)
                {
                    try
                    {
                        var channel = GrpcChannel.ForAddress(_options.Value.ProjectsServiceConnectionString);
                        _projectTaskServiceClient = new Tasks.TasksClient(channel);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw new Exception("Ошибка подключения к сервису заданий");
                    }

                }
                return _projectTaskServiceClient;
            }
        }
        public static Sprints.SprintsClient NotificationServiceClient
        {
            get
            {
                if (_sprintServiceClient == null)
                {
                    try
                    {
                        var channel = GrpcChannel.ForAddress(_options.Value.ProjectsServiceConnectionString);
                        _sprintServiceClient = new Sprints.SprintsClient(channel);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw new Exception("Ошибка подключения к сервису спринтов");
                    }

                }
                return _sprintServiceClient;
            }
        }



        public static Members.MembersClient MembersServiceClient
        {
            get
            {
                if (_membersServiceClient == null)
                {
                    try
                    {
                        var channel = GrpcChannel.ForAddress(_options.Value.ProjectsServiceConnectionString);
                        _membersServiceClient = new Members.MembersClient(channel);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw new Exception("Ошибка подключения к сервису участников проектов");
                    }

                }
                return _membersServiceClient;
            }
        }
       

    }
}
