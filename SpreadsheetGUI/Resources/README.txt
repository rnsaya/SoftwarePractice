I certify that the work to create this GUI was done entirely by myself and my partner - William Nate Merrill, Rachel Nicole Saya 
README created for CS 3500, November 2015

// Revision history:  
//   Version 1.0 11/03/15 10:05 a.m.   External Resources and GUI features added to README
//	 Version 1.1 11/04/15 3:18  p.m.   Design Decisions and Implementations Notes added to README
//   Version 1.1 11/04/15 4:16 p.m	   Added GUI Testing Notes


PS6 Version 1.6 README:
<summary>

---------------------------------------------
External Resources, Projects, and Sub-Moduals
---------------------------------------------

	1. SpreadsheetPanel, written by Joe Zachary, modified by Nate Merrill and Rachel Saya, is a GUI tool for representing the cells of the spreadsheet.
	2. Spreadsheet Version 1.1 represent the inner workings of the spreadsheet GUI.
	3. AbstractSpreadsheet Version 1.7, written by Joe Zachary, is used to represents the state of a simple spreadsheet as an abstract class.
	4. Formula Version 1.2 from PS3 is used to represent formulas of cells. 
	5. DependencyGraph Version 1.0 from PS2 is used to keep track of relationships between cells

-------------------------
GUIs Additional Features
-------------------------

	1. Any cell that is updated as a result of a formula or input is briefly highlighted until the user clicks away.
	2. There is a search fuction at the top of the GUI that allows user to navigate to a specific cell.
	3. User can navigate spreadsheet with the arrow keys.
	4. GUI displays the title of the spreadsheet, if it is a new spreadhsheet without a name the default title is "Spreadsheet".
	5. Added shortcut ctrl + S, which checks to see if the user needs to save and prompts if the spreadsheet needs to be saved.
	6. Added shortcut ctrl + n, which creates a new spreadsheet window.
	7. Added shortcut ctrl + o, which opens a new file that replaces the current window.

----------------
Design Decisions
----------------

	1. When a user tries to go to a cell that does not exist, we chose to have a message box that says it's invalid and cancels the action.
	2. If the GUI catches an Argument exception, it is only detecting an undefined variable. Check the "ignore this exception"
		checkbox, and then click continue. This only applies for when the GUI is running in Debug mode.
	3. The user can navigate the GUI with the arrow keys. However, it does not update the scrollbar.
	4. Our Help menu is used by navigating through categories until the user gets to the topic they are interested in. Once that is clicked a message
		box is displayed providing the requested information.

--------------------
Implementation Notes
--------------------

	1. In the spreadsheet GUI, the evaluation/update of cells is done in the background and when it is completed the GUI is updated.
	2. SpreadsheetPanel project was added to our solution to add additional functionality. Certain modifications required alterations to SpreadsheetPanel
	NOTE: Original implementation notes can be found at: http://www.learning2.eng.utah.edu/mod/assign/view.php?id=15703		

--------------------
GUI Testing Notes
--------------------
	1. For GUI testing we have been making complete assertions and recording to test the GUI. However, the tests someimes miss the mark and throw exceptions.
	2. Matt (The TA) stated on Octorber 3rd, 2015 in help hours that as long as our assertions are sound, even if the GUI Tests do not work properly on execution,
	   we should still be getting our points.

</summary>