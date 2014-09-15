eva-skypebot
============

A skypebot which automates access to the internal team facilities (like jira, build systems, etc.). Can be easily extended to support new feature since follows an extendable architecture.

Why the project was started
============

Many development teams usually have lots of internal resources like build server, bugtracker, time management system and etc. These resource are very convinient themselves (otherwise they wouldn't have been used) but there is one big inconvinient thing about them: there is no single point of access (Some resources are managed via browser app, some have desktop tray app).
The purpose of this project is to address such issue. The skypebot will provide 'command line' style access to each of the resources (for more see next section). You may ask: why skypebot and not one more app? My answer is: the skype is the usual communicator within teams, it is ported to many platforms (including mobile), it doesn't require any vpn, tunnel or other transport tweaks. So it's very popular and widespread.

A typical usage of this skypebot
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

4. In case you've setup build system skypebot will notify you about build failures/fixes in the following manner:
   eva: the build 'MyAwesomeProject' has been broken. Breaker: Nick.L

5. Print random person from the current chat (E.g. you need to choose who will deploy build to production :)):
   you: eva randomguy
   eva: nick.l
   
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

How to extend and add new command:
============

1. To add new command you need:
   create new class YouNewCommand
   inherit it from the AbstractCommand
   add YouNewCommand into the constructor of CommandManager

2. To add new notification you need:
   To be added

For now it's not very convinient to manage command, it should be after issue #1 is done. 
