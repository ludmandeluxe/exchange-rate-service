using ExchangeRateService.Abstractions;
using ExchangeRateService.Models;
using ExchangeRateService.Services.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateService.Services
{
	public class HistoricalExchangeRatesService : IHistoricalExchangeRatesService
	{
		private readonly ILogger<HistoricalExchangeRatesService> logger;
		private readonly IConfiguration configuration;

		private const string dateFormat = "yyyy-MM-dd";

		public HistoricalExchangeRatesService(ILogger<HistoricalExchangeRatesService> logger, IConfiguration configuration)
		{
			this.logger = logger;
			this.configuration = configuration;
		}

		private List<DateTime> ParseDates(string dates)
		{
			logger.LogTrace($"Started splitting dates: {dates}");

			var splittedDates = dates.Split(',').ToList();

			if (!splittedDates.Any())
			{
				throw new Exception($"Dates must be provided");
			}

			return splittedDates.Select(date =>
			{
				if (!DateTime.TryParse(date.Trim(), out var resultDate))
				{
					throw new Exception($"Date {date} is not valid");
				}

				return resultDate;
			}).ToList();
		}

		public async Task<HistoricalRatesResultModel> GetHistoricalExchangeRates(CurrencyType baseCurrency, CurrencyType targetCurrency, string dates)
		{
			// Parse dates param from comma separated string to list of DateTime objects
			var parsedDates = ParseDates(dates);
			// Convert back list of DateTime objects to list of Date formatted strings
			// It doesn't have big impact on performance but it's much safer for later filtering of result set
			// Because ExchangeRates API does not support custom dates, only range, so we need to filter the result set
			var stringFormattedDates = parsedDates.Select(date => date.ToString(dateFormat));

			// Prepare start and end dates in proper format for the ExchangeRates API request
			var startDate = parsedDates.Min().ToString(dateFormat);
			var endDate = parsedDates.Max().ToString(dateFormat);

			logger.LogInformation($"About to fetch history from ExchangeRates API for dates from {startDate} to {endDate} with base currency {baseCurrency} and target currency {targetCurrency}");

			// Fetch ExchangeRates API base URL from configuration
			var exchangeRatesBaseUrl = configuration.GetSection("ExchangeRatesApiBase").Value;
			var exchangeRatesHistoryUrl = $"history?start_at={startDate}&end_at={endDate}&symbols={targetCurrency}&base={baseCurrency}";

			var result = await HttpRequestLauncher.GetAsync<HistoricalRatesResponseModel>($"{exchangeRatesBaseUrl}{exchangeRatesHistoryUrl}");

			// In case that result could not be obtained, throw error
			if (result == null || result.Rates == null)
			{
				throw new Exception("Unknown error occured while fetching historic exchange rates from external API, result is null");
			}

			logger.LogTrace($"Successfully fetched {result.Rates.Count()} exchange rates");

			// Filter the result set to include only relevant dates, normalize it to simple dictionary and order by values
			var normalizedResult = result.Rates.Where(rate => stringFormattedDates.Contains(rate.Key) && rate.Value != null).ToDictionary(k => k.Key, k => k.Value.FirstOrDefault().Value).OrderBy(rate => rate.Value);

			// In case that result is empty after filtering, no matching dates could be found
			if (!normalizedResult.Any())
			{
				throw new Exception("No exchange rates data could be found for the requested dates");
			}

			logger.LogTrace($"Successully filtered {normalizedResult.Count()} exchange rates that match requested dates");

			var averageRate = normalizedResult.Average(rate => rate.Value);

			return new HistoricalRatesResultModel
			{
				// Return average result with 10 decimals, same as we get from the ExchangeRates API
				AverageExchangeRate = decimal.Round(averageRate, 10),
				MinExchangeRate = ModelFactory.Create(normalizedResult.FirstOrDefault()),
				MaxExchangeRate = ModelFactory.Create(normalizedResult.LastOrDefault()),
				BaseCurrency = baseCurrency,
				TargetCurrency = targetCurrency
			};
		}
	}
}
