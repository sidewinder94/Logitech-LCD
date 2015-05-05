Logitech-LCD
============

A C# Wrapper allowing one to use the functions of the logitech SDK in .NET

Usage
============
1. Clone the project in any location
2. Add the project to your solution
3. Add a reference from your project to this one
4. Add in your project a Lib folder containing an x86 and an x64 folder, each of these must contain the LogitechLcd.dll file
5. You're ready to go and use this wrapper
6. All of the SDK's base methods are exposed in the LogitechLcd class

Applets
------------
To make an applet using more than plain text and a background image, one can create a userControl and make it inherit from the Logitech_LCD.Applets.BaseApplet class.
You will then have to override the OnDataUpdate method and you're ready to design your control like you would do for any other Winform Control.

Known Issues
============
If you are in Release build configuration or the first build of the lib wasn't done yet, the designer won't be able to initialize.
When disiging the your control, be sure to be in Debug build configuration, run a build of the lib, close the designer window and reopen it.
