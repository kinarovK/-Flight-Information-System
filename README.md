# Flight Information System
## About project
Flight Management Application is a system for managing flight data. The application consists of two parts:

FlightStorageService (backend) - REST API for managing flight data (CRUD operations).

FlightClientApp (frontend) - a client web application that interacts with the backend and allows users to manage flights through a user-friendly UI.

## Installation && Usage

1. Download app components from [gitHub](https://github.com/kinarovK/-Flight-Information-System) or clone project

```bash
git clone https://github.com/kinarovK/-Flight-Information-System.git
```
2. Go to the FlightStorageService directory.

3. Configure the database connection string (connectionStrings in appsettings.json):

```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FlightsDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```


To add test data for testing you can use script from [flights.sql](https://github.com/kinarovK/-Flight-Information-System/blob/main/Flights.sql). 

Firstly you need run the FlightStorageService (backend), after you can run the FlightClientApp (frontend) and use the Application.



## Usage

## Operations:

### Add a new flight:

Click the Add New Flight button on the home page, fill out the form, and click Create Flight.

### Delete a flight:

Click the Delete button on Flight Details page.

### Update a flight:

Click the Update button on Flight Details page, fill out the form, and click Save Changes button.

### Cleanup Old Flights:
Use this button to delete all old (before actual date) flights

### Search flight by Flight number
### Search flights by Date
### Search flights by Date and Departure city
### Search flights by Date and Arrival city


