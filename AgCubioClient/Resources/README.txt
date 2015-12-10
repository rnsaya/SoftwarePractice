I certify that the work to create this GUI was done entirely by myself and my partner - Ken Bonar, Rachel Nicole Saya 
README created for CS 3500, November 2015

// Revision history:  
//   Version 1.0 11/03/15 10:05 a.m.   External Resources and GUI features added to README
//	 Version 1.1 11/04/15 3:18  p.m.   Design Decisions and Implementations Notes added to README
//   Version 1.1 11/04/15 4:16 p.m	   Added GUI Testing Notes


AgCubio README:
<summary>

---------------------------------------------
External Resources, Projects, and Sub-Moduals
---------------------------------------------

	1. The network implementation used a fair amount of the demonstration implementation from the MSDN Asynchronous Server
	link that is found in the commented code. The stopwatch code was designed using suggestions from Stackoverflow, which is also
	in the comments.

-------------------------
GUIs Additional Features
-------------------------

	1. The ability to relaunch and enter the world is something we are particularly proud of. Most other groups do not have this functionality.
	It also resizes 

----------------
Design Decisions
----------------

	1. We chose to update the GUI when the mouse moved instead of on a timer. We expect around 100 FPS consistently throughout the game.

	2. The design of the project was to implement speed and quick redraws. For this, we decided to use a dictionary that the 
	cubes would be stored in, and iterate through that dictionary to draw the objects. We attempted to minimize redudant code
	as well which contributed to our speed in drawing.



--------------------
Implementation Notes
--------------------

	1. Our implementation of the client uses a Model which consists of a World and Cube class. These store the cubes which are read from
	the server. The Controller is the Form.cs, which dictates how the server interacts with the UI. The UI is designed in the Form.designer.
	Cs file. Tests were designed and attained 100% code coverage. 

	The code for the FPS is implemented, but it is not refreshing as it is supposed to.


</summary>