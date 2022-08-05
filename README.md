## Humble Grab ##

### Description ###
Humble Grab is a tool for quickly grabbing your games on your Humble Bundle account and cross-checking
it against other game libraries such as Steam, GOG, and Epic Games.

### Usage ###
First, you need to get your authentication cookie from humblebundle.com. To do so, navigate there, log in and press F12 to open the developper tools of your browser. Navigate to the cookies tab, and look for a cookie named _simpleauth_sess (on the humblebundle.com domain). Copy and paste the value into the config file.

`config.yaml` is the configuration file for the program. There, you will find settings for using different clients, steam id, and more.

Results are saved into an index.html that will automatically open when the program is finished.

### Notes ###
If the included Steam API key stops working, you must use your own and replace the key in the config file.

### Parameters ###
TODO

### Supported Platforms ###
| Platform | Supported |
| --- | --- |
| Humble | ✔️ |
| Steam | ✔️ |
| Origin | ❌ |
| Epic | ❌ |
| GOG | ❌ |
| Blizzard |❌|
