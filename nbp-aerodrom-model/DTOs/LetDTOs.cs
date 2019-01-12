using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_aerodrom_model.DTOs
{
    public class CreateLetDTO
    {
        public int BrojMesta { get; set; }
        public String PrevoznikID { get; set; }
        public String PolazniAeroID { get; set; }
        public String DolazniAeroID { get; set; }
        public DateTime VremePolaska { get; set; }
        public DateTime VremeDolaska { get; set; }

        public Let FromDTO()
        {
            return new Let
            {
                BrojMesta = BrojMesta,
                VremePolaska = VremePolaska,
                VremeDolaska = VremeDolaska
            };
        }
    }

    public class UpdateLetDTO
    {
        public String Id { get; set; }
        public int BrojMesta { get; set; }
        public DateTime VremePolaska { get; set; }
        public DateTime VremeDolaska { get; set; }

    }
        
}
