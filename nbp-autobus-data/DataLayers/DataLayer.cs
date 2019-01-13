using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data
{
    public class DataLayer
    {
        private static GraphClient client = null;

        public static bool Test()
        {

            GraphClient c = Client;

            return true;
        }

        public static GraphClient Client
        {
            get
            {
                if (client == null)
                    return Connect();
                return client;
            }
        }

        private static GraphClient Connect()
        {
            try
            {
                client = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "milica");
                client.Connect();
                return client;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return null;
            }
        }
    }
}
