# exchange-rate-service startup steps

1. Open ExchangeRateService.sln in Visual studio
2. Make sure that ExchangeRateService.Api is set as startup project
3. Rebuild the solution in **Debug** solution configuration
4. Run project in **Debug** solution configuration via IISExpress (it will run on https://localhost:44354/) or via console (it will run on https://localhost:5001/)
5. Necessary configuration settings are already set up in "ExchangeRateService.Api\appsettings.Development.json". For production (Release) deploy, appsettings.json will be used
6. Launch this sample GET request for results (via Postman or browser):\
   https://localhost:44354/exchangeRates/historicalExchangeRates?baseCurrency=JPY&targetCurrency=ILS&dates=2018-01-31,2018-02-01,2018-02-02,2018-02-03,2018-02-04
7. Expected result is in the following format:
   {\
    "minExchangeRate": {\
        "value": 0.0312614779,\
        "date": "2018-02-02T00:00:00"\
    },\
    "maxExchangeRate": {\
        "value": 0.0312853982,\
        "date": "2018-01-31T00:00:00"\
    },\
    "averageExchangeRate": 0.0312708101,\
    "baseCurrency": "JPY",\
    "targetCurrency": "ILS"\
}
