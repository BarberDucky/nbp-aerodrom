using nbp_autobus_data.BusinessModel;
using nbp_autobus_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.DTOs
{
    public class CarrierDTO
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Website { get; set; }
        public String PhoneNumber { get; set; }
        public String UserId { get; set; }

        public CarrierDTO()
        {

        }
        public CarrierDTO(Carrier c)
        {
            this.Id = c.Id;
            this.Name = c.Name;
            this.PhoneNumber = c.PhoneNumber;
            this.Website = c.Website;

        }

        public CarrierDTO(BusinessCarrier c)
            : this(c.Carrier)
        {
            UserId = c.UserId;

        }

        public static Carrier FromDTO(CarrierDTO dto)
        {
            return new Carrier()
            {
                Id = dto.Id,
                Name = dto.Name,
                Website = dto.Website,
                PhoneNumber = dto.PhoneNumber
            };
        }

        public static IEnumerable<CarrierDTO> FromEntityList(IEnumerable<BusinessCarrier> c)
        {
            List<CarrierDTO> list = new List<CarrierDTO>();
            if (c != null && c.Count() > 0)
            {
                foreach (var el in c)
                {
                    list.Add(new CarrierDTO(el));
                }
            }

            return list;
        }

        public static IEnumerable<CarrierDTO> FromEntityList(IEnumerable<Carrier> c, string userId)
        {
            List<CarrierDTO> list = new List<CarrierDTO>();
            if (c != null && c.Count() > 0)
            {
                foreach (var el in c)
                {
                    list.Add(new CarrierDTO(el)
                    {
                        UserId = userId
                    });
                }
            }

            return list;
        }
    }
}
