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

public class Menu : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject errorPopUp;
    private bool active = false;

    void Start()
    {
        Canvas.SetActive(false);
    }

    void Update()
    {
        //esc toggles menu
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(active == false)
            {
                Canvas.SetActive(true);
                active = true;
            }
            else
            {
                Canvas.SetActive(false);
                active = false;
            }
        }
        
        //if menu is up then show the cursor
        if(active == true)
        {
            Cursor.visible = true;
        }
        //if error panel is up activate cursor
        else if(errorPopUp.activeSelf == true)
        {
            Cursor.visible = true;
        }
        //otherwise 
        else
        {
            Cursor.visible = false;
        }
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }
    public void Resume()
    {
        Canvas.SetActive(false);
        errorPopUp.SetActive(false);
        //reset esc toggle function
        active = false;
    }
    public void Refresh()
    {
        SceneManager.LoadScene("Scroller");
    }
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
