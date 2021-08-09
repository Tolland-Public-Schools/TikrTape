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
using System.Text.RegularExpressions;

public class SettingsMenu : MonoBehaviour
{
    private string stockUserInputStr;
    private int stockRefreshFreq;
    private int newsRefreshFreq;
    //game objects in scene
    public GameObject userInput;
    public GameObject stockRefreshSlider;
    public GameObject newsRefreshSlider;
    public GameObject economistToggle;
    public GameObject nytToggle;
    public GameObject CNS1Obj;
    public GameObject CNS2Obj;
    public GameObject CNS3Obj;

    public void Start()
    {
        //make sure the cursor is visable
        Cursor.visible = true;
        
        //make sure UI elements match the state saved in userprefs
        
        //input boxes: 
        userInput.GetComponent<TMP_InputField>().text = PlayerPrefs.GetString("rawSymbols");
        CNS1Obj.GetComponent<TMP_InputField>().text = PlayerPrefs.GetString("CNS1");
        CNS2Obj.GetComponent<TMP_InputField>().text = PlayerPrefs.GetString("CNS2");
        CNS3Obj.GetComponent<TMP_InputField>().text = PlayerPrefs.GetString("CNS3");
        //sliders:
        stockRefreshSlider.GetComponent<Slider>().value = (float)PlayerPrefs.GetInt("stockRefreshFreq");
        newsRefreshSlider.GetComponent<Slider>().value = (float)PlayerPrefs.GetInt("newsRefreshFreq");
        //toggle boxes:
        if (PlayerPrefs.GetString("ECON") == "true")
        {
            economistToggle.GetComponent<Toggle>().isOn = true;
        }
        else
        {
            economistToggle.GetComponent<Toggle>().isOn = false;
        }
        if (PlayerPrefs.GetString("NYT") == "true")
        {
            nytToggle.GetComponent<Toggle>().isOn = true;
        }
        else
        {
            nytToggle.GetComponent<Toggle>().isOn = false;
        }
    }

    public void ValidateCharacters()
    {
        //only allow characters A-Z (capital or not) and commas
        userInput.GetComponent<TMP_InputField>().text = Regex.Replace(userInput.GetComponent<TMP_InputField>().text, @"[^a-zA-Z, ]", "");
        userInput.GetComponent<TMP_InputField>().text = userInput.GetComponent<TMP_InputField>().text.ToUpper();
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
        stockUserInputStr = userInput.GetComponent<TMP_InputField>().text;
        PlayerPrefs.SetString("rawSymbols", stockUserInputStr);

        //save the news sources and feed links to user prefs
        FormatNews();
        
        //go back to scroller 
        SceneManager.LoadScene("Scroller");
    }

    public void Exit()
    {
        Debug.Log("Exit");
        SceneManager.LoadScene("Scroller");
    }

    public void FormatNews()
    {
        List<string> newsPrefsList = new List<string>();
        
        //check if switches are toggled
        if(economistToggle.GetComponent<Toggle>().isOn == true)
        {
            PlayerPrefs.SetString("ECON", "true");
        }
        else
        {
            PlayerPrefs.SetString("ECON", "false");
        }
        
        if(nytToggle.GetComponent<Toggle>().isOn == true)
        {
            PlayerPrefs.SetString("NYT", "true");
        }
        else
        {
            PlayerPrefs.SetString("NYT", "false");
        }

        if(CNS1Obj.GetComponent<TMP_InputField>().text != null | CNS1Obj.GetComponent<TMP_InputField>().text != "")
        {
            //add to player prefs
            string inputRss = CNS1Obj.GetComponent<TMP_InputField>().text;
            PlayerPrefs.SetString("CNS1", inputRss);
        }
        else
        {
            PlayerPrefs.SetString("CNS1", null);
        }

        if(CNS2Obj.GetComponent<TMP_InputField>().text != null | CNS2Obj.GetComponent<TMP_InputField>().text != "")
        {
            //add to player prefs
            string inputRss = CNS2Obj.GetComponent<TMP_InputField>().text;
            PlayerPrefs.SetString("CNS2", inputRss);
        }
        else
        {
            PlayerPrefs.SetString("CNS2", null);
        }

        if(CNS3Obj.GetComponent<TMP_InputField>().text != null | CNS3Obj.GetComponent<TMP_InputField>().text != "")
        {
            //add to player prefs
            string inputRss = CNS3Obj.GetComponent<TMP_InputField>().text;
            PlayerPrefs.SetString("CNS3", inputRss);
        }
        else
        {
            PlayerPrefs.SetString("CNS3", null);
        }
    }

    public void ResetStockSymbols()
    {
        stockUserInputStr = "AAPL, GOOGL, MSFT, AMZN, FB, TSLA, MRNA, GE, NVDA, JPM, AMD, V, JNJ, QCOM, DELL, ADBE, NFLX, TWTR, ORCL, COIN, PFE, GME";
        userInput.GetComponent<TMP_InputField>().text = stockUserInputStr;
    }
}
