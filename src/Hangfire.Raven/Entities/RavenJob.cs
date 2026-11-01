using Hangfire.Storage;
using Hangfire.Storage.Monitoring;
using System;
using System.Collections.Generic;

namespace Hangfire.Raven.Entities
{
    public class RavenJob
    {
        public RavenJob()
        {
            Parameters = new Dictionary<string, string>();
            History = new List<StateHistoryDto>();
        }

        public string Id { get; set; }
        public InvocationData InvocationData { get; set; }
        public IDictionary<string, string> Parameters { get; set; }
        public DateTime CreatedAt { get; set; }

        public StateData StateData { get; set; }
        public List<StateHistoryDto> History { get; set; }
    }
}