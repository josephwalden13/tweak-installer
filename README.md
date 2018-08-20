## I am not responsible for any damage that may be caused by using this tool. Use at your own risk

# tweak-installer

An open source C# tweak installer for iOS. 
This tool can be used as an alternative to Cydia / Installer and other package managers, but it is not a replacement. 
When electra11.1.2 was first released it had no package manager built in so I created this to make installing deb packages easier. 
When installing files over pre-existing files on the device it'll ask you to confirm and will create a backup which can then be restored when removing the package.

### Requirements

* An iOS device with root access and ssh installed. 
* If you want to use tweaks you need either Substitute, Substrate or some other dylib injection method.
