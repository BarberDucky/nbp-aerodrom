using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_aerodrom_model.Data_Providers
{
    public class UserDataProvider
    {
        #region Private methods

        private static bool CheckIfExists(string email)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>
            {
                { "Email", email },
            };

            var query = new CypherQuery("start n=node(*) where (n:User) and n.Email = {Email} RETURN count(*)", queryDict, CypherResultMode.Set);
            try
            {
                int num = ((IRawGraphClient)DataLayer.Client).ExecuteGetCypherResults<int>(query).ToList()[0];
                if (num > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        #endregion

        public static User LogInUser(string email, string password)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>
            {
                { "Email", email },
                { "PassWord", password},
            };

            var query = new CypherQuery("MATCH (n:User {Email: {Email}, PassWord: {PassWord} }) RETURN n", queryDict, CypherResultMode.Set);

            try
            {
                List<User> users = ((IRawGraphClient)DataLayer.Client).ExecuteGetCypherResults<User>(query).ToList();
                if (users.Count > 0)
                {
                    return users[0];
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static String RegisterUser(string email, string pass,
            string name, string lastname, string passportNum)
        {
            //Ako vec postoji user sa tim emailom
            if (CheckIfExists(email))
                return null;

            User user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                PassWord = pass,
                Ime = name,
                Prezime = lastname,
                BrojPasosa = passportNum
            };

            Dictionary<string, object> queryDict = new Dictionary<string, object>
            {
                { "Id", user.Id },
                { "Email", user.Email },
                { "PassWord", user.PassWord },
                { "Ime", user.Ime },
                { "Prezime", user.Prezime },
                { "BrojPasosa", user.BrojPasosa }
            };

            //var query = new CypherQuery("CREATE (n:User {Id:'" + user.Id + "', Email:'" + user.Email
            //                                                + "', PassWord:'" + user.PassWord + "', Ime:'" + user.Ime
            //                                                + "', Prezime:'" + user.Prezime
            //                                                + "', BrojPasosa:'" + user.BrojPasosa + "'}) return n",
            //                                                queryDict, CypherResultMode.Set);

            //((IRawGraphClient)DataLayer.Client).ExecuteCypher(query);

            var query = new CypherQuery("CREATE (n:User { Id:{Id}, Email:{Email}, PassWord:{PassWord}," +
                                                         " Ime:{Ime}, Prezime:{Prezime}, BrojPasosa:{BrojPasosa} }) return n",
                                                           queryDict, CypherResultMode.Set);
            try
            {
                List<User> users = ((IRawGraphClient)DataLayer.Client).ExecuteGetCypherResults<User>(query).ToList();
                return user.Id;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public static bool ChangeInformationUser(string id, string name, string lastname, string passportNum)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>
            {
                {"Id", id },
                { "Ime", name},
                { "Prezime", lastname },
                { "BrojPasosa", passportNum }
            };
            var query = new CypherQuery("start n=node(*) where (n:User) and n.Id = {Id}" +
                "set n.Ime = {Ime}, n.Prezime = {Prezime}, n.BrojPasosa = {BrojPasosa} " +
                "return n",queryDict, CypherResultMode.Set);
            try
            {
                List<User> users = ((IRawGraphClient)DataLayer.Client).ExecuteGetCypherResults<User>(query).ToList();

                if (users.Count > 0)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
