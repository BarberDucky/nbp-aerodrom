using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nbp_autobus_data.Model;

namespace nbp_autobus_data.DTOs
{
    public class CardDTO
    {
        public String Id { get; set; }
        public float Price { get; set; }
        public DateTime TakeOfDate { get; set; }

        public CardDTO(Card card)
        {
            Id = card.Id;
            Price = card.Price;
            TakeOfDate = card.TakeOfDate;
        }

        public static IEnumerable<CardDTO> FromEntityList(IEnumerable<Card> c)
        {
            List<CardDTO> list = new List<CardDTO>();
            if (c != null && c.Count() > 0)
            {
                foreach (var el in c)
                {
                    list.Add(new CardDTO(el));
                }
            }

            return list;
        }
    }
}
