# Airline Reservation System
The Airline Reservation System is a self-project developed out of curiosity to learn and experience the realms of C# language and Server Side Systems. The project was initially started on an online repository of Replit so as the name.

## Project Structure
  ### Booking Class
- #### Data Attributes
  - `BookingID` A random and unique booking ID is generated by the system every time a new booking is made.
  -  `FlightCode` The flight code for which the booking was made.
  -  `Name`  The name of the person for whom the booking is made.
  -  `SeatNo` The specific seat number the user booked for the flight.
- #### Methods
  - `newBooking(flightcode, seatno)` This method is called every time the user chooses to generate a new booking.
  - `displayBooking()` This method is called every time the user chooses to view details of a specific or all the bookings in the system.

### Flight Class
- #### Data Attributes
  - `FlightCode` The system generates a random and unique flight code every time a new flight entity is made.
  - `Flight Destination` The destination to which the flight is bound.
  - `Available Seats[char]` A collection of 10 seats predefined as (A)vailable. Whenever a seat is booked, it will be marked as (R)eserved. The system will only display the list of seats marked as (A)available for users to choose from.
- #### Methods
  - `List<int> availableSeats()` This method will filter out a list of seats marked as (A)available.
  - `newFlight()` This method is called every time the user chooses to generate a new flight entity.
  - `displayFlight()` This method is called in every iteration of the user session to display available flight details. It is also called whenever a user chooses to view details of a specific flight.

### Airline Class
- #### Data Attributes
  - `FlightsList` A collection of flights currently available in the system. 
  - `BookingsList` A collection of bookings currently available in the system.
  - The above collections are used for local processing and viewing of data and are updated in every iteration of a session.
 
  - `DBops` An object to class `DatabaseOperations` to access the database CRUD and traversing operations.
- #### Methods
  - `int flightsCount()` This method returns the count of flight entities available in the system.
  - `int bookingsCount()` This method returns the count of all booking entities available in the system.
  - `updateFlights()` This method accesses the `readDatabaseFT()` operation of the `DatabaseOperations` class to clear the local system collection of flights and update it with fresh data from the database.
  - `updateBookings()` This method accesses the `readDatabaseBK()` operation of the `DatabaseOperations` class to clear the local system collection of bookings and update it with fresh data from the database.
  - `char seatAvailability(flightcode, seatno)` This method is called every time a new booking is made. It checks the availability of the seat number for the user-specified flight code. After seat availability is ensured, it accesses the `createNewBooking(newBooking)` operation in `DatabaseOperations` to handle the database updation.
  - `addNewBooking()` This is the first point of interaction for the users whenever they want to create a new booking. From here, a new Booking class entity will be made, asking users to enter the required details, flight code and seat availability will be ensured, and then the database updation will be called. 
  - `addNewFlight()` This is the first point of interaction for the users whenever they want to create a new flight entity. A new flight class entity will be made from here, asking users to enter the required details. Then, the database update will be called. 
  - `displayAllBookings()` This method is called whenever the user chooses to view all the bookings available in the system.
  - `displayAllFlights()` This method displays all the flight entities available in the system with their respective details.
  - `searchBooking(bookingID)` This method searches for a specific booking in the system using its unique 'bookingID'.
  - `searchFlight(flightCode)` This method searches for a specific flight in the system using its unique 'flightCode'.
  - `deleteBooking(bookingID)` This method searches for a specific booking and deletes its data from the whole system.
  - `deleteFlight(flightcode)` This method searches for a specific flight and deletes its data from the whole system. It also updates bookings according to available flights in the system.

### DatabaseOperations Class
- #### Data Attributes
  - `connectionString` MySQL connection string
  - `FTList` A Flight type list collection to handle flights data for database management operations.
  - `BKList` A Booking type list collection to handle bookings data for database management operations.
- #### Methods
  - `readDatabaseFT()` This method reads the updated flights' data from the database and updates the system's local collection.
  - `readDatabaseBK()` This method reads the updated bookings' data from the database and updates the system's local collection.
  - `createNewBooking(newBooking)` This method adds a new booking entry to the database.
  - `createNewFlight(newFlight)` This method adds a new flight entry to the database.
  - `SeatsDatabaseUpdation(seatsUpdated[], flightcode)` This method updates the seats' status in the database after every update in the collection of the seats for respective flight.
  - `deleteFlight(flightcode)` This method deletes an entry of a specific flight in the database.
  - `deleteBooking(bookingID)` This method deletes an entry of a specific booking in the database.
  - `deleteFT_Bookings(flightcode)` This method deletes bookings related to the respective deleted flight.

### DBconsts
- #### Data Attributes
  - `Server` Holds the server
  - `Database` Holds the database name
  - `Uid` Holds the username or user id for the database.
  - `Pwd` Holds user-specific password for the database.
  - `tableF` Holds table name for flights table.
  - `tableB` Holds table name for bookings table.
- #### Methods
  - `returnConectionString()` Returns MySQL connection string for database connectivity.
  - `readFlightsQuery()` Generates and returns SQL query to read data from flights table.
  - `readBookingsQuery()` Generates and returns SQL query to read data from bookings table.
  - `createNewFlightQuery(newFlight)` Generates and returns SQL query to create a new entry for flight entity.
  - `createNewBookingQuery(newBooking)` Generates and returns SQL query to create a new entry for booking entity.
  - `seatsDatabaseUpdateQuery(seatsUpdated[], flightcode)` Generates and returns SQL query to update flight seats.
  - `deleteBookingQuery(bookingID)` Generates and returns SQL query to delete an entry from bookings table.
  - `deleteFlightQuery(flightcode)` Generates and returns SQL query to delete an entry from flights table.
  - `deleteFT_BookingsQuery(flightcode)` Generates and returns SQL query to delete bookings related to a deleted flight.