namespace Api.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class SessionResponse
    {
        public PolicyDetailsApi policy_details_api { get; set; }

        public string GetReviewStatus()
        {
            if (policy_details_api == null
                || policy_details_api.policy_detail_api == null
                || !policy_details_api.policy_detail_api.Any()
                || policy_details_api.policy_detail_api[0].customer == null)
            {
                return null;
            }

            return policy_details_api.policy_detail_api[0].customer.review_status;
        }

        public string GetRiskRating()
        {
            if (policy_details_api?.policy_detail_api == null 
                || !policy_details_api.policy_detail_api.Any() 
                || policy_details_api.policy_detail_api[0].customer == null)
            {
                return null;
            }

            return policy_details_api.policy_detail_api[0].customer.risk_rating;
        }

        public string GetReasonCode()
        {
            if (policy_details_api?.policy_detail_api == null 
                || !policy_details_api.policy_detail_api.Any() 
                || policy_details_api.policy_detail_api[0].customer?.rules == null 
                || !policy_details_api.policy_detail_api[0].customer.rules.Any())
            {
                return null;
            }

            return policy_details_api.policy_detail_api[0].customer.rules[0].reason_code;
        }
    }

    public class PolicyDetailsApi
    {
        public List<PolicyDetails> policy_detail_api { get; set; }
    }

    public class PolicyDetails
    {
        public Customer customer { get; set; }
    }

    public class Customer
    {
        public string review_status { get; set; }

        public string risk_rating { get; set; }

        public List<CustomerRules> rules { get; set; }
    }

    public class CustomerRules
    {
        public string reason_code { get; set; }
    }
}