using nbp_autobus_data.BusinessModel;
using nbp_autobus_data.DTOs;
using nbp_autobus_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.DataProvider
{
    public class ReviewDataProvider
    {
        public static string InsertReview(CreateReviewDTO dto)
        {
            try
            {
                Review newReview = CreateReviewDTO.FromDTO(dto);
                newReview.Id = Guid.NewGuid().ToString();

                var query = DataLayer.Client.Cypher
                    .Create("(review : Review {newReview})")
                    .WithParam("newReview", newReview)
                    .With("review")
                    .Match("(user: User)", "(carrier: Carrier)")
                    .Where((User user) => user.Id == dto.UserId)
                    .AndWhere((Carrier carrier) => carrier.Id == dto.CarrierId)
                    .Create("(review) - [: REVIEWED_BY] -> (user)")
                    .Create("(review) - [: REVIEW_FOR] -> (carrier)")
                    .Return<Review>("review")
                    .Results;

                if (query != null && query.Count() > 0)
                {
                    return query.ToList()[0].Id;
                }
                return null;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ReadReviewDTO GetReview(string id)
        {
            try
            {
                var query = DataLayer.Client.Cypher
                    .Match("(review :Review) - [: REVIEWED_BY] -> (user:User)",
                    "(review: Review) - [: REVIEW_FOR] -> (carrier:Carrier)")
                    .Where((Review review) => review.Id == id)
                    .Return((review, carrier, user) => new BusinessReview
                    {
                        Review = review.As<Review>(),
                        Carrier = carrier.As<Carrier>(),
                        User = user.As<User>()

                    }).Results;

                if(query!=null && query.Count() > 0)
                {
                    return new ReadReviewDTO(query.ToList()[0]);
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static IEnumerable<ReadReviewDTO> GetReviewsByCarrier(string carrierId)
        {
            try
            {
                var query = DataLayer.Client.Cypher
                    .Match("(review :Review) - [: REVIEWED_BY] -> (user:User)",
                    "(review: Review) - [: REVIEW_FOR] -> (carrier:Carrier)")
                    .Where((Carrier carrier)=>carrier.Id == carrierId)
                    .Return((review, carrier, user) => new BusinessReview
                    {
                        Review = review.As<Review>(),
                        Carrier = carrier.As<Carrier>(),
                        User = user.As<User>()

                    }).Results;

                return ReadReviewDTO.FromEntityList(query);

            }
            catch(Exception e)
            {
                return new List<ReadReviewDTO>();
            }
        }

        public static IEnumerable<ReadReviewDTO> GetReviewsByUser(string userId)
        {
            try
            {
                var query = DataLayer.Client.Cypher
                    .Match("(review :Review) - [: REVIEWED_BY] -> (user:User)",
                    "(review: Review) - [: REVIEW_FOR] -> (carrier:Carrier)")
                    .Where((User user) => user.Id == userId)
                    .Return((review, carrier, user) => new BusinessReview
                    {
                        Review = review.As<Review>(),
                        Carrier = carrier.As<Carrier>(),
                        User = user.As<User>()

                    }).Results;

                return ReadReviewDTO.FromEntityList(query);

            }
            catch (Exception e)
            {
                return new List<ReadReviewDTO>();
            }
        }
    }
}
