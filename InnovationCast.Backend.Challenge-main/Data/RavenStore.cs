using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Raven.Client.Documents;

namespace Backend.Challenge.Data
{
    public class RavenStore
    {
        private readonly IDocumentStore _store;
        public IDocumentStore Store { get {return _store;} }

        public RavenStore(IConfiguration configuration)
        {
            using (IDocumentStore Store = new DocumentStore
                {
                    Urls = new[]                        // URL to the Server,
                    {                                   // or list of URLs 
                        configuration.GetSection("RavenConfiguration").GetValue<string>("DefaultConnection")  // to all Cluster Servers (Nodes)
                    },
                        Database = configuration.GetSection("RavenConfiguration").GetValue<string>("Database"),             // Default database that DocumentStore will interact with
                        Conventions = { }                   // DocumentStore customizations
                })
                {
                    _store = Store.Initialize();                 // Each DocumentStore needs to be initialized before use.
                                                        // This process establishes the connection with the Server
                                                        // and downloads various configurations
                                                        // e.g. cluster topology or client configuration
                }


        }
    }
}