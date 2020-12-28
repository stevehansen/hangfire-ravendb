//using Raven.Abstractions.Data;

//using Raven.Client.Indexes;
//using Raven.Client.Embedded;
//using Raven.Embedded;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Expiration;
using Raven.Client.Documents.Session;
using Raven.TestDriver;
using System;
using System.Collections.Generic;

//using Hangfire.Raven.Listeners;

namespace Hangfire.Raven.Tests
{
    public class TestRepository : RavenTestDriver, IRepository
    {
        private readonly IDocumentStore _documentStore;

        public TestRepository()
        {
            _documentStore = GetDocumentStore(null,"hangfire-raven-tests");

            //_documentStore.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord("hangfire-raven-tests")));
            _documentStore.Maintenance.Send(new ConfigureExpirationOperation(new ExpirationConfiguration
            {
                Disabled = false,
                DeleteFrequencyInSec = 3
            }));

            //_documentStore.Listeners.RegisterListener(new NoStaleQueriesListener());
            //_documentStore.Listeners.RegisterListener(new TakeNewestConflictResolutionListener());
            //_documentStore.Initialize();

            //new RavenDocumentsByEntityName().Execute(_documentStore.DatabaseCommands, _documentStore.Conventions);
        }

        public void Create()
        {
        }

        public void Destroy()
        {
        }

        //public void Dispose()
        //{
        //    _documentStore.Dispose();
        //}

        //public IDisposable DocumentChange(Type documentType, Action<DocumentChangeNotification> action)
        //{
        //    return _documentStore.Changes().ForDocumentsStartingWith(GetId(documentType, ""))
        //        .Subscribe(new RepositoryObserver<DocumentChangeNotification>(action));
        //}

        //public IDisposable DocumentChange(Type documentType, string suffix, Action<DocumentChangeNotification> action)
        //{
        //    return _documentStore.Changes().ForDocumentsStartingWith(GetId(documentType, string.Format("{0}/", suffix)))
        //        .Subscribe(new RepositoryObserver<DocumentChangeNotification>(action));
        //}

        public void ExecuteIndexes(List<AbstractIndexCreationTask> indexes)
        {
            _documentStore.ExecuteIndexes(indexes);
        }

        //public FacetResults GetFacets(string index, IndexQuery query, List<Facet> facets)
        //{
        //    return _documentStore.DatabaseCommands.GetFacets(index, query, facets);
        //}

        public string GetId(Type type, params string[] id)
        {
            //return _documentStore.Conventions.FindFullDocumentKeyFromNonStringIdentifier(string.Join("/", id), type, false);
            return type.ToString() + '/' + string.Join("/", id);
        }
        public IDocumentSession OpenSession()
        {
            return _documentStore.OpenSession();
        }
    }
}
