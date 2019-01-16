using nbp_autobus_data.DTOs;
using nbp_autobus_data.Model;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.DataProvider
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

        public static UserDTO GetUser(string userId)
        {
            try
            {
                var query = DataLayer.Client.Cypher
                    .Match("(user: User)")
                    .Where((User user) => user.Id == userId)
                    .Return<User>("user")
                    .Results;
                List<User> list;
                if (query != null)
                {
                    list = query.ToList();
                    if(list.Count > 0)
                    {
                        return new UserDTO(list[0]);
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ReadUserDTO LogInUser(string email, string password)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>
            {
                { "Email", email },
                { "Password", password},
            };

            var query = new CypherQuery("MATCH (n:User {Email: {Email}, Password: {Password} }) RETURN n", queryDict, CypherResultMode.Set);

            try
            {
                List<User> users = ((IRawGraphClient)DataLayer.Client).ExecuteGetCypherResults<User>(query).ToList();
                if (users.Count > 0)
                {
                    return new ReadUserDTO(users[0]);
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static String RegisterUser(UserDTO dto)
        {
            //Ako vec postoji user sa tim emailom
            if (CheckIfExists(dto.Email))
                return null;

            User user = UserDTO.FromDTO(dto);
            user.Id = Guid.NewGuid().ToString();


            Dictionary<string, object> queryDict = new Dictionary<string, object>
            {
                { "Id", user.Id },
                { "Email", user.Email },
                { "PassWord", user.Password },
                { "Ime", user.Name },
                { "Prezime", user.LastName },
                { "BrojPasosa", user.PassportNumber }
            };



            var query = new CypherQuery("CREATE (n:User { Id:{Id}, Email:{Email}, Password:{PassWord}," +
                                                         " Name:{Ime}, LastName:{Prezime}, PassportNumber:{BrojPasosa} }) return n",
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

        public static bool UpdateUser(string userId, UserDTO dto)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>
            {
                {"Id", userId },
                { "Ime", dto.Name},
                { "Prezime", dto.LastName },
                { "BrojPasosa", dto.PassportNumber }
            };
            var query = new CypherQuery("start n=node(*) where (n:User) and n.Id = {Id}" +
                "set n.Name = {Ime}, n.LastName = {Prezime}, n.PassportNumber = {BrojPasosa} " +
                "return n", queryDict, CypherResultMode.Set);
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
