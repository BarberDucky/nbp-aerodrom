using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nbp_autobus_data.Model;

namespace nbp_autobus_data.DTOs
{
    public class StationDTO
    {
        public String Id { get; set; }
        public String City { get; set; }
        public String Country { get; set; }
        public String Name { get; set; }

        public StationDTO()
        {

        }

        public StationDTO(Station station)
        {
            this.Id = station.Id;
            City = station.City;
            Country = station.Country;
            Name = station.Name;
        }

        public static Station FromDTO(StationDTO station)
        {
            return new Station()
            {
                Id = station.Id,
                City = station.City,
                Country = station.Country,
                Name = station.Name,
            };
        }

        public static IEnumerable<StationDTO> FromEnitityList(IEnumerable<Station> c)
        {
            List<StationDTO> list = new List<StationDTO>();
            if (c != null && c.Count<Station>() > 0)
            {
                foreach (var el in c)
                {
                    var dto = new StationDTO(el);
                    list.Add(dto);
                }
            }

            return list;
        }
    }
}
