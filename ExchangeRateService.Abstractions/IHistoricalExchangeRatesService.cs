using ExchangeRateService.Models;
using System.Threading.Tasks;

namespace ExchangeRateService.Abstractions
{
	public interface IHistoricalExchangeRatesService
	{
		/// <summary>
		/// Gets historical exchange rates from the external API and formats response as HistoricalRatesResultModel
		/// </summary>
		/// <param name="baseCurrency">Base currency</param>
		/// <param name="targetCurrency">Target currency</param>
		/// <param name="dates">Comma separated string of dates for which exchange rate history is gathered</param>
		/// <returns>HistoricalRatesResultModel with historic details of the requested currencies and dates</returns>
		Task<HistoricalRatesResultModel> GetHistoricalExchangeRates(CurrencyType baseCurrency, CurrencyType targetCurrency, string dates);
	}
}
