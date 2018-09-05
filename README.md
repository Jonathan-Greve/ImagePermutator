# ImagePermutator
Creates a sheet of images in any format you want.

How to use: Only meant as a class library for now.

The basic use pattern is:
1) Give path to image file
2) Set sheet format (ex. A4 -> 210mm x 297mm) or you can give it any format you want, ex. 1234mm x 4321mm.
3) Set up the image format. For US passports it seems to be 51mm x 51mm. For Chinese visa it's 38mm x 48mm.
4) Decide which area of the original image should be used. Called the CropArea.
5) Call the ImageSheet.Create() function.

Works with very high pixel resolution. Not sure what the limit is, or if it's limited to memory.
