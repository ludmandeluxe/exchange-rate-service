using ExchangeRateService.Abstractions;
using ExchangeRateService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExchangeRateService.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ExchangeRatesController : ControllerBase
	{
		private readonly IHistoricalExchangeRatesService historicalExchangeRatesService;

		public ExchangeRatesController(IHistoricalExchangeRatesService historicalExchangeRatesService)
		{
			this.historicalExchangeRatesService = historicalExchangeRatesService;
		}

		/// <summary>
		/// Gets historical exchange rates from the external API and formats response as HistoricalRatesResultModel
		/// </summary>
		/// <param name="baseCurrency">Base currency</param>
		/// <param name="targetCurrency">Target currency</param>
		/// <param name="dates">Comma separated string of dates for which exchange rate history is gathered</param>
		/// <returns>HistoricalRatesResultModel with historic details of the requested currencies and dates</returns>
		[HttpGet]
		[Route("historicalExchangeRates", Name = nameof(GetHistoricalExchangeRates))]
		public async Task<ActionResult<HistoricalRatesResultModel>> GetHistoricalExchangeRates(CurrencyType baseCurrency, CurrencyType targetCurrency, string dates)
		{
			return Ok(await historicalExchangeRatesService.GetHistoricalExchangeRates(baseCurrency, targetCurrency, dates));
		}
	}
}
