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

Applets
------------
To make an applet using more than plain text and a background image, one can create a userControl and make it inherit from the Logitech_LCD.Applets.BaseApplet class.
You will then have to override the OnDataUpdate method and you're ready to design your control like you would do for any other Winform Control.