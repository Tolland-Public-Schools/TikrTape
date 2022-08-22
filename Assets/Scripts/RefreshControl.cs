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
using UnityEngine.SceneManagement;
using System;

public class RefreshControl : MonoBehaviour
{
    private static float elapsed;
    private static float maxTime; // seconds until refresh
    
    public int refreshFreq;

    // Start is called before the first frame update
    void Start()
    {        
        // pull user prefs
        refreshFreq = PlayerPrefs.GetInt("refreshFreq");

        // 30 min
		if (refreshFreq == 0)
            maxTime = 1800;
			
		// 1 hour
		if (refreshFreq == 1)
            maxTime = 3600;
			
		// 3 hours
		if (refreshFreq == 2)
            maxTime = 10800;

		// 6 hours
		if (refreshFreq == 3)
            maxTime = 21600;
    }

    // Update is called once per frame
    void Update()
    {	
        elapsed += Time.deltaTime;

        if (elapsed >= maxTime)
        {
            SceneManager.LoadScene("Scroller");
            elapsed = 0;
        }  
    }
}
