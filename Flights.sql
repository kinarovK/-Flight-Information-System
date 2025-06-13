CREATE DATABASE FlightsDb;
GO
USE FlightsDb;

CREATE TABLE dbo.Flights (
    FlightNumber NVARCHAR(10) NOT NULL PRIMARY KEY,
    DepartureDateTime DATETIME2 NOT NULL,
    DepartureAirportCity NVARCHAR(100) NOT NULL,
    ArrivalAirportCity NVARCHAR(100) NOT NULL,
    DurationMinutes INT NOT NULL
);

CREATE PROCEDURE AddFlight
    @FlightNumber NVARCHAR(10),
    @DepartureDateTime DATETIME2,
    @DepartureAirportCity NVARCHAR(100),
    @ArrivalAirportCity NVARCHAR(100),
    @DurationMinutes INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Checkk the next 7day
    IF @DepartureDateTime BETWEEN GETDATE() AND DATEADD(DAY, 7, GETDATE())
    BEGIN
        INSERT INTO dbo.Flights (FlightNumber, DepartureDateTime, DepartureAirportCity, ArrivalAirportCity, DurationMinutes)
        VALUES (@FlightNumber, @DepartureDateTime, @DepartureAirportCity, @ArrivalAirportCity, @DurationMinutes);
    END
    ELSE
    BEGIN
        RAISERROR('DepartureDateTime must be between 7 days!', 16, 1);
    END
END;


CREATE PROCEDURE GetFlightByNumber
    @FlightNumber NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Flights
    WHERE FlightNumber = @FlightNumber;
END;

CREATE PROCEDURE GetFlightsByDate
    @Date DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Flights
    WHERE CAST(DepartureDateTime AS DATE) = @Date;
END;

CREATE PROCEDURE GetFlightsByDepartureCityAndDate
    @City NVARCHAR(100),
    @Date DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Flights
    WHERE DepartureAirportCity = @City AND CAST(DepartureDateTime AS DATE) = @Date;
END;

CREATE PROCEDURE GetFlightsByArrivalCityAndDate
    @City NVARCHAR(100),
    @Date DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Flights
    WHERE ArrivalAirportCity = @City AND CAST(DepartureDateTime AS DATE) = @Date;
END;

CREATE PROCEDURE CleanupOldFlights
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.Flights
    WHERE DepartureDateTime < GETDATE();
END;


CREATE PROCEDURE UpdateFlight
    @FlightNumber NVARCHAR(10),
    @DepartureDateTime DATETIME2,
    @DepartureAirportCity NVARCHAR(100),
    @ArrivalAirportCity NVARCHAR(100),
    @DurationMinutes INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.Flights WHERE FlightNumber = @FlightNumber)
    BEGIN
        UPDATE dbo.Flights
        SET DepartureDateTime = @DepartureDateTime,
            DepartureAirportCity = @DepartureAirportCity,
            ArrivalAirportCity = @ArrivalAirportCity,
            DurationMinutes = @DurationMinutes
        WHERE FlightNumber = @FlightNumber;
    END
    ELSE
    BEGIN
        RAISERROR('Flight with the specified FlightNumber does not exist.', 16, 1);
    END
END;

CREATE PROCEDURE DeleteFlight
    @FlightNumber NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.Flights WHERE FlightNumber = @FlightNumber)
    BEGIN
        DELETE FROM dbo.Flights
        WHERE FlightNumber = @FlightNumber;
    END
    ELSE
    BEGIN
        RAISERROR('Flight with the specified FlightNumber does not exist.', 16, 1);
    END
END;



--script to fill the db:
DECLARE @i INT = 1;

WHILE @i <= 100
BEGIN
    INSERT INTO Flights (FlightNumber, DepartureDateTime, DepartureAirportCity, ArrivalAirportCity, DurationMinutes)
    VALUES (
        CONCAT('FN', FORMAT(@i, '000')), 
        DATEADD(MINUTE, FLOOR(RAND() * 1440), DATEADD(DAY, FLOOR(RAND() * 6), GETDATE())),
        CASE FLOOR(RAND() * 5)
            WHEN 0 THEN 'Kyiv'
            WHEN 1 THEN 'London'
            WHEN 2 THEN 'Paris'
            WHEN 3 THEN 'Berlin'
            WHEN 4 THEN 'Warsaw'
        END,
        CASE FLOOR(RAND() * 5)
            WHEN 0 THEN 'New York'
            WHEN 1 THEN 'Madrid'
            WHEN 2 THEN 'Rome'
            WHEN 3 THEN 'Dublin'
            WHEN 4 THEN 'Lisbon'
        END,
        FLOOR(RAND() * 360 + 60)
    );

    SET @i = @i + 1;
END;