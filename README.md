# SMBOMapViewer
A map viewer to view maps from the fangame, Super Mario Bros. Online.


## Overview
This program reads in the binary map files from the Super Mario Bros. Online fangame (http://supermariobrosonline.tk/) and displays them. The map data was originally written to binary files in Visual Basic 6. This should also work for other games made in the Eclipse engine, provided you have the tilesets to display them properly.

All SMBO maps and tilesets are included in the project.

## Getting Started
Take a look at the [releases](https://github.com/tdeeb/SMBOMapViewer/releases) and download the version you want that's available for your platform. Follow the instructions on the page to run it.

## Compiling
1. Clone the repository
2. Install [MonoGame](https://github.com/MonoGame/MonoGame) 3.7.0.1129 or later.
3. Install Visual Studio 2017, though earlier versions may work as well. On non-Windows platforms, install JetBrains Rider, MonoDevelop, or Xamarin Studio. On OSX you can also use Visual Studio for Mac. The project targets DesktopGL and .NET 4.7, and it uses the latest major C# version (7.0).

## Features
* Press L to turn on Linear map changing (default) and G to turn on GameLike map changing. Linear goes through maps in order of map number, and GameLike mode goes through maps connected to other maps.
    * In Linear mode, press the Left arrow key to go down a map number and press the Right arrow key to go up a map number. The Up and Down arrow keys go down and up, respectively, by 10 maps.
    * In GameLike mode, pressing an arrow key will go where the map would lead in the actual game. This doesn't work for warp tiles and there may be maps that aren't linked, so you will need to switch to Linear mode to get out of these maps.
* Use WASD to translate the camera.
* Use the + and - keys to zoom into and out of the map.
* Press R to reset the camera.
* Press O to toggle whether or not to render tiles hidden by roofs. There aren't many of these in SMBO. If you want to see an example, Map 14 utilizes roofs.
* Press Space to take a screenshot of the currently displayed map. Map names with question marks in them are automatically replaced with spaces, since you cannot have a question mark in a file name. On Windows, you will be prompted what to name the file and where to save it. On Linux and OSX, screenshots will be placed in a newly created Screenshots folder in the same directory as the application, with the map name as the file name; it also supports multiple screenshots of the same map. As of the current version, the produced screenshot will ignore any manipulations to the camera, so you no longer need to reset the camera before taking a screenshot.

## Author(s)
* Kimimaru

## Acknowledgements
* hydrakiller4000 (aka hydra) - helped track down the original data types used for holding maps and tiles.
