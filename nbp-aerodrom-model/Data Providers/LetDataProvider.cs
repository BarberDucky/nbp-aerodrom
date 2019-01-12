using nbp_aerodrom_model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_aerodrom_model.Data_Providers
{
    public class LetDataProvider
    {
        //Fali povezivanje sa prevoznikom
        public static String CreateLet(CreateLetDTO letDTO)
        {
            try
            {
                Let newLet = letDTO.FromDTO();
                newLet.Id = Guid.NewGuid().ToString();

                var query = DataLayer.Client.Cypher
                    .Create("(let : Let {newLet})")
                    .WithParam("newLet", newLet)
                    .With("let")
                    .Match("(polazni: Aerodrom)", "(dolazni:Aerodrom)")
                    .Where((Aerodrom polazni) => polazni.Id == letDTO.PolazniAeroID)
                    .AndWhere((Aerodrom dolazni) => dolazni.Id == letDTO.DolazniAeroID)
                    .Create("(let) - [: POLAZI] -> (polazni)")
                    .Create("(let) - [: DOLAZI] -> (dolazni)")
                    .Return<Let>("let")
                    .Results;

                if (query != null)
                {
                    return ((Let)query).Id;
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public static bool UpdateLet(UpdateLetDTO letDTO)
        {
            try
            {
                Dictionary<string, object> queryDict = new Dictionary<string, object>
                {
                    { "vremePolaska", letDTO.VremePolaska },
                    { "vremeDolaska", letDTO.VremeDolaska },
                    { "brojMesta", letDTO.BrojMesta }
                };

                var query = DataLayer.Client.Cypher
                    .Match("(let: Let)")
                    .Where((Let let) => let.Id == letDTO.Id)
                    .Set("let.VremePolaska = {vremePolaska}")
                    .Set("let.VremeDolaska = {vremeDolaska}")
                    .Set("let.BrojMesta = {brojMesta}")
                    .WithParams(queryDict)
                    .Return<Let>("let")
                    .Results;
                if (query != null)
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

        public static bool Delete(String Id)
        {
            try
            {
                var query = DataLayer.Client.Cypher
                    .OptionalMatch("(let:Let)<-[r]-()")
                    .Where((Let let) => let.Id == Id)
                    .Delete("r, let");

                query.ExecuteWithoutResults();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}
