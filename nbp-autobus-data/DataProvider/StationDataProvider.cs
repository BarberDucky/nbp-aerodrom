using nbp_autobus_data.DTOs;
using nbp_autobus_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.DataProvider
{
    public class StationDataProvider
    {
        #region Private
        private static bool ExistsStation(string stationName)
        {
            try
            {
                var query = DataLayer.Client.Cypher
                    .Match(("(s: Station)"))
                    .Where((Station s) => s.Name == stationName)
                    .Return<Station>("s")
                    .Results;

                if (query != null && query.Count<Station>() > 0)
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

        public static StationDTO InsertStation(StationDTO dto)
        {
            try
            {
                //TODO pogledati da li moze sa merge
                if (ExistsStation(dto.Name))
                    return null;

                Station s = StationDTO.FromDTO(dto);
                s.Id = Guid.NewGuid().ToString();

                var query = DataLayer.Client.Cypher
                    .Create("(station: Station {s})")
                    .WithParam("s", s)
                    .Return<Station>("station")
                    .Results;

                if (query != null && query.Count<Station>() > 0)
                {
                    return new StationDTO(query.ToList()[0]);
                }

                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static IEnumerable<StationDTO> GetAllStations()
        {
            try
            {
                var query = DataLayer.Client.Cypher
                    .Match("(s:Station)")
                    .Return<Station>("s")
                    .Results;

                if (query != null && query.Count<Station>() > 0)
                {
                    return StationDTO.FromEnitityList(query);
                }

                return new List<StationDTO>();
            }
            catch (Exception e)
            {
                return new List<StationDTO>();
            }
        }

        public static StationDTO Get(string id)
        {
            try
            {
                var query = DataLayer.Client.Cypher
                    .Match("(s:Station)")
                    .Where((Station s) => s.Id == id)
                    .Return<Station>("s")
                    .Results;

                if (query != null && query.Count<Station>() > 0)
                {
                    var s = query.ToList()[0];
                    return new StationDTO(s);
                }

                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
