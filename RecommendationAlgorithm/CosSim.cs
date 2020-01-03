using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BE;
using BE.Models;

namespace RecommendationAlgorithm
{
    public static class CosSim
    {
        public static IEnumerable<Output> Calculate(int issuerId,
            IEnumerable<UserInterests> issuerInterests,
            IEnumerable<IEnumerable<UserInterests>> otherUserInterests)
        {
            var outputs = new List<Output>();
            foreach (var value in otherUserInterests)
            {
                var output = new Output
                {
                    IssuerId = issuerId, 
                    UserId = value.ElementAt(0).UserId,
                    SimilarityScore = Calculate(issuerInterests, value)
                };
                outputs.Add(output);
            }
            return outputs.OrderByDescending(e => e.SimilarityScore);
        }

        private static double Calculate(IEnumerable<UserInterests> issuerInterests,
            IEnumerable<UserInterests> otherUserInterests)
        {
            var wages = new List<List<int>>();
            foreach (var issuerInterest in issuerInterests)
            {
                var otherUserInterest = otherUserInterests.SingleOrDefault(e =>
                    e.Id == issuerInterest.Id);
                int otherUserInterestWage = otherUserInterest != null
                    ? otherUserInterest.Wage
                    : 0;
                int issuerInterestWage = issuerInterest.Wage;
                wages.Add(new List<int>{ issuerInterestWage, otherUserInterestWage });
            }
            //zakładając, że wzór wygląda następująco A/sqrt(A1)*sqrt(A2)
            double A = wages.Sum(e => e[0] * e[1]);
            double A1 = Math.Sqrt(wages.Sum(e => Math.Pow(e[0], 2)));
            double A2 = Math.Sqrt(wages.Sum(e => Math.Pow(e[1], 2)));
            double similarity = A / A1 * A2;
            return similarity;
        }
    }
}