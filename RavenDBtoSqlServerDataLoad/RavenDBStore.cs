using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations;
using System.Security.Cryptography.X509Certificates;

namespace RavenDBtoSqlServerDataLoad
{
    public class RavenDBStore
    {
        private static DocumentStore store;
        
        public static IDocumentStore InitializeStore()
        {
            string thumbprint = "9213615dfd973e4171d98c1a17b2d4d84d316758";
            X509Certificate2 certificate = GetCertificateFromStore(thumbprint);
            store = new DocumentStore()
            {

                Urls = new[] { "https://a.ravendbtutorial.ravendb.community/" },
                Database = "DemoDB",
                Certificate = certificate
            };
            store.Initialize(); 

            return store;
        }


        private static X509Certificate2 GetCertificateFromStore(string thumbprint)
        {
            X509Certificate2 cert = null;
            using (var store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                store.Open(OpenFlags.ReadOnly);
                var certs = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
                if (certs.Count > 0)
                {
                    cert = certs[0];
                }
            }

            if (cert == null)
            {
                throw new Exception($"Certificate with thumbprint {thumbprint} not found.");
            }

            return cert;
        }

    }
}
