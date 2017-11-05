# Exporting

Gold Box Explorer lets you export all images from a DAX file in several forms. In this example, we'll export the contents of a file from Pool of Radiance.

![](Exporting_wiki-exporting01.png)

In the file list, select the file you want to export images from and right click to bring up the **Export** menu.

![](Exporting_wiki-exporting02.png)

Click on **Export** and this will launch the **Export Images** dialog.

![](Exporting_wiki-exporting03.png)

From here you can select the folder where to export images to (defaults to the users My Pictures directory) and setting the output format (Windows Bitmap, JPEG, or PNG). Click **OK** to export the images to the target directory. A dialog box will pop up after the export is done to indicate the number of images saved and in what format.

![](Exporting_wiki-exporting04.png)

Once exported you can view the images in Windows Explorer and use them in other programs.

![](Exporting_wiki-exporting05.png)

The files are saved using this naming convention:

{"[DAX File Name](DAX-File-Name)_[Block Id](Block-Id)_[Bitmap Number](Bitmap-Number).[File Format](File-Format)"}

Where:
* **DAX File Name** is the original filename where the image was exported from (without the .DAX extension)
* **Block Id** is a saved id number that each block in a DAX file has
* **Bitmap Number** is the sequential number of the bitmap in the block. Some blocks contains more than one bitmap
* **File Format** is the exported format (BMP, JPG, or PNG)

_Note: Exported images overwrite any other files with the same name without prompting_