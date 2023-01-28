# -*- coding: utf-8 -*-
"""
Created on Sat Jan 28 21:20:01 2023

@author: Simon
"""

path = "C:\\Users\\Simon\\Desktop\\CubeV2\\MapRead1.bmp"

import os
import scipy
scipy
image= misc.imageio.imread(path, flatten= 0)


from PIL import Image
img = Image.open(path)
for i in img:
    print(i)

img = np.array(Image.open('path_to_file\file.bmp'))
imglist = list(img.getdata())
size = img.size

i = 0
for pixel in imglist:
    if pixel[0] == 0:
        print('X',end='')
    else:
        print('-',end='')
        
    if i % size[0] is 0:
        print()
    i=i+1
        
    