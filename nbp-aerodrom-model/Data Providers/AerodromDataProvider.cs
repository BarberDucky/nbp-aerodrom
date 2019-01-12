using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_aerodrom_model.Data_Providers
{
    public class AerodromDataProvider
    {
        public static String CreateAerodrom( String city, String country, String name)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>
            {
                { "Id", Guid.NewGuid().ToString()},
                { "Grad", city },
                { "Drzava", country },
                { "Naziv", name }
            };

            
            var query = new CypherQuery("CREATE (n:Aerodrom { Id:{Id}, Grad:{Grad}, Drzava:{Drzava}," +
                                                        " Naziv:{Naziv} }) return n",
                                                          queryDict, CypherResultMode.Set);
            try
            {
                List<Aerodrom> aerodrom = ((IRawGraphClient)DataLayer.Client).ExecuteGetCypherResults<Aerodrom>(query).ToList();
                return aerodrom[0].Id;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static bool UpdateAerodrom(String id, String city, String country, String name)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>
            {
                { "Id", id },
                { "Grad", city },
                { "Drzava", country },
                { "Naziv", name }
            };

            var query = new CypherQuery("start n = node(*) where(n: Aerodrom) and n.Id = {Id} " +
                "set n.Grad = {Grad}, n.Drzava = {Drzava}, n.Naziv = {Naziv} " +
                "return n", queryDict, CypherResultMode.Set);

            try
            {
                List<Aerodrom> aerodrom = ((IRawGraphClient)DataLayer.Client).ExecuteGetCypherResults<Aerodrom>(query).ToList();
                if (aerodrom.Count > 0)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool RemoveAerodrom(String Id)
        {
            var query = DataLayer.Client
             .Cypher
             .Start(new { all = All.Nodes })
             .Match("(n:Aerodrom)")
             .Where((Aerodrom n) => n.Id == Id)
             .Delete("n");

            try
            {
                query.ExecuteWithoutResults();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }       
        }

        public static Aerodrom GetAerodrom(String id)
        {
            return DataLayer.Client
                .Cypher
                .Match("(a:Aerodrom)")
                .Where((Aerodrom a) => a.Id == id)
                .Return<Aerodrom>("a")
                .Results.ToList()[0];
        }
    }
}
