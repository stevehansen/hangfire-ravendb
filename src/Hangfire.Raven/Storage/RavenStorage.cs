using Hangfire.Logging;
using Hangfire.Raven.Extensions;
using Hangfire.Raven.JobQueues;
using Hangfire.Storage;

namespace Hangfire.Raven.Storage
{
    public class RavenStorage : JobStorage
    {
        private readonly RavenStorageOptions _options;
        private readonly IRepository _repository;

        public RavenStorage(RepositoryConfig config)
            : this(config, new RavenStorageOptions())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="options"></param>
        public RavenStorage(RepositoryConfig config, RavenStorageOptions options)
            : this(new Repository(config), options)
        {
        }

        public RavenStorage(IRepository repository)
            : this(repository, new RavenStorageOptions())
        {
        }

        public RavenStorage(IRepository repository, RavenStorageOptions options)
        {
            repository.ThrowIfNull("repository");
            options.ThrowIfNull("options");

            _options = options;
            _repository = repository;

            _repository.Create();

            InitializeQueueProviders();
        }

        public RavenStorageOptions Options => _options;
        public IRepository Repository => _repository;

        public virtual PersistentJobQueueProviderCollection QueueProviders { get; private set; }

        public override IMonitoringApi GetMonitoringApi() => new RavenStorageMonitoringApi(this);

        public override IStorageConnection GetConnection() => new RavenConnection(this);

        public override void WriteOptionsToLog(ILog logger) => logger.Info("Using the following options for Raven job storage:");

        private void InitializeQueueProviders()
        {
            var defaultQueueProvider = new RavenJobQueueProvider(this, _options);
            QueueProviders = new PersistentJobQueueProviderCollection(defaultQueueProvider);
        }
    }
}