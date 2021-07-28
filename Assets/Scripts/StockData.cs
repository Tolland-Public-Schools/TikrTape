/* TikrTape
 * Copyright (C) 2021 Tolland Public Schools
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using MiniJSON;
using System;
using System.Linq;
using TMPro;

public class StockData : MonoBehaviour
{
    //editable list of what stocks should appear
    public List<string> stockSymbolList; 
    //list of stock items
    public List<StockItem> stockList = new List<StockItem>(); 
	public GameObject popUpText;
	public GameObject popUp;
	private bool noErrors = true;

	//for every stock in StockSymbolList make an http request and assign the data to the a new StockItem in stockList and determine if the price increased or decreased
	public void GetStocks()
	{
		//get stock symbol list from user prefs
		SyncStockPrefs();

		//make sure there's no old data in stockList
		stockList.Clear();

		//make http request for each stock and assign the data to a new StockItem in stockList 
		foreach (string stockSymbol in stockSymbolList)
		{
			//define a variables for later
			string responseFromServer;
			float openingValue;
			float marketValue;

			try
			{
				//request data from yahoo finance
				WebRequest request = WebRequest.Create("https://query1.finance.yahoo.com/v8/finance/chart/?symbol=" + stockSymbol + "&range=1d&interval=1h");
				request.ContentType = "application/json; charset=utf-8";
				//save response object to response variable
				WebResponse response = request.GetResponse();
				//read data
				using (Stream dataStream = response.GetResponseStream())
				{
					// open the stream using a StreamReader for easy access
					StreamReader reader = new StreamReader(dataStream);
					// read the content
					responseFromServer = reader.ReadToEnd();
				}

				// deserialize json data
				var stockData = Json.Deserialize(responseFromServer) as Dictionary<string, object>;
				// go down each level of data structure
				Dictionary<string, object> chart = (Dictionary<string, object>)stockData["chart"];
				List<object> resultList = (List<object>)chart["result"];
				Dictionary<string, object> resultDictionary = (Dictionary<string, object>)resultList[0];
				Dictionary<string, object> indicators = (Dictionary<string, object>)resultDictionary["indicators"];
				List<object> quoteList = (List<object>)indicators["quote"];
				Dictionary<string, object> quoteDictionary = (Dictionary<string, object>)quoteList[0];

				//pull opening and market values
				try //usually use data from open and close lists
				{
					List<object> close = (List<object>)quoteDictionary["close"];
					List<object> open = (List<object>)quoteDictionary["open"];

					// openingValue = the first value in open
					openingValue = (float)(double)open[0];
					// marketValue = the most recent value in close
					marketValue = (float)(double)close[close.Count - 1];
				}
				catch //if between market opening and first sale (close/open lists have not been populated yet) use 'previous close' and 'regular market price' variables from meta
				{
					Dictionary<string, object> meta = (Dictionary<string, object>)resultDictionary["meta"];

					// openingValue = previous close 
					openingValue = (float)(double)meta["previousClose"];
					// marketValue = regular market price
					marketValue = (float)(double)meta["regularMarketPrice"];
				}

				//!Debug.Log("Stock price for "+stockSymbol+" set to: "+ marketValue.ToString());

				//shorten marketValue
				marketValue = (float)Math.Round(marketValue, 1);

				//assign data to new StockItem
				stockList.Add(new StockItem { symbol = stockSymbol, opening = openingValue, marketValue = marketValue });

				//close connection
				response.Close();

				CheckIncrease();
			}
			catch
			{
				//have a little box that pops up with error message for user
				string errorMsg = "Error with stock symbol '" + stockSymbol + "'. Please make sure this symbol is correct or check for extraneous commas.";
				popUpText.GetComponent<TMP_Text>().text = errorMsg;
				noErrors = false;
				popUp.SetActive(true);
			}
		}

		//after going through all the stocks if there are no errors don't show the pop up box
		if(noErrors == true)
		{
			popUp.SetActive(false);
		}
	}

	void SyncStockPrefs()
	{
		List<string> rawInputList = new List<string>();
		//Format stock symbol input; take the string and break into a list of strings separated by commas
        rawInputList = PlayerPrefs.GetString("rawSymbols").Split(',').ToList();

		//remove spaces and capitilize everything
		foreach (string item in rawInputList)
		{
			if(item != "") //only process if item is not blank
			{
				string trimmed = item.Trim();
				string stockSymbol = trimmed.ToUpper();
				//add to final stock symbol list
				stockSymbolList.Add(stockSymbol);
			}
		}
	}

	//check to see if the stock value has increased or decreased and set bool for StockItem accordingly
	private void CheckIncrease()
	{
		foreach (StockItem stock in stockList)
		{
            if (stock.marketValue > stock.opening)
			{
				stock.valueIncrease = true;
			}
			else
			{
				stock.valueIncrease = false;
			}
		}
	}
}
