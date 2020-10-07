using System;
using System.Collections.Generic;

namespace ExchangeRateService.Models
{
	public static class ModelFactory
	{
		public static ExchangeRateModel Create(KeyValuePair<string, decimal> exchangeRateKeyValuePair)
			=> new ExchangeRateModel
			{
				Date = DateTime.Parse(exchangeRateKeyValuePair.Key),
				Value = exchangeRateKeyValuePair.Value
			};
	}
}
