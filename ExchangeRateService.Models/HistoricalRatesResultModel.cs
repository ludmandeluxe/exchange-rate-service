namespace ExchangeRateService.Models
{
	public class HistoricalRatesResultModel
	{
		public ExchangeRateModel MinExchangeRate { get; set; }

		public ExchangeRateModel MaxExchangeRate { get; set; }

		public decimal AverageExchangeRate { get; set; }

		public CurrencyType BaseCurrency { get; set; }

		public CurrencyType TargetCurrency { get; set; }
	}
}
