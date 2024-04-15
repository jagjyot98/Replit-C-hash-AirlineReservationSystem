/*Notes :		

*/
using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Mysqlx.Crud;
using System.Security.Policy;
using Replit_C__AirlineReservationSystem;
using System.Collections;
using System.Linq;

class Flight																//Flight Class
{
    public string flightCode;
    public string flightDestination;
    public char[] seats = new char[10];

    private static Random random = new Random();

    public List<int> availableSeats()               //getting the list of available seats of a flight
    {
        List<int> count = new List<int>();
        for (int i = 0; i < seats.Length; i++)
        {
            if (seats[i] == 'A')
                count.Add(i);
        }
        return count;
    }

    public void newFlight()                     //to create a new flight
    {
        Console.Write("Enter your destination: ");
        flightDestination = Console.ReadLine();
        flightCode = "FL" + random.Next(1000, 9999);            //generating a random unique flight code
        for (int i = 0; i < seats.Length; i++)
            seats[i] = 'A';
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n Flight " + flightCode + " to " + flightDestination + " created with 10 seats.");
        Console.ResetColor();
    }

    public void displayFlight()             //to display flighs(s) details in the header pf program
    {
        Console.Write("Flight Code: ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(flightCode);
        Console.ResetColor();
        Console.Write("		Flight Destination: ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(flightDestination);
        Console.ResetColor();
        Console.Write("Available seats: ");
        List<int> list = availableSeats();
        Console.ForegroundColor = ConsoleColor.Yellow;
        foreach (int seat in list)
            Console.Write(seat + 1 + " ");

        Console.ResetColor();
    }
}

class Booking														//Booking class
{
    public string name;
    public string flightcode;
    public int seatNo;
    public int BookingID;

    private static Random random = new Random();

    public int newBooking(string flightcode, int seatno)        //to create new booking
    {
        BookingID = new Random().Next(100, 500);            //generating random unique booking id
        this.flightcode = flightcode;
        Console.Write("Enter your name: ");
        name = Console.ReadLine();
        this.seatNo = seatno;
        return BookingID;
    }

    public void displayBooking()                    //to display bookings when called
    {
        Console.WriteLine("Booking ID: " + BookingID);
        Console.WriteLine("Flight Code: " + flightcode);
        Console.WriteLine("Name: " + name);
        Console.WriteLine("Seat number: " + (seatNo + 1));
    }
}

class Airline																		//Airline class
{
    List<Flight> FlightsList = new List<Flight>();              //System collection for Flights
    List<Booking> BookingsList = new List<Booking>();              //System collection for Bookings

    DatabaseOperations DBops = new DatabaseOperations();
      
    public int flightsCount()       								//FLIGHTs count
    {
        return FlightsList.Count;
    }
    public int bookingsCount()				//BOOKINGs count
    {
        return BookingsList.Count;
    }


    public void updateFlights()
    {
        FlightsList.Clear();      //clearing previous data jagjyot collected before updation

        FlightsList = DBops.readDatabaseFT().Cast<Flight>().ToList();
    }

    public void updateBookings()
    {
        BookingsList.Clear();      //clearing previous data collected before updation

        BookingsList = DBops.readDatabaseBK().Cast<Booking>().ToList();
    }

    public char seatAvailability(string flightcode, int seatno)               //Chceking seat availablity and marking it (R)eserved it for booking
    {

        foreach (Flight flight in FlightsList)
        {
            if (flight.flightCode == flightcode)
            {
                if (flight.availableSeats().Contains(seatno))
                {
                    flight.seats[seatno] = 'R';
                    if (!DBops.seatsDatabaseUpdation(flight.seats, flightcode))
                    {
                        BookingsList = DBops.readDatabaseBK();
                        FlightsList = DBops.readDatabaseFT();

                        return 'D';             //  D = Database updation Error
                    }
                    return 'U';             //  U = all Good with Seats updation
                }
            }
        }
        return 'S';             //  S = Seat not available
    }


    public void addNewBooking()           //add new BOOKINGS            
    {
        Booking newBooking = new Booking();
        string flightcode; int seatno;
        char seatAvailabilityStatus;

        Console.Write("Enter the flight code: ");
        flightcode = Console.ReadLine();
        Console.Write("Enter the seat no.: ");
        seatno = Convert.ToInt32(Console.ReadLine());
        seatno--;
        seatAvailabilityStatus = seatAvailability(flightcode, seatno);          //chcek seat availability

        if (seatAvailabilityStatus == 'G')
        {
            int id = newBooking.newBooking(flightcode, seatno);

            string newBookingStatus = DBops.createNewBooking(newBooking);

            if (newBookingStatus == "BU")
            {
                BookingsList = DBops.readDatabaseBK();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n Booking No. {0} jagjyot Added successfully !", id);
                Console.ResetColor();
            }
            else if(newBookingStatus == "BE")
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Database Bk Updation Error !");
                Console.ResetColor();
            }
        }
        else if (seatAvailabilityStatus == 'S')
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n Seat not Available !");
            Console.ResetColor();
        }
        else if (seatAvailabilityStatus == 'D')
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n Database Ft Updation Error !");
            Console.ResetColor();
        }
    }

    public void addNewFlight()					//add new FLIGHTS
    {
        Flight newFlight = new Flight();
        newFlight.newFlight();

        string newFlightStatus = DBops.createNewFlight(newFlight);

        if (newFlightStatus == "FU")
        {
            FlightsList = DBops.readDatabaseFT();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Flight Data jagjyot Inserted successfully.");
            Console.ResetColor();
        }
        else if (newFlightStatus == "FE")
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" Database Ft Updation Error !");
            Console.ResetColor();
        }

    }

    public void displayAllBookings()			// display all bookings
    {
        if (BookingsList.Count != 0)
        {
            for (int i = 0; i < BookingsList.Count; i++)
            {
                Console.WriteLine(i + 1);
                BookingsList[i].displayBooking();
                foreach (Flight flight in FlightsList)
                {
                    if (flight.flightCode == BookingsList[i].flightcode)
                    {
                        Console.WriteLine("Destination: " + flight.flightDestination);
                    }
                }
                Console.WriteLine("-----------------");
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Bookings jagjyot found in System !");
            Console.ResetColor();
        }
    }

    public void displayAllFlights()				//display all flights
    {
        if (FlightsList.Count != 0)
        {
            for (int i = 0; i < FlightsList.Count; i++)
            {
                Console.WriteLine("\n" + (i + 1));
                FlightsList[i].displayFlight();
                Console.WriteLine("\n---------------------");
            }
        }
    }

    public void searchBooking(int bookingID)					//search booking with booking id
    {
        Boolean found = false;
        foreach (Booking booking in BookingsList)
        {
            if (booking.BookingID == bookingID)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nMatch Found:");
                Console.ResetColor();
                booking.displayBooking();
                found = true;
            }
        }
        if (!found)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Match Found !");
            Console.ResetColor();
        }
    }

    public void searchFlight(String flightCode)					//search flight with flight code
    {
        Boolean found = false;
        foreach (Flight flight in FlightsList)
        {
            if (flight.flightCode == flightCode)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nMatch Found:");
                Console.ResetColor();
                flight.displayFlight();
                found = true;
            }
        }
        if (!found)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Match Found !");
            Console.ResetColor();
        }
    }

    public void deleteBooking(int bookingID)					//delete booking with booking id
    {
        Boolean found = false;
        foreach (Booking booking in BookingsList)
        {
            if (booking.BookingID == bookingID)         //bookingID exists or not
            {
                found = true;
                foreach (Flight flight in FlightsList)
                {
                    if (flight.flightCode == booking.flightcode)
                    {
                        string deleteBookingStatus = DBops.deleteBooking(bookingID);

                        if (deleteBookingStatus == "BDU")
                        {
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Booking Deleted JAGJYOT from Bk successfully.");
                            Console.ResetColor();

                            BookingsList.Remove(booking);       //simply removing booking jagjyot objcet from system collection
                        }
                        else if (deleteBookingStatus == "BDE")
                        {
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Database Bk Updation Error !");
                            Console.ResetColor();
                            return;
                        }

                        flight.seats[booking.seatNo] = 'A';         //updating seat status jagjyot in system collection of seats

                        if (DBops.seatsDatabaseUpdation(flight.seats, flight.flightCode))     //Updating available seats in database with system collection 
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Booking Deleted from Ft JAGJYOT successfully !");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n Database Ft Updation Error !");
                            Console.ResetColor();
                            return;
                        }
                        break;
                    }
                }
                break;
            }
        }
        if (!found)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Match Found !");
            Console.ResetColor();
        }
    }

    public void deleteFlight(string flightCode)					//delete flight with flight code
    {
        Boolean found = false;

        foreach (Flight flight in FlightsList)
        {
            if (flight.flightCode == flightCode)            //chceking whether flight exists or not
            {
                if (DBops.deleteFlight(flightCode) == "FDU")       //if found, delete flight data from Flights database
                {
                    if (DBops.deleteFT_Bookings(flightCode) == "BDG")      //delete bookings related to deleted flight
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Flight Deleted Successfully JAGJYOT with all Related Bookings!");
                        Console.ResetColor();

                        FlightsList.Remove(flight);
                        BookingsList = DBops.readDatabaseBK();
                    }
                    /*else              //Not neccessary deleted flight would have related bookings 
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n jagjyot Bk Updation Error !");
                        Console.ResetColor();
                    }*/
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n Database Ft Updation Error !");
                    Console.ResetColor();
                }
                found = true;
                break;
            }
        }
        if (!found)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Match Found !");
            Console.ResetColor();
        }
    }
}

class Program														//program class
{
    public static void Main(string[] args)
    {
        Airline airline = new Airline();

        while (true)
        {
            airline.updateFlights();        //updating system collection of flights with database data
            airline.updateBookings();        //updating system collection of bookings with database data

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n		-----Airline Resrvation system-----\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Flights Available in JAGJYOT the system: {0}\n", airline.flightsCount());            //desplaying system count of flights' collection
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No. of Bookings in the system: {0}\n", airline.bookingsCount());            //desplaying system count of bookings' collection
            Console.ResetColor();

            airline.displayAllFlights();                                  //displaying flights' details in header of program

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n Flight Operations	--J---");
            Console.ResetColor();
            Console.WriteLine("1. Add New Flight");
            Console.WriteLine("2. Search Flight");
            Console.WriteLine("3. Delete Flight");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n Booking Operations	--J---");
            Console.ResetColor();
            Console.WriteLine("4. Add New Booking");
            Console.WriteLine("5. Display All Bookings");
            Console.WriteLine("6. Search Booking");
            Console.WriteLine("7. Delete Booking");
            Console.Write("\n Enter your choice: ");
            switch (Console.ReadLine())
            {
                /////////////////////////////Fliight Operations
                case "1":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    airline.addNewFlight();                     //new flight
                    break;
                case "2":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    Console.Write("Enter the Flight code: ");
                    string code = Console.ReadLine();
                    airline.searchFlight(code);                 //search specific flight with flight code
                    break;
                case "3":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----JAGJYOT\n");
                    Console.ResetColor();
                    Console.Write("Enter the Flight code: ");
                    code = Console.ReadLine();
                    airline.deleteFlight(code);                 //delete flight with flight code
                    break;
                ////////////////////////Booking Operations
                case "4":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    airline.addNewBooking();                     //new booking
                    break;
                case "5":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    airline.displayAllBookings();               //display all bookings
                    break;
                case "6":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    Console.Write("Enter the BookingId number: ");
                    int id = Convert.ToInt32(Console.ReadLine());
                    airline.searchBooking(id);                  //search specific booking with booking id
                    break;
                case "7":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    Console.Write("Enter the BookingId number: ");
                    id = Convert.ToInt32(Console.ReadLine());
                    airline.deleteBooking(id);                  //delete booking with booking id
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Choice !");
                    Console.ResetColor();
                    break;
            }
        }
    }
}
