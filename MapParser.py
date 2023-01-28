# -*- coding: utf-8 -*-
"""
Created on Sat Jan 28 21:20:01 2023

@author: Simon
"""

path = "C:\\Users\\Simon\\Desktop\\CubeV2\\MapRead1.bmp"
pathOut = "C:\\Users\\Simon\\Desktop\\CubeV2\\BigMap.txt"

f = open(pathOut,'w+')

def writePrint(char):
    print(char,end='')
    f.write(char)

from PIL import Image
img = Image.open(path)
imglist = list(img.getdata())
size = img.size

i = 1
for pixel in imglist:
    if pixel[0] == 0:
        writePrint('x')
    elif pixel[0] == 255:
        writePrint('-')
    else:
        writePrint('?')
    if i % size[0] is 0:
        writePrint('\n')
    i=i+1

f.close()