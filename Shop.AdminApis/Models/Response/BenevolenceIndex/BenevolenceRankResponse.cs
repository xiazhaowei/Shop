using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.BenevolenceIndex
{
    public class BenevolenceRankResponse:BaseApiResponse
    {
        public IList<WalletAlis> WalletAlises { get; set; }
    }

    public class WalletAlis
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string NickName { get; set; }
        public string Mobile { get; set; }
        public string Portrait { get; set; }

        public decimal Cash { get; set; }
        public decimal Benevolence { get; set; }
        public decimal Earnings { get; set; }

    }
}