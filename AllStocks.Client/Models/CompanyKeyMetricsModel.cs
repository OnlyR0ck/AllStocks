using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace AllStocks.Client.Models
{
    public class CompanyKeyMetricsModel 
    {
        public CompanyKeyMetrics[] Metrics { get; set; }

        public class CompanyKeyMetrics
        {
            public long marketCapTTM { get; set; }
            public long enterpriseValueTTM { get; set; }
            public float peRatioTTM { get; set; }
            public float priceToSalesRatioTTM { get; set; }
            public float ptbRatioTTM { get; set; } //price to book
            public float evToSalesTTM { get; set; } //ev-to-rev
            public float enterpriseValueOverEBITDATTM { get; set; } //ev-to-ebitda
            public float currentRatioTTM { get; set; }
            public float roicTTM { get; set; }
            public float roeTTM { get; set; }
        }

    }
}
