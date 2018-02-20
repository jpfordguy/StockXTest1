using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;


// This is a simplified HTTP "web service" that compolies with the test requirements.  I've used WCF as the transport mainly because
// Running a console application with administrative rights is requires much less installation and instructions than the alternative.
// Since the HTTP channel typically uses SOAP, while you may use HTTP paraneters to send the data, the return value of the call is 
// a SOAP document.  It is what it is.
//
// There is a file in the same directory as the application called settings.xml - which holds the database settings.  If you need to 
// configure this application for database connectivity, modify that file.  Essentially, this works using a the localhost address on 
// port 8000 - that part isn't configurable.  So whatever HTTP GET commands you use must do this locally.  The commands I used are as 
// follows:
// http://localhost:8000/AddShoeSize?name=Yeezy&size=1
// http://localhost:8000/GetAverageSize?name=Yeezy
//
//  And they obtained the desired results of the test.
//
// Full disclosure: I am not a web expert.  I know this isn't how it's done professionally.  This was a simple test.  This is the 
// first time I've ever used Postgres as a database, or even made a web service like this (I;ve done some others but they weren't
// production-grade).  If this was a web shop, I'd use a configuration script for the DB configuration, anf probably make this a 
// full-blown REST service, given that's the general trend things are going.  But as you can plainly see, this isn't the first
// time I've done SQL, and I'm not exactly a slouch with communications either.


namespace StockXTest1
{
    class Program
    {
        static void Main(string[] args)
        {
            //DatabaseHelper.SaveSettings();
            if(!DatabaseHelper.LoadSettings())
            {
                Console.WriteLine("ERROR: Unable to load application settings!");
                Console.WriteLine("Press enter to quit...");
                Console.ReadLine();
                return;
            }

            if (!DatabaseInitializer.CreateDatabase(DatabaseHelper.Server, DatabaseHelper.Port, DatabaseHelper.UserId, DatabaseHelper.Password))
            {
                Console.WriteLine("ERROR: Unable to create database!");
                Console.WriteLine("Press enter to quit...");
                Console.ReadLine();
                return;
            }
            try
            {
                DatabaseHelper.DatabaseInterface = new PostgresInterface(DatabaseHelper.Server, DatabaseHelper.Port, DatabaseHelper.UserId, DatabaseHelper.Password);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                Console.WriteLine("Press enter to quit...");
                Console.ReadLine();
                return;
            }
            WebServiceHost host = new WebServiceHost(typeof(ShoeService), new Uri("http://localhost:8000/"));
            ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IShoeService), new WebHttpBinding(), "");
            host.Open();
            Console.WriteLine("Service is running");
            Console.WriteLine("Press enter to quit...");
            Console.ReadLine();
            host.Close();
        }
    }
}
