using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Expiration;
using Raven.Client.Documents.Session;
using Raven.Embedded;
using Raven.TestDriver;
using System;
using System.Collections.Generic;

namespace Hangfire.Raven.Tests
{
    public class TestRepository : RavenTestDriver, IRepository
    {
        private readonly IDocumentStore _documentStore;

        static TestRepository()
        {
            // RavenDB 6+ refuses to start an embedded server without a license; tests run unlicensed.
            ConfigureServer(new TestServerOptions
            {
                Licensing = new ServerOptions.LicensingOptions { ThrowOnInvalidOrMissingLicense = false }
            });
        }

        public TestRepository()
        {
            _documentStore = GetDocumentStore(null, "hangfire-raven-tests");

            _documentStore.Maintenance.Send(new ConfigureExpirationOperation(new ExpirationConfiguration
            {
                Disabled = false,
                DeleteFrequencyInSec = 3
            }));
        }

        public IDocumentStore DocumentStore => _documentStore;

        public void Create()
        {
        }

        public void Destroy()
        {
        }

        public void ExecuteIndexes(List<AbstractIndexCreationTask> indexes) => _documentStore.ExecuteIndexes(indexes);

        public string GetId(Type type, params string[] id) => type.ToString() + '/' + string.Join("/", id);

        public IDocumentSession OpenSession() => _documentStore.OpenSession();
    }
}
