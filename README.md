# KeePassChallenge

KeePassChallenge is a plugin for [KeePass](https://keepass.info/) that adds support for Yubikey challenge-response.

## Installation

Right now this plugin is only supported by a forked version of KeePass available [here](https://github.com/pragacz/KeePass).

To install this plugin, transfer the `KeePassChallenge` folder from the latest [release](https://github.com/pragacz/KeePassChallenge/releases) into the `Plugins` directory of KeePass.

## Usage

To add Yubikey protection to your password database, simply select this plugin as the `Key provider` in the `Master Key` settings.

Every time you open the database, your Yubikey will be interviewed and must be present. If you configured the Yubikey to require physical input, you will be asked to touch it two times (or just hold it longer).

## Advantages

There are some other plugins trying to serve the same purpose. Because I was not satisfied with them, I decided to create this one.

To give you some context, the advantages this plugin offers over similar plugins:

- Does not use additional files. Everything is stored inside the database itself.
- Your secret remains completely safe inside your Yubikey. It is not and in fact cannot be accessed by this plugin, thus substantially increasing security.
- Has subjectively nicer looking UI.
- Uses correct default system font.
- Is semi-compatible with [KeePassXC](https://keepassxc.org/), having the ability to unlock databases created by it.
