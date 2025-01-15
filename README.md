# Jayo's Crowd Control Plugin for VNyan

A VNyan Plugin that allows you to connect to an active [Crowd Control](https://crowdcontrol.live) session and trigger VNyan Triggers in response. Take your viewer interaction to the next level by responding to Crowd Control effects through your VNyan node graphs!

# Table of contents
1. [Installation](#installation)
2. [Usage](#usage)
    1. [Authorizing Crowd Control](#authorizing-crowd-control)
    2. [Controlling VNyan](#controlling-vnyan)
    3. [Triggers](#triggers)
    4. [Parameters](#parameters)
    5. [Trigger Browser and Trigger History](#trigger-browser-and-trigger-history)
3. [Example Resources](#example-resources)
4. [Development](#development)
5. [Special Thanks](#special-thanks)

## Installation
1. Grab the ZIP file from the [latest release](https://github.com/jayo-exe/CrowdControlVNyanPlugin/releases/latest) of the plugin.
2. Extract the contents of the ZIP file _directly into your VNyan installation folder_.  This will add the plugin files to your VNyan `Item\Assemblies` folder.
3. Open the VNyan Settings window, go to the "Misc" section, and ensure that **Allow 3rd Party Mods/Plugins** is enabled. This is required for this plugin  (or any plugin) to function correctly, so if you've already got other plugins installed you can probably skip this step. Restart VNyan if you needed to enable this in order for it to take effect.
6. Once VNyan loads, confirm that a button for the plugin now exists in your Plugins window!

## Usage
### Authorizing Crowd Control
In order for the plugin to see your active Crowd Control session and listed for incoming effects, you'll need to authorize the plugin access to your Crowd Control account.  You can do this with the "Authorize" button in the main plugin window. This will open up a tab in your browser where Crowd Control will ask you to log in to thier service through your platform of choice. This authorization is saved in the plugin's settings, so you won't need to do this every time!

Once the plugin has been authorized, it'll show the name and platform of the account that provided the auth, and either begin listening for an active Crowd Control session, or load the game and details about an active session currently in progress

### Controlling VNyan
During an active Crowd Control Session, the plugin will fire semantically-named VNyan Triggers in response to incoming Crowd Control events.  At the same time, it will set semantically-named VNyan parameters as a way of passing in event-related information that can be used by the triggers in your node graphs.

#### Triggers

Here's the naming scheme for parameters and triggers that this plugin uses:

| Trigger Type        | Naming Scheme       | Example                   |
|---------------------|---------------------|---------------------------|
| Effect Request      | `_xcc_erq_*`        | `_xcc_erq_moneyup`        |
| Effect Success      | `_xcc_esc_*`        | `_xcc_esc_silvers`        |
| Effect Failure      | `_xcc_efl_*`        | `_xcc_efl_killplayer`     |
| Effect Retry        | `_xcc_ert_*`        | `_xcc_ert_arrowsup`       |
| Effect Refund       | `_xcc_erf_*`        | `_xcc_erf_bombsup`        |
| Timed Effect Begin  | `_xcc_teu_*_begin`  | `_xcc_teu_icephys1_begin` |
| Timed Effect Pause  | `_xcc_teu_*_pause`  | `_xcc_teu_ohko1_pause`    |
| Timed Effect Resume | `_xcc_teu_*_resume` | `_xcc_teu_infmag1_resume` |
| Timed Effect End    | `_xcc_teu_*_end`    | `_xcc_teu_swordup1_end`   |

#### Parameters

Parameters are names and set based on two factors, the type of event they are related to, and the type of information that they represent. You can use VNyan's built in "Monitor" feature to experiment and see how these are set in real-time.  The parameter names are constructed according to a structure that looks like this:

**\_xcc\_`event type`_`field name`**

The `event type` represents the type of Crowd Control event that it is related to, and can be any of the following:

| Trigger Type         | Identifier | Example             | Description                                                               |
|----------------------|------------|---------------------|---------------------------------------------------------------------------|
| Effect Request       | `erq`      | `_xcc_erq_effectID` | A requested game effect has been queued in Crowd Control                  |
| Effect Success       | `esc`      | `_xcc_esc_effectID` | A queued effect was suggessfully activated in the game!                   |
| Effect Failure       | `efl`      | `_xcc_efl_effectID` | A queued effect was unable to activate before expiring                    |
| Effect Retry         | `ert`      | `_xcc_ert_effectID` | A a previously-failed effect is being retried by Crowd Control            |
| Effect Refund        | `erf`      | `_xcc_erf_effectID` | An effect has had the coins paid for the effect refunded to the requester |
| Timed Effect Update  | `teu`      | `_xcc_teu_effectID` | A timed effect has changed state (either begin, pause, resume or end)     |

The `field name` described the piece of information that it represents, and can be any of the following:

| Field Name         | Identifier  | Example Name         | Example Value | Description                                                                           |
|--------------------|-------------|----------------------|---------------|---------------------------------------------------------------------------------------|
| Effect ID          | `effectID`  | `_xcc_erq_effectID`  | `moneyup`     | the indentifier of the Crowd Control effect                                           |
| Effect Name        | `name`      | `_xcc_esc_name`      | `Add Rupees`  | A user visible name of the Crowd Control effect                                       |
| Effect Quantity    | `quantity`  | `_xcc_efl_quantity`  | `69`          | For effects with a quantity, this is the amount requested                             |
| Effect Duration    | `duration`  | `_xcc_ert_duration`  | `60`          | For timed effects, this is the total duration of the effect                           |
| Effect Price       | `price`     | `_xcc_erf_price`     | `420`         | The price that was paid by the requester for this effect                              |
| Sender Name        | `sender`    | `_xcc_teu_sender`    | `jayo_exe`    | The username of the person who requested the effect                                   |
| Timed Effect State | `status`    | `_xcc_teu_status`    | `begin`       | For Timed Effect Updates, this is the new state that the effect has transitioned to   |
| Time Remaining     | `remaining` | `_xcc_teu_remaining` | `13`          | For Timed Effect Updates, this is the remaining amount of time before the effect ends |

### Trigger Browser and Trigger History

When an active Crowd Control sessio is running, the plugin window will populate two tabs to help make setting things up easier for you: the "Trigger Browser" and the "Trigger Histroy".

the Trigger Browser is a complete list of all of the game effects for the current Crowd Control session, allowing you to browse and copy triggers names for any effect and state, so you can more easily paste the triggers into your node graphs.

the Trigger History is a chronological list of all of the triggers sent by the plugin during this Session.  You can use this when testing effects directly to confirm that they are coming through in the plugin, and that your trigger names in the code graph are correct. You can also copy the trigger names here if you wanted to build responses to an effect on the fly!

## Example Resources

Included in each release is a zip file of sample resources to demonstrate how this plugin works.  These are intended to work for Crowd control sesions of _A Link to the Past_ and indluces these assets to get you started:

* VNyan Droppable and Throwable Items:
  * Egg
  * Bomb
  * Arrow
  * Magic Jar
  * Green Gem
* a "Trigger Map" node graph that assigns "Friendly" names to each of the important triggers for effects in this game
* a "Sample Interactions" node graph that provides concrete examples of a few different use-cases to give you some inspiration:
  * Instant Effects without a quantity
  * Instant Effects with a quantity
  * Timed Effects that can "toggle" some VNyan thing on when starting or resuming, and off when pausing or ending an effect
  * Timed Effects that involve some sort of "looping" behaviour that should keep going when the effect is active, but stop when it is paused or ends
 
Installing the sample stuff works in a similar way to the plugin, with a couple of extra steps:

1. Grab the Sample Assets ZIP file from the [latest release](https://github.com/jayo-exe/CrowdControlVNyanPlugin/releases/latest) of the plugin.
2. Extract the contents of the ZIP file _directly into your VNyan installation folder_.  This will add the demo assets files to your VNyan `Items\Throwables` and `Item\Droppables` folders, as well as adding a couple sample graph files to your VNyan `Items\Graphs` folder
3. Launch VNyan, open your Node Graphs, and import the LTTP Trigger Map from the `Items\Graphs` folder
4. Also import the "LTTP Demo Interactions" node graph, also found in the `Items\Graphs` folder.  Due to the nature of imported node graphs, **You'll need to select your desired throwable/droppable items for all of the nodes in this graph since it is likely that none will be selected**.  the sample items provided should be available for selection in the lists of each node.
5. Assuming that the Crowd Control plugin was previously set up, you should be able to start a crowd Control session for _A Link To The Past_, send one of the effects referenced in the Demo Interactions" graph to yourself, and see the associated actions happen inside VNyan!
6. Create your own actions and experiment with different games and effects!

## Development
(Almost) Everything you'll need to develop a fork of this plugin (or some other plugin based on this one)!  The main VS project contains all of the code for the plugin DLL, and the `dist` folder contains a `unitypackage` that can be dragged into a project to build and modify the UI and export the modified Custom Object.

It's worth noting that per VNyan's requirements, this plugni in built under **Unity 2020.3.40f1** , so you'll need to develop on this version to maintain compatability with VNyan.
You'll also need the [VNyan SDK](https://suvidriel.itch.io/vnyan) imported into your project for it to function properly.
Your Visual C# project will need to mave the paths to all dependencies updated to match their locations on your machine.  Most should point to Unity Engine libraries for the correct Engine version **2020.3.40f1**.

## Special Thanks

The team over at [Crowd Control](https://crowdcontrol.live) for thier cooperation, support, and for consistently raising the bar on what it means to be an interactive and engaging broadcaster.

Suvidriel for building and maintaining the [VNyan](https://suvidriel.itch.io/vnyan) VTubing platform, and the surrounding community. It's given me a cool place to make neat stuff and share it with nice people.
