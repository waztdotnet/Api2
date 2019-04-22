  SimpleAddIn Installer Sample
  ============================
  
  DESCRIPTION
  This sample is an example of an installer for an Inventor Add-In. In this particular case, it
  demonstrates the steps to package and install the SimpleAddIn sample on a user's machine. After
  installation, the following files will be placed on the user's machine:

	SimpleAddIn.dll - The main AddIn dll
	Inventor.Interop.dll - The interop assembly for the Inventor typelibrary
	Registration.exe files - The registration executable for the AddIn
	
  The SimpleAddIn project (in the main folder for this sample) contains the source code for the AddIn
  and the Registration project (in the "Registration" folder) contains the source code for the
  registration executable.  The Readme text files in the respective folders also provide more information.

  This installer project is a "Setup and Deployment Project" created in Microsoft Visual Studio 2005.

  The most important step is to install and register the AddIn dll which in this case is the 
  SimpleAddIn.dll. Since this is a .NET assembly, regasm.exe can be used to register it. But, since
  this AddIn utilizes a registration executable (for more information see the Registration project 
  (in the "Registration" folder) and its Readme file), this installer invokes the registation 
  executable.
 
  The steps performed by the installer are as follows:
  1) Installs the files SimpleAddIn.dll (the main AddIn dll), Inventor.Interop.dll (the interop assembly 
     for the Inventor typelibrary) and the Registration.exe files (the registration executable for the AddIn) 
     on the user's machine.

  2) Invokes the Registration.exe file with a "/install" command line argument during install. 
     This is performed as an install custom action.

  3) Upon uninstall, invokes the Registration.exe file with a "/uninstall" argument before removing 
     the files from the disk. 
     This is performed as an uninstall custom action.

 