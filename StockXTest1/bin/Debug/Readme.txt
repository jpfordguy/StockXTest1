The code comtained in this compressed file should rin right from Windows Explorer.  It *should* work if you have .NET 4.6.1 installed.  If not, then get 
Visual Studio 2017 Community Edition (as a minimum), and this should install any WCF stuff you'd need to run the application.

I downloaded ind installed Postgres on my PC, and set up a server just using localhost, and the default port.  When run, the application auto-generates a 
database called "stockxtest".  If you need thnis changed, then you'll have to modify the _dbName constant in PostgresInterface, and DatabaseInitializer classes.
Then simply do a Clean and Rebuld from the Build menu on Visual Studio.  The actual database connection properties can be found in the settings.xml file in the
same directory as StockXTest1.exe.

This application needs to run under administative provlideges - not sure why.  I used to do WCF with NetTcp bindings without it.  Either way, you only need to 
run StockXTest1.exe by right-clicking on the application, and selecting "Run as administator" from the menu.

You will need to download Postgres 10.1 for the database.  I installed the Ngpsql .NET libraries, though I really didn't need to because I could get them with
NuGet.  I've added comments in the code about how I went about things.  These are at the top of the individual source files (.cs files) in the project.  Please
refer to those notes for more detailed informnation.

Here are the commands I used for updating the database and reading back the average.  As per your instructions, I got the values you specified - with one exception.
The second step - adding a "2" to the list of numbers - gave me 2.53333333333332.  I'm assuming this is a database rounding issue as I used the Ave() function.

http://localhost:8000/AddShoeSize?name=Yeezy&size=1
http://localhost:8000/GetAverageSize?name=Yeezy

Let me know if you need anything else.