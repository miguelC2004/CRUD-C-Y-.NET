CREATE DATABASE Clinica


USE Clinica

CREATE TABLE Persona (
    IdPersona INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Direccion VARCHAR(100) NULL
)

CREATE TABLE MotivoCita (
    IdMotivo INT PRIMARY KEY IDENTITY(1,1),
    Descripcion VARCHAR(100) NOT NULL
)

CREATE TABLE Cita (
    IdCita INT PRIMARY KEY IDENTITY(1,1),
    FechaCita DATETIME NOT NULL,
    IdPersona INT NOT NULL,
    FOREIGN KEY (IdPersona) REFERENCES Persona(IdPersona),
    CONSTRAINT CK_FechaCita CHECK(FechaCita >= GETDATE()),
)

CREATE TABLE CitaMotivo (
    IdCita INT NOT NULL,
    IdMotivo INT NOT NULL,
    CONSTRAINT PK_CitaMotivo PRIMARY KEY (IdCita, IdMotivo),
    FOREIGN KEY (IdCita) REFERENCES Cita(IdCita),
    FOREIGN KEY (IdMotivo) REFERENCES MotivoCita(IdMotivo)
)
