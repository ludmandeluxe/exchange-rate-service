using System;

namespace ExchangeRateService.Models
{
	public class ExchangeRateModel
	{
		/// <summary>
		/// Value of the exchange rate
		/// </summary>
		public decimal Value { get; set; }

		/// <summary>
		/// Date of the exchange rate
		/// </summary>
		public DateTime Date { get; set; }
	}
}
