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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;

public class NewsData
{
	public List<ISyndicationItem> rawRssList = new List<ISyndicationItem>(); //unprocessed news list with full headlines and descriptions
	public List<NewsItem> newsList = new List<NewsItem>(); //final, processed list

	//* hardcoded rss feed links
	private string EconFeed = "https://www.economist.com/business/rss.xml";
	private string NYTFeed = "https://rss.nytimes.com/services/xml/rss/nyt/US.xml";

	public void GetNews() //overall method that calls other methods
	{
		//make sure there's no old data in newsList
		newsList.Clear();

		//get data from user prefs

		//Economist
		if (PlayerPrefs.GetString("ECON") == "true")
		{
			GetRss("Economist", EconFeed);
		}
		//New York Times
		if (PlayerPrefs.GetString("NYT") == "true")
		{
			GetRss("NewYorkTimes", NYTFeed);
		}

		//custom feeds
		if (PlayerPrefs.GetString("CNS1") != "")
		{
			GetRss("CNS1", PlayerPrefs.GetString("CNS1"));
		}
		if (PlayerPrefs.GetString("CNS2") != "")
		{
			GetRss("CNS2", PlayerPrefs.GetString("CNS2"));
		}
		if (PlayerPrefs.GetString("CNS3") != "")
		{
			GetRss("CNS3", PlayerPrefs.GetString("CNS3"));
		}
	}

	public async void GetRss(string sourceName, string sourceFeed)
	{
		try
		{
			int amtArticles = 30;
			string _FeedUri = sourceFeed;
			using (var xmlReader = XmlReader.Create(_FeedUri, new XmlReaderSettings() { Async = false }))
			{
				var feedReader = new RssFeedReader(xmlReader);
				while (await feedReader.Read())
				{
					if (amtArticles > 0)
					{
						if (feedReader.ElementType == Microsoft.SyndicationFeed.SyndicationElementType.Item)
						{
							//assign each object from the syndication feed as an article
							ISyndicationItem article = await feedReader.ReadItem();

							//get the title and description from the article
							string title = article.Title;
							string description = article.Description;

							//shorten string if needed
							title = Truncate(article.Title, 60);
							description = Truncate(article.Description, 145);

							//add the article, shortened or not, to newsList
							newsList.Add(new NewsItem { Title = title, Description = description, Source = sourceName });

							amtArticles -= 1;
						}
					}
				}
			}
		}
		catch
		{
			Debug.Log("error with" + sourceName);
		}
	}

	public static string Truncate(string source, int length)
	{
		if (source.Length > length)
		{
			//cut down str length
			source = source.Substring(0, length);
			//add '...'
			source = source + "...";
		}
		return source;
	}
}