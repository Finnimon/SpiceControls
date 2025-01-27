Prep for Matlab:
- Assert that you have at least .NET Core 6 installed and no .NET Core 9.x.x installed
- Add the release compiled dll into your workfolder
- Add the following lines at the top of your script:

"  
%NET interop preperations  
dotnetenv('core');  
dllPath=strcat(pwd,'\NetlistEditor.dll');  
NET.addAssembly(dllPath);  
"
