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
using UnityEngine.SceneManagement;
using System;

public class Scroller : MonoBehaviour{

	public int refreshFreq;
	public DateTime lastRefresh;
    public bool isStock;
    public GameObject prefab;
    private Vector3 objPos;
    public float scrollSpeed;
    public bool ready = false;
	public string currentTime;

    public List<GameObject> GameObjList;
	public List<string> times = new List<string>();
	private List<string> afterTimes = new List<string>();
	private float elapsed;

	//for looping
	private GameObject previousObj;
	public GameObject mainCameraObj;
    private Camera mainCamera;
    private Vector2 screenBounds;
    public readonly object _listLock = new object();
    public float overFlow; // distance object continues scrolling before removed
    public float topBuffer; // distance from top of screen
    public float spacerSize; // space between objects

	void Start()
	{
		// for first run set default stocks
		if(PlayerPrefs.GetString("rawSymbols") == null)
		{
			// set as default
			PlayerPrefs.SetString("rawSymbols", "AAPL, GOOGL, MSFT, AMZN");
		}

		// hide cursor
		Cursor.visible = false;
		
		// establish screen boundaries
		ScreenBoundries();

		// if script is attached to the News Controller, instantiate the News GameObjs
		if(isStock == false) 
		{
			SetNewsObjs();
		}
		// if script is attached to the Stock Controller, instantiate the Stock GameObjs
		else if (isStock == true)
		{
			SetStockObjs();
		}

		// ready is true once all objects are instantiated so everything starts moving at the same time
		ready = true;
	}

	void Update()
	{
		// loop gameObjs on screen
		Loop();
	}

	void SetStockObjs()
	{
		// create instance of StockData Class
		var StockData = GetComponent<StockData>();
		// Pull stock data from api
		StockData.GetStocks();

		foreach (StockItem stock in StockData.stockList)
		{
			// determine position
			if (stock == StockData.stockList[0]) // instantiate first object just before top right corner
			{
				objPos = new Vector3((screenBounds.x + 4), (screenBounds.y - topBuffer), 0);
			}
			else // if not the first, space out from the previous
			{
				objPos = new Vector3((objPos.x + spacerSize), objPos.y, 0);
			}

			GameObject StockObj = Instantiate(prefab, objPos, Quaternion.identity);

			// set symbol displayed on gameObj
			StockObj.GetComponent<StockProperties>().stockSymbol = stock.symbol;
			// set price displayed on gameObj
			StockObj.GetComponent<StockProperties>().stockPrice = stock.marketValue;
			// set up/down arrow
			StockObj.GetComponent<StockProperties>().stockIncrease = stock.valueIncrease;

			// add to list of stock objects
			GameObjList.Add(StockObj);

			StockObj.GetComponent<StockProperties>().scrollSpeed = scrollSpeed;
			StockObj.GetComponent<StockProperties>().scroller = this;
		}
	}

	void SetNewsObjs()
	{
		// create instance of NewsData Class
		var NewsData = new NewsData();

		// Pull news from rss feed
		NewsData.GetNews();

		// for every article in the article list, instantiate a game object
		foreach (NewsItem article in NewsData.newsList)
		{
			// determine position (objPos) based on index in list
			// if it's the first list item instantiate it just before the top right corner
			if (article == NewsData.newsList[0])
			{
				objPos = new Vector3((screenBounds.x + 4), (screenBounds.y - topBuffer), 0);
			}
			else // if it's not the first list item, space out from the previous
			{
				objPos = new Vector3((objPos.x + spacerSize), objPos.y, 0);
			}

			// instantiate game object
			GameObject NewsObj = Instantiate(prefab, objPos, Quaternion.identity);

			// set the title based on article list
			NewsObj.GetComponent<NewsProperties>().headlineText.text = article.Title;
			// set the description based on article list
			NewsObj.GetComponent<NewsProperties>().descriptionText.text = article.Description;
			// set the source based on article list
			NewsObj.GetComponent<NewsProperties>().newsSource = article.Source;

			// add to list of game objects
			GameObjList.Add(NewsObj);

			// set speed
			NewsObj.GetComponent<NewsProperties>().scrollSpeed = scrollSpeed;
			NewsObj.GetComponent<NewsProperties>().scroller = this;
		}
	}

	void ScreenBoundries()
	{
		mainCamera = mainCameraObj.GetComponent<Camera>();
		screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
	}

	void Loop() // off-screen looping: after last object passes screen bounds it moves back to beginning
	{
		lock (_listLock)
		{
			foreach (GameObject obj in GameObjList) //for each object in the list
			{
				if (obj.transform.position.x < (-screenBounds.x - overFlow)) //if it passes the screen bounds
				{
					// find last object in list
					int listItems = GameObjList.Count;
					GameObject lastItem = GameObjList[listItems - 1];

					// if it's the first list item put it after the last
					if (obj == GameObjList[0])
					{
						obj.transform.position = new Vector3((lastItem.transform.position.x + spacerSize), obj.transform.position.y, 0);
						previousObj = obj; // once moved set as previous object for reference for next obj
					}
					else // otherwise it should be before the list element numerically before it
					{
						obj.transform.position = new Vector3((previousObj.transform.position.x + spacerSize), obj.transform.position.y, 0);
						previousObj = obj; // once moved set as previous object for reference for next obj
					}
				}

				// if it's a news object, hide the news source image until it's in screen bounds
				if (isStock == false)
				{	
					SpriteRenderer spriteRenderer = obj.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
					// if the object is in screen bounds show the image
					if(obj.transform.position.x < (screenBounds.x + overFlow))
					{
						spriteRenderer.enabled = true;
					}
					else
					{
						spriteRenderer.enabled = false;
					}
				}
			}
		}
	}
	// Joke:
	// What is an astronaut's favorite key?
	// The space bar!
	// :)
}