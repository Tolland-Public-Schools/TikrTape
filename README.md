# TikrTape
A free and open source stock ticker designed and developed by IT interns at Tolland Public Schools. 

![screenshot](https://github.com/Tolland-Public-Schools/TikrTape/blob/main/Screenshot.png)

## Features

- Adjustable refresh rate so stocks and news are always up to date.
- Compatible with any screen size.
- Stocks can easily be added/ removed via the settings menu.
- Default headlines from the Economist and New York Times.
- Up to 3 custom rss news sources.

## Setup & Development

TikrTape was developed in Unity. The [Unity Editor](https://unity3d.com/unity/whats-new/) is required to compile it. All scripts are in C#.

On the first launch of the program if no stocks appear, press esc to enter the menu and navigate to settings. Either set the stocks to default with the provided button or enter your own.

Current hardcoded yet easily adjustable features are:

 - Scroll speed (Unity editor under News/Stock scroller.cs attached to game object) 
 - Position of stocks/news items (Unity editor under News/Stock scroller.cs attached to game object)
 - Default stock symbols (string at top of Scroller.cs)

[Diagram of script structure](https://github.com/Tolland-Public-Schools/TikrTape/wiki/General-Script-Structure-Diagram)

## License
GNU GPL v3.0
