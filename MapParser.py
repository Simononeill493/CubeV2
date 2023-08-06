# -*- coding: utf-8 -*-
"""
Created on Sat Jan 28 21:20:01 2023

@author: Simon
"""

pathIn = r"C:\Users\Simon\Desktop\CubeV2\New tutorial\StageMockup.png"
pathOut = r"C:\Users\Simon\Desktop\CubeV2\New tutorial\NewTutorialMap.txt"


colorsToCharacters = dict()
colorsToCharacters[(255,255,255)] = '-' #Space
colorsToCharacters[(89,89,89)] = 'W' #Wall
colorsToCharacters[(0,0,0)] = 'R' #Rock
colorsToCharacters[(255,0,200)] = 'P'  #Player
colorsToCharacters[(255,145,0)] = 'T'  #Respawner
colorsToCharacters[(221,126,0)] = 'C'  #Crafting Table
colorsToCharacters[(157,0,255)] = 'E'  #Turret
colorsToCharacters[(255,251,0)] = 'G'  #Goal
colorsToCharacters[(0,97,255)] = 'L'  #Laser flower
colorsToCharacters[(255,103,0)] = 'M'  #Pickaxe flower




f = open(pathOut,'w+')

def writePrint(char):
    print(char,end='')
    f.write(char)

from PIL import Image
img = Image.open(pathIn)
imglist = list(img.getdata())
size = img.size


i = 1
for pixel in imglist:
    pixelTuple = (pixel[0],pixel[1],pixel[2])
    if(pixelTuple in colorsToCharacters):
        writePrint(colorsToCharacters[pixelTuple])
    else:
        writePrint('?')
    if i % size[0] == 0:
        writePrint('\n')
    i=i+1


#for pixel in imglist:
#    print("(" + str(pixel[0]) + "," + str(pixel[1]) + "," +str(pixel[2]) + "),")



f.close()