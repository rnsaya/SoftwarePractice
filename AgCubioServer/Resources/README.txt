I certify that the work to create this GUI was done entirely by myself and my partner - Dyllon Gagnier - 00708264,
Rachel Nicole Saya - 00852330
README created for CS 3500, November 2015

 Revision history:  
   Version 1.0 12/03/15 6:02 p.m.   PS8: AgCubio Server allows connection from multiple clients and manages the
									rules of the game. 
   Version 2.0 12/08/15 4:30 p.m.	PS9: AgCubio Server augmented to utilize a database to store the history of
									games played. Server is able to report this information to a web browser.


----------------------------------------------------------------------------------------------------------------------------
											     PS9 README: Database Description
----------------------------------------------------------------------------------------------------------------------------

<summary>

-----------------
Table Descriptions
-----------------

	1. Descriptions of the tables contained in your database

----------------------
Representative SQL Code
----------------------

	1. representative SQL code corresponding to each query 
		(i.e., inserts, updates, and removes) performed on the database by your server.


Software Engineering Tip: It is very important to document the DB relationship to a program
as this information can easily be lost and is harder to "infer" from the code.

</summary>


----------------------------------------------------------------------------------------------------------------------------
															PS8 README
----------------------------------------------------------------------------------------------------------------------------

NOTE: PLEASE RUN AGCUBIO IN RELEASE MODE. Running AgCubio in Debug mode causes performance issues.

---------------------------------------------
External Resources, Projects, and Sub-Moduals
---------------------------------------------

	1. PS8 server branch builds off of the work created by Dyllon Gagnier and Ross DiMassimo for PS7.
	Details on their work can be found at the bottom of this text file.

	2. Periodic Task Executor: This class regulates the heartbeat of the game. It works on scheduled intervals

-------------------------
Game Play Design Decisions
-------------------------

	1. Food: The amount of food we add per heartbeat relies on how many players are in the game. 
	   Our algorithm takes in the number of players and multiplies it by 2. That regulates the amount of food
	   added so that more food is added when more players are in the game. This way there doesn't end up being
	   an advantage for players already in the game and leaves food for new players. 

	2. Virus Strategy: If the player is big enough and hits a virus, it eats the virus and splits into three smaller cubes.
	   The amount of virus's in the world and their size is controllable. All virus's are green. No other cubes in the world
	   are green except virus's.

----------------
Server as Exemplar
----------------

	1. Our server initializes the world before the player cube is added. This prevents the player cube from being eaten
	   or moving while the world is sending the initial state.

	2. Our constants can be found in a seperate class called "GlobalConstants". Our constants are set to read only variables.

	3. Our split momentum is based off of the cube size. The larger the cube, the more momentum it has when
	   it splits. This is the opposite of the normal speed in the game, where the smaller you are, the faster
	   you move.	

	4. We gracefully disconnect. Our server cleans up the list of connections as soon as a send or receive fails.


----------------
Testing
----------------

	1. We tested our code thoroughly while developing. Since it is a GUI we played our game multiple times and tried
	   testing as many cases as possible. We also created a test class called "ServerModelTests" which tests our Model
	   project. We also ran our old tests to see what we already had code coverage for. We us a large amount of helper
	   functions in our development process, to get 100% code coverage we focused on testing the important
	   methods, that in turn, tested the helper methods.


----------------------------------------------------------------------------------------------------------------------------
															PS7 README
----------------------------------------------------------------------------------------------------------------------------

Coders9001 - 20151104
PS7 - AgCubio Client

I certify that the work to create this GUI was done entirely by myself and my partner:
	Ross DiMassimo - 00585028, Dyllon Gagnier - 00708264
This code is licensed under the GPL v2.0

Libraries:
	Newtonsoft JSON library v7

User Guide:
	To play AgCubio, first ensure that there is a running server and obtain the internet address of that
	server. Once you have that information and the server is running, launch the client application.

	Once the client is up, input the server name from before and a name that you would like your cube
	to be known as, then click login.

	To play AgCubio, move your mouse to direct your cell to where you would like it to go. If your cube
	is big enough compared to another, your cube will eat the other cube if it gets close enough
	(and vice versa).

	You can split your cell for a speed up by hitting the spacebar. This will also shoot off a portion of
	your mass (which can and is intended to be used for strategical manuvers).
	
	Once your cell is eaten by another, game over! But don't worry, you can always play again! 

Design Decisions:
	We decided to have our view talk very little to the NetworkController because it seemed simpler to
	instead have the NetworkController manipulate the model and then have the model signal the GUI when
	it needed to change. This has numerous advantages including keeping the networking code mostly separated
	from the GUI as well as from the Model (the network only talks to the model, not vice versa) as well
	as making the GUI more performant since it only changes when prompted to by the model indirectly by
	the network.

	Our World class with its callback system is also flexible enough that in later work (on the server), it
	should be possible to mostly keep the same model, have a state machine that manipulates the model, and
	then propogate those changes using the same event model that is used to update the GUI.