using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nbp_autobus_data.BusinessModel;
using nbp_autobus_data.Model;

namespace nbp_autobus_data.DTOs
{
    public class CreateReviewDTO
    {
        public String Text { get; set; }
        public int Grade { get; set; }
        public String UserId { get; set; }
        public String CarrierId { get; set; }

        public static Review FromDTO(CreateReviewDTO dto)
        {
            return new Review()
            {
                Text = dto.Text,
                Grade = dto.Grade
            };
        }
    }

    public class ReadReviewDTO
    {
        public ReadReviewDTO(BusinessReview businessReview)
        {
            this.Id = businessReview.Review.Id;
            Text = businessReview.Review.Text;
            Grade = businessReview.Review.Grade;
            TimeStamp = businessReview.Review.TimeStamp;
            UserId = businessReview.User.Id;
            UserEmail = businessReview.User.Email;
            CarrierId = businessReview.Carrier.Id;
        }

        public String Id { get; set; }
        public String Text { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Grade { get; set; }
        public String UserId { get; set; }
        public String UserEmail { get; set; }
        public String CarrierId { get; set; }

        public static IEnumerable<ReadReviewDTO> FromEntityList(IEnumerable<BusinessReview> c)
        {
            List<ReadReviewDTO> list = new List<ReadReviewDTO>();
            if (c != null && c.Count() > 0)
            {
                foreach (var el in c)
                {
                    list.Add(new ReadReviewDTO(el));
                }
            }

            return list;
        }
    }
}
