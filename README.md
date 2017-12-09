# SMBOMapViewer
A map viewer to view maps from the fangame, Super Mario Bros. Online.


## Overview
This program reads in the binary map files from the Super Mario Bros. Online fangame (http://supermariobrosonline.tk/) and displays them. The map data was originally written to binary files in Visual Basic 6, so this program uses legacy file system methods to decode it.

All SMBO maps and tilesets are included in the project.

## Getting Started
You will need at least MonoGame 3.7 and Visual Studio 2017, but earlier versions of Visual Studio should work just fine.

## Features
* Press L to turn on Linear map changing (default) and G to turn on GameLike map changing. Linear goes through maps in order of map number, and GameLike mode goes through maps connected to other maps.
** In Linear mode, press the Left arrow key to go down a map number and press the Right arrow key to go up a map number. The Up and Down arrow keys go down and up, respectively, by 10 maps.
** In GameLike mode, pressing an arrow key will go where the map would lead in the actual game. This doesn't work for warp tiles and there may be maps that aren't linked, so you will need to switch to Linear mode to get out of these maps.
* Use WASD to translate the camera.
* Use the + and - keys to zoom into and out of the map.
* Press R to reset the camera.
* Press Space to take a screenshot of the currently displayed map. You will be prompted what to name the file and where to save it. Keep in mind that if you've manipulated the camera, those manipulations will show up in the screenshot as well. Reset the camera with R before taking a screenshot to avoid this.

## Author(s)
* **Thomas Deeb (aka Kimimaru)**

## License
This project is licensed under the MIT License - see the LICENSE.md file for details.

## Acknowledgements
* hydrakiller4000 (aka hydra) - helped track down the original data types used for holding maps and tiles.