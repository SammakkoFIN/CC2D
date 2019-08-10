# Character Controller 2D for Unity
![CC2D](https://github.com/SammakkoFIN/CC2D/blob/master/GIF/CC2D.GIF)

Character Controller 2D contains scripts, which uses custom collision detection (Physics2D.BoxCast) for 2D platformer games.
CharacterController2D.cs can handle only basic collisions, but not slopes. PlayerController.cs takes movement and jump inputs and applies them to CC2D script. Character Controller 2D gets its inspirations from Sebastian Lague and Unity 2D platformer tutorials.

# How to use it
* Choose a game object you wish to control via scripts
* Add Box Collider 2D component
* Add CharacterController2D and PlayerController scripts
* Modify public variable 'Collision Mask' in CharacterController2D, adding layers the player can collide with

**It is recommended that you enabled 'Auto Sync Transforms' from Physics 2D settings to make sure script works without problems.**

# Coming features
* Slopes
* Moving Platform

and some more features in the future.

# LICENSE
Check the LICENSE file.
