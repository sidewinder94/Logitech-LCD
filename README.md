[![Build status](https://ci.appveyor.com/api/projects/status/x4ut9kqy8vr3khuy/branch/master?svg=true)](https://ci.appveyor.com/project/sidewinder94/logitech-lcd/branch/master)

#Logitech-LCD

A C# Wrapper allowing one to use the functions of the logitech SDK in .NET

#Usage

1. Clone the project in any location
2. Add the project to your solution
3. Add a reference from your project to this one
4. Add in your project a Lib folder containing an x86 and an x64 folder, each of these must contain the LogitechLcd.dll (the wrapper dll in the logitech LCD SDK, you may have to rename it as the name might have changed) file
5. You're ready to go and use this wrapper
6. All of the SDK's base methods are exposed in the LogitechLcd class

##Applets

###Applet WinForm :
  
To make an applet using more than plain text and a background image, one can create a userControl and make it inherit from the Logitech_LCD.Applets.BaseApplet class.

You will then have to override the OnDataUpdate method and you're ready to design your control like you would do for any other Winform Control.

###Applet WPF :

To use the WPF applet, you'll have to create a new WPF window/usercontrol

This control will have to look like this : 
```
<UserControl
            x:Class="TestWPFApplet.UserControl1"
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
            xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
            xmlns:applets="clr-namespace:Logitech_LCD.Applets;assembly=Logitech-LCD"
            d:DesignHeight="240"
            d:DesignWidth="320"
            mc:Ignorable="d">

    <applets:BaseWPFApplet>
        <Grid Background="White">
            <Label>Applet Content</Label>
        </Grid>
    </applets:BaseWPFApplet>

</UserControl>
```
If you have some code to run before any visual update, there is an event called `OnDataUpdate` who will be called before any visual update.

#Known Issues
##WinForms
If you are in Release build configuration or the first build of the lib wasn't done yet, the designer won't be able to initialize.
When disiging the your control, be sure to be in Debug build configuration, run a build of the lib, close the designer window and reopen it.
