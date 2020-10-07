using System.Collections.Generic;

namespace ExchangeRateService.Models
{
	public class HistoricalRatesResponseModel
	{
		public IDictionary<string, IDictionary<CurrencyType, decimal>> Rates { get; set; }
	}
}
