## Reset Camera

This plugin will reset the Camera to a saved position relative to the world, and NOT relative to the character.

This is useful for activities where you want your camera to be synced up with the EXACT same coordinates across all clients at the press of a button. Namely, multiboxing.

## Install

Add `https://raw.githubusercontent.com/Vueren/ResetCamera/main/repo.json` to your list. Enjoy.

## How To Use

### Saving the Camera Position

Type /rcsave to save the current camera position for resetting. I'd advise a setting looking at True North, top/down.

You can refine these values in the ResetCamera.json file in the AddonSettings folder.

For True North looking *almost* top-down:
- "Distance": 20.0,
- "HRotation": 0.0,
- "VRotation": -0.75,

### Resetting the Camera Position

Type /rcreset to reset the camera position to the saved position. This may be used in a macro to assign a keybind to it.

## Enjoy!

Thanks for using this plugin! It's my very first one for FF14.

## Change Log

- **1.0.0.3**
  - Update to API 12 for 7.2

- **1.0.0.2**
  - Update to API 11 for 7.1

- **1.0.0.1**
  - Update to API 10 for 7.0
  - Use FFXIVClientStructs camera instead of a static address 

- **1.0.0.0**
  - Initial Release
