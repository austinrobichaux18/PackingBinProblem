# PackingBinProblem

# What is this Application?
This application was designed to check if all image sizes fit within the master image.  
This is also considered the 2-dimensional Packing Bin problem.  
It is considered a NP-Hard problem with no polynomial time complexity.  
The 2-dimensional version of this problem has uses in fitting images on a page and fitting advertisements within time slots
The 3-dimensional version of this problem has uses in packing shipping containers, mail carriers, and other logisitics.  
It is similar to the 0/1 Knapsack problem.  

This application is NOT an exact decider for this problem. It uses a battery of approximation algorithms instead.  
A brute force algorithm for this would likely be near n! time complexity or worse.  

# How to run this application
To check a file, edit the "InputFile.txt" to contain data you wish to check and run the application. 
The console will indicate pass or failure. 
Alternatively, you can add test cases in the Tests project.  

# Sources, Similar material, Educational 
See relevant sources that helped me understand this problem.
Ultimately none of which gave usable c# code, so I decided to create this application!  

https://github.com/tahoe01/Bin-Packing

https://en.wikipedia.org/wiki/Cutting_stock_problem  
https://en.wikipedia.org/wiki/Bin_packing_problem  
https://en.wikipedia.org/wiki/NP-hardness  
https://www.sciencedirect.com/science/article/pii/S0307904X19301830  

https://github.com/krris/3d-bin-packing/blob/master/README  
https://github.com/juj/RectangleBinPack/blob/master/Readme.txt  

https://stackoverflow.com/questions/2192087/3-dimensional-bin-packing-algorithms  
https://stackoverflow.com/questions/61771700/check-if-rectangle-fits-in-other-rectangle-which-already-contains-rectangles  
https://cs.stackexchange.com/questions/88499/fitting-different-rectangles-inside-a-rectangle  

Company that uses 3d version (has nice 3d visual):  
https://topseng.com/maxload-cargo-load-planning-optimization/  
Online calculator that does something similar:  
https://www.engineeringtoolbox.com/smaller-rectangles-within-larger-rectangle-d_2111.html   

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