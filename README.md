# SDRSharp_v17xx-Plugin-for-SDRPlay RSP1
SDRSharp v17 Plugin for SDRPlay: Source code and binary for Windows 10 (dll)
This plugin is taken from https://github.com/Sir-Loin/SDRPlay_SDRSharp_Plugin and adapted to allow the RSP1 SDRPlay to work with recent 2019 versions of SDR# (v 1.0.0.0.1708).

To build:
With Microsoft Visual Studio 2017:

- Open the .sln file in Visual Studio
- Add SDRSharp.Radio.dll and SDRSharp.Common.dll from your SDR# install folder as external references.

To install:
- put the file SDRSharp.SDRplay.dll in the SDR# install folder
- with a text editor, open the file "FrontEnds.xml" in the SDR# install folder and add the line : 
 <add key="SDRplay" value="SDRSharp.SDRplay.SDRplayIO,SDRSharp.SDRplay"/>
 in the "<frontendPlugins>" section.
  
