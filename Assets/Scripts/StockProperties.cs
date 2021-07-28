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

public class StockProperties : MonoBehaviour{
    //movement variables
    private Vector3 velocity;
    private Vector3 desiredPosition;
    private Vector3 smoothPosition;
    public float scrollSpeed;

    //stock info variables
    public string stockSymbol;
    public float stockPrice;
    public bool stockIncrease;
    private TextMeshProUGUI symbolText;
    private TextMeshProUGUI priceText;
    public GameObject Arrow;
    public Sprite upArrow;
    public Sprite downArrow;
    public Scroller scroller;

    void Start()
    {
        //determine which arrow to display
        Arrow = transform.GetChild(0).gameObject;
        if (stockIncrease == true) //set sprite to up arrow
        {
            Arrow.GetComponent<SpriteRenderer>().sprite = upArrow;
        }
        else //set sprite to down arrow
        {
            Arrow.GetComponent<SpriteRenderer>().sprite = downArrow;
        }

        //display stock symbol on game obj
        symbolText = transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        symbolText.SetText(stockSymbol);

        //display stock price on game obj
        priceText = transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        priceText.SetText((stockPrice.ToString()));
    }


    void Update()
    {
        if (!scroller?.ready ?? false) return;
        //translate
        velocity = Vector3.zero;
        desiredPosition = transform.position + new Vector3(-scrollSpeed, 0, 0);
        smoothPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 0.3f);
        transform.position = smoothPosition;
    }
}
