# PackingBinProblem

# What is this Application?
This application was designed to check if all image sizes fit within the master image.

# How to run this application
To check a file, edit the "InputFile.txt" to contain data you wish to check and run the application. 
The console will indicate pass or failure. 


# Original Directions
We have an input text file where each line specifies the width and the height (in pixels) of an image.

The first image is the size of the blank Master image. 
Write a program that fits the rest of the images into this Master image, or prints an appropriate message if a solution could not be found. 
To fit an image means to place it somewhere in the Master image, such that no two images overlap.

Example input file:
80 50
20 5
7 18
7 18
6 32
48 50
3 7
80 6
(In this case we have 7 images of various widths and heights, and the Master image is 80x50
pixels).