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
using TMPro;

public class NewsProperties : MonoBehaviour
{
    //News Object variables
    public TextMeshProUGUI headlineText;
    public TextMeshProUGUI descriptionText;
    public string headlineStr;
    public string descriptionStr;
    public Scroller scroller;
    public string newsSource;
    public GameObject sourceLogo;
    public Sprite NYT;
    public Sprite Economist;
    public Sprite Generic;

    //movement variables
    private Vector3 velocity;
    private Vector3 desiredPosition;
    private Vector3 smoothPosition;
    public float scrollSpeed;

    void Start()
	{
		//set correct news source image
        sourceLogo = transform.GetChild(2).gameObject;
		if (newsSource == "Economist") //set sprite as Economist
		{
			sourceLogo.GetComponent<SpriteRenderer>().sprite = Economist;
		}
		else if (newsSource == "NewYorkTimes") //set sprite to NYT
		{
			sourceLogo.GetComponent<SpriteRenderer>().sprite = NYT;
		}
        else
        {
            sourceLogo.GetComponent<SpriteRenderer>().sprite = Generic;
        }
	}

    void Update()
    {
        //if all objs haven't instantiated yet don't start moving
        if (!scroller?.ready ?? false) return; 

        //scroll
        velocity = Vector3.zero;
        desiredPosition = transform.position + new Vector3(-scrollSpeed, 0, 0);
        smoothPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 0.3f);
        transform.position = smoothPosition;
    }
}
