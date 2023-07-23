Begin by downloading the ZIP file and extracting the "Serialmote" folder to a location of your choice.

Inside the "Serialmote" folder, you'll find a file named "Settings.ini" with basic configuration options such as URL Port and COM Port. Only modify these options if needed; leave the rest unchanged.

Open Command Prompt by pressing Win + R, typing "cmd," and pressing Enter.

Use the cd command to navigate to the directory where your "Serialmote.exe" file is located. For example:

**Installing the service with the following command:**
cd C:\your\chosen\folder\Serialmote
Install the Windows service using the sc command:
sc create Serialmote binPath= "C:\your\chosen\folder\Serialmote\Serialmote.exe"

**Start the service with the following command:**
sc start Serialmote
sc config Serialmote start= auto
Now, your "Serialmote" service will automatically start whenever your computer boots up
