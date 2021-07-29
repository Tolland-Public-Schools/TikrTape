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
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;

public class SettingsMenu : MonoBehaviour
{
    public GameObject userInput;
    public GameObject stockRefreshSlider;
    public GameObject newsRefreshSlider;
    private string userInputStr;
    private int stockRefreshFreq;
    private int newsRefreshFreq;

    public void Start()
    {
        //make sure the cursor is visable
        Cursor.visible = true;
        //make sure the user input box and slider match the state saved in user prefs
        userInput.GetComponent<TMP_InputField>().text = PlayerPrefs.GetString("rawSymbols");
        stockRefreshSlider.GetComponent<Slider>().value = (float)PlayerPrefs.GetInt("stockRefreshFreq");
        newsRefreshSlider.GetComponent<Slider>().value = (float)PlayerPrefs.GetInt("newsRefreshFreq");
    }

    public void ValidateCharacters()
    {
        //only allow characters A-Z (capital or not) and commas
        userInput.GetComponent<TMP_InputField>().text = Regex.Replace(userInput.GetComponent<TMP_InputField>().text, @"[^a-zA-Z, ]", "");
        userInput.GetComponent<TMP_InputField>().text = userInput.GetComponent<TMP_InputField>().text.ToUpper();
    }

    public void ValidateStocks() //make sure each stock symbol is valid
    {
        List<string> rawInputList = new List<string>();
        List<string> checkSymbolList = new List<string>();

		//spit the string into a list and format characters
		rawInputList = PlayerPrefs.GetString("rawSymbols").Split(',').ToList();
		foreach (string item in rawInputList)
		{
			string trimmed = item.Trim();
			string stockSymbol = trimmed.ToUpper();
			//add to final stock symbol list
			checkSymbolList.Add(stockSymbol);
		}
    }

    //button functions:
    public void Save() //commit changes to user prefs and exit
    {
        //update stock update freq value
        stockRefreshFreq = (int)stockRefreshSlider.GetComponent<Slider>().value;
        PlayerPrefs.SetInt("stockRefreshFreq", stockRefreshFreq);

        //update news update freq value
        newsRefreshFreq = (int)newsRefreshSlider.GetComponent<Slider>().value;
        PlayerPrefs.SetInt("newsRefreshFreq", newsRefreshFreq);

        //update stock symbol list in user prefs
        userInputStr = userInput.GetComponent<TMP_InputField>().text;
        PlayerPrefs.SetString("rawSymbols", userInputStr);
        
        //go back to scroller 
        SceneManager.LoadScene("Scroller");
    }

    public void Exit()
    {
        Debug.Log("Exit");
        SceneManager.LoadScene("Scroller");
    }

    public void ResetSymbols()
    {
        userInputStr = "AAPL, GOOGL, MSFT, AMZN, FB, TSLA, MRNA, GE, NVDA, JPM, AMD, V, JNJ, QCOM, DELL, ADBE, NFLX, TWTR, ORCL, COIN, PFE, GME";
        userInput.GetComponent<TMP_InputField>().text = userInputStr;
    }
}
