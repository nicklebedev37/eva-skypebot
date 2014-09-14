eva-skypebot
============

A skypebot which automates access to the internal office facilities (like jira, build systems, etc.). Can be easily extended to support new feature since follows an extendable architecture.

A Typical usage of this skypebot
============

1. Accessing to the build system (e.g. cruise control) and printing status of the buidls
   you: eva cc
   eva: firstProject: success
        secondProject: failure
        
2. Hinting in the chat about mentioned bug/task
   you: hey, what's about bug BG-1234
   eva: BG-1234 'Eva incorrectly handles messages about Jira'
        status: opened, assignee: nick.l
        
3. Quick executing simple js code:
   you: js var x = 3; x * 2 + 3; x
   eva: 9
   
How to build and deploy
============

1. Register a skype account for the bot and put its id to the 'src\Skypebot.Core.Test\app.config' and 'src\Skypebot.ConsoleInterface\app.config' into botskypeid entry.

2. Change database username and password to the appropriate ones in the 'src\Skypebot.ConsoleInterface\app.config'.

3. Create empty database with the name 'evadb'

4. Execute 'evadb.sql' over this db.

5. Fill the 'evadb_populatedata.sql' with the appropriate data (following the example given in this file) and execute over this db.

6. Refresh the dbcontext item via visual studio in the DataAccessLayer project.

7. Run the build.bat file from the build folder.

8. Open skype app under you test account registered in the step 1, run the 'Skypebot.ConsoleInterface.exe' from the ouput folder.

9. Skype may ask you to allow access for 3rd party components - of course allow :)

It's all, now skype bot should reply to your commands, good luck!
