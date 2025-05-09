## Reset Camera

This plugin will reset the Camera to a saved position relative to the world, and NOT relative to the character.

This is useful for activities where you want your camera to be synced up with the EXACT same coordinates across all clients at the press of a button. Namely, multiboxing.

## Install

Add `https://raw.githubusercontent.com/Vueren/ResetCamera/main/repo.json` to your list. Enjoy.

## How To Use

### Saving the Camera Position

Type /rcsave to save the current camera position for resetting.

### Tweaking the Camera Position

Type /rcconfig to further tweak the camera position data.

I'd advise a setting looking at True North.

HRotation Degrees can go from 0 to 360.
This is the cardinal direction (e.g. North).
0 and 360 points North and 180 points South. 90 points West and 270 points East.

VRotation can go from 0.79 to -1.49.
This is the vertical direction.
-1.49 points top/down and 0 points horizontally.

For True North looking 45 degrees downward:
- "Distance": 20.0,
- "HRotation Degrees": 0.0,
- "VRotation": -0.75,

"Roll Degrees" is included as a joke. Feel free to have fun with it.

### Resetting the Camera Position

Type /rcreset to reset the camera position to the saved position. This may be used in a macro to assign a keybind to it.

## Enjoy!

Thanks for using this plugin! It's my very first one for FF14.

## Change Log

- **1.0.1.0**
  - Added a Config window! Open with /rcconfig
  - The following options will NOT have a Config UI, but will remain behind-the-scenes for the sake of backwards compatibility:
    - ZoomFoV
    - GposeFoV
    - Pan (seemingly does nothing)
    - Tilt (seemingly does nothing)
  - Updated this README's tutorial section

- **1.0.0.3**
  - Update to API 12 for 7.2
  - (Also finally fixed the icon image)

- **1.0.0.2**
  - Update to API 11 for 7.1

- **1.0.0.1**
  - Update to API 10 for 7.0
  - Use FFXIVClientStructs camera instead of a static address 

- **1.0.0.0**
  - Initial Release
