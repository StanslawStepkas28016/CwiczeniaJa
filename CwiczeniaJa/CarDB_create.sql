-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2024-05-09 11:56:00.551

-- tables
-- Table: Car
CREATE TABLE Car (
    Id int  NOT NULL,
    Brand nvarchar(200)  NOT NULL,
    Price numeric(25,2)  NOT NULL,
    ManufacturedDate datetime  NOT NULL,
    CONSTRAINT Car_pk PRIMARY KEY  (Id)
);

-- End of file.

