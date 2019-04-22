  SimpleAddIn Sample
  =======================
  
  DESCRIPTION
  This sample demonstrates a minimal implementation of an Add-In.  
  
  This sample corresponds to the Add-In section of the Developer's Guide found in the programming 
  online help.  See that section for details about how Add-Ins work and how to create an Add-In.

  How to run this sample:
  1) Creat SimpleAddIn.addin file copy the following section into it.

     <?xml version="1.0" encoding="utf-8"?>
     <!-- Type attribute is same as Type registry key (Standard, Translator, Plugin (Server only) -->
     <Addin Type="Standard">
     <ClassId>{963308E2-D850-466D-A1C5-503A2E171552}</ClassId>
     <ClientId>{963308E2-D850-466D-A1C5-503A2E171552}</ClientId>

     <!-- Both of the following fields should be translated. NO OTHER FIELDS SHOULD BE TRANSLATED! -->
     <DisplayName>SimpleAddIn</DisplayName>
     <Description>SimpleAddIn</Description>

     <!-- Assumes that SimpleAddIn.dll is underneath Inventor\bin -->
     <Assembly>SimpleAddIn.dll</Assembly>

     <SupportedSoftwareVersionGreaterThan>15..</SupportedSoftwareVersionGreaterThan>
     <LoadOnStartUp>1</LoadOnStartUp>
     <Hidden>0</Hidden>
     </Addin>

  2) Copy SimpleAddIn.addin into ...\Autodesk\Inventor 20XX\Addins folder.
     For XP: C:\Documents and Settings\All Users\Application Data\Autodesk\Inventor 20XX\Addins.
     For Vista/Win7: C:\ProgramData\Autodesk\Inventor 20XX\Addins.

  3) Copy bin\Release\SimpleAddIn.dll into Inventor bin folder(For example: C:\Program Files\Autodesk\Inventor 20XX\Bin).

  4) Startup Inventor, the AddIn should be loaded
  
  When you run Inventor with classic UI and if you open a part document and activate a sketch and click on the title of the panel bar, 
  you can select the toolbar "AddInSlot".You can also access the floating toolbar by going
  to "Tools -> Customize -> Toolbars (tab)" and select the "AddInSlot" toolbar and press
  the "Show" button. 
  
  When you run Inventor with Ribbon UI and if you open a part document and activate a sketch and you can see the "Slot" panel on "Sketch" tab.
  
  The two combo boxes allow you to specify the height and width of the slot 
  sketch. If you run the "Draw Slot" command and a sketch is active, it will draw a shape in the 
  active sketch. If you run the "Add slot width/height" command it will add one more entry in the 
  combo boxes for width and height. The "Toggle Slot State"command toggles the "Draw Slot" command.

  To unregister the AddIn, use regasm.exe (type regasm /u "dllName.dll" at the command prompt)

  Language/Compiler: VC# (.NET)
  Server: Inventor.

 