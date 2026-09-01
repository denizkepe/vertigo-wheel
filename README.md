# Vertigo Wheel

A Unity wheel game where the player spins to collect rewards.

## Gameplay

The player can spin the wheel to win rewards.

- Normal zones include rewards and a bomb.
- If the player gets the bomb, collected rewards are lost.
- Every 5th zone is a Safe Zone with no bomb.
- Every 30th zone is a Super Zone with special rewards.
- The player can collect rewards and leave the game at any time.
- The player can use Revive after getting a bomb.
- Bomb probability changes based on zone progression and fail streak.
```text
     Each wheel slice has a weight value. Before the wheel spins, the bomb slice weight is updated with this logic:
        Final Bomb Weight =
        Base Bomb Weight × Zone Multiplier × Mercy Multiplier
    We can adjust how zone multiplier change (lineer, exp. or custom calculations)
```
- The player can see what items can be collected at the top of the screen.
- Rewards are randomized and higher value poolis on every spin. 

## Editable Content

Wheel rewards can be changed from the Unity Inspector.

There are three wheel configurations:

- `Wheel_Normal`
- `Wheel_Safe`
- `Wheel_Super`

## Supported Aspect Ratios

The UI was tested on:

- 20:9
- 16:9
- 4:3

## APK

You can download the Android APK from the GitHub Release page:

[Download APK](https://github.com/denizkepe/vertigo-wheel/releases/tag/v1.0.0)
