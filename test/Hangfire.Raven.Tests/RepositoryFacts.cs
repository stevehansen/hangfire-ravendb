using Hangfire.Raven.Extensions;
using Raven.Client.ServerWide.Operations;
using System;
using Xunit;

namespace Hangfire.Raven.Tests
{
    public class RepositoryFacts
    {
        [Fact]
        public void Create_CreatesDatabaseWithExpiration_AndDestroyDeletesIt()
        {
            using (var server = new TestRepository())
            {
                var store = server.DocumentStore;
                var config = new RepositoryConfig
                {
                    ConnectionUrl = store.Urls[0],
                    Database = "hangfire-raven-create-" + Guid.NewGuid().ToString("N")
                };

                using (var repository = new Repository(config))
                {
                    Assert.False(store.DatabaseExists(config.Database));

                    repository.Create();

                    var record = store.Maintenance.Server.Send(new GetDatabaseRecordOperation(config.Database));
                    Assert.NotNull(record);
                    Assert.False(record.Expiration.Disabled);

                    repository.Destroy();

                    Assert.False(store.DatabaseExists(config.Database));
                }
            }
        }
    }
}
