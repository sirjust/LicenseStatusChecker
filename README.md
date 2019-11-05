This program takes in a list of Washington State Licenses and outputs two files.
One, called Send[Date].xlsx, is a list of license holders who have not yet renewed.
The other, called DoNotSend[Date].xlsx, is a list of those who have already completed their CEUs.

**IMPORTANT**
The file to be read must be called "listToCheck.xlsx" (with no quotation marks) and be inside of 
the "Files" folder. It can have either one or multiple sheets (PL, PT, and EL) from which to check.
The outcoming spreadsheets have only one sheet, for ease of reading.

As of October 2019 the program is capable of running 150 licenses in 4 minutes.

**NOTES**
Sometimes an error will show up in the console, something like this:
[14592:13944:0309/221728.709:ERROR:component_installer.cc(125)] 
Move failed.: The system cannot find the path specified. (0x3)

I have thoroughly checked the output of the program and it remains consistent: the license holders
before and after the error are the same in the output documents as in the original spreadsheet.
This error shouldn't cause a problem.

The program should be run on a fast, modern computer with a solid broadband internet connection.
If internet is spotty or slow the program will not run correctly.

Sometimes the web driver doens't find the very first element and times out. 
If this happens the program must be run again.
I have seen this only a couple times and believe it has to do with connecting initially to the browser.
Keep an eye on the program at the very start to avoid losing out on a test.
Other bugs shouldn't keep it from running at this point.

If the state of the Washington L&I website changes, the program will need to be updated.
Contact Justin Moore for support if that happens.
