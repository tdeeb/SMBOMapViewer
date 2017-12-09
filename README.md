# SMBOMapViewer
A map viewer to view SMBO maps.


## Overview
This program reads in the binary map files from the Super Mario Bros. Online fangame and displays them. The map data was originally written to binary files in Visual Basic 6, so this program uses legacy file system methods to decode it.

All SMBO maps and tilesets are included in the project.

## Getting Started
You will need at least  MonoGame 3.7.0.1129 and Visual Studio 2017, but earlier versions of Visual Studio should work just fine.

## Features
* Use the left and right arrow keys to change maps. They loop around; there are some empty maps, so keep going if you switch and don't see anything.
* Use WASD to translate the camera.
* Press R to reset the camera.
* Press Space to take a screenshot of the currently displayed map. Keep in mind that if you manipulated the camera, those manipulations will show up in the screenshot as well. Reset the camera with R before taking a screenshot to avoid this. The file is currently placed on your Desktop with the name "image.png," but a file system dialogue will be added so you can choose where to place it and what to name it.

## Author(s)
* **Thomas Deeb (aka Kimimaru)**

Contact: thomasdeeb1@gmail.com

## License
This project is licensed under the MIT License - see the LICENSE.md file for details.

## Acknowledgements
* hydrakiller4000 (aka hydra) - helped track down the original data types used for holding maps and tiles.