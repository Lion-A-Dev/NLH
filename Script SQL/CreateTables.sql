-- Création de la table Assurance
CREATE TABLE Assurance (
    IDAssurance INT PRIMARY KEY IDENTITY(102030405,598),
    NomCompagnie VARCHAR(100) NOT NULL
);

-- Création de la table Patient
CREATE TABLE Patient (
    NSS CHAR(12) PRIMARY KEY,
    DateNaissance DATE NOT NULL,
    Nom VARCHAR(50) NOT NULL,
    Prenom VARCHAR(50) NOT NULL,
    Adresse VARCHAR(100) NOT NULL,
    Ville VARCHAR(50) NOT NULL,
    Province VARCHAR(50) NOT NULL,
    CodePostal CHAR(6) NOT NULL,
    Telephone CHAR(10) NOT NULL,
    IDAssurance INT,
    FOREIGN KEY (IDAssurance) REFERENCES Assurance(IDAssurance)
);

-- Création de la table Medecin
CREATE TABLE Medecin (
    IDMedecin INT PRIMARY KEY IDENTITY(111,111),
    Nom VARCHAR(50) NOT NULL,
    Prenom VARCHAR(50) NOT NULL
);

-- Création de la table Département
CREATE TABLE Departement (
    IDDepartement INT PRIMARY KEY IDENTITY(10,10),
    NomDepartement VARCHAR(100) NOT NULL
);

-- Création de la table TypeLit
CREATE TABLE TypeLit (
    IDType INT PRIMARY KEY IDENTITY(1,1),
    LitDesc VARCHAR(100) NOT NULL
);

-- Création de la table Lit
CREATE TABLE Lit (
    NumeroLit INT PRIMARY KEY IDENTITY(9900,1),
    Occupe BIT NOT NULL,
    IDType INT NOT NULL,
    IDDepartement INT NOT NULL,
    FOREIGN KEY (IDType) REFERENCES TypeLit(IDType),
    FOREIGN KEY (IDDepartement) REFERENCES Departement(IDDepartement)
);

-- Création de la table Admission
CREATE TABLE Admission (
    IDAdmission INT PRIMARY KEY IDENTITY(50100,1),
    ChirurgieProgramme BIT NOT NULL,
    DateAdmission DATE NOT NULL,
    DateChirurgie DATE NULL,
    DateDuConge DATE NULL,
    Televiseur BIT NOT NULL,
    Telephone BIT NOT NULL,
    NSS CHAR(12) NOT NULL,
    NumeroLit INT NOT NULL,
    IDMedecin INT NOT NULL,
    FOREIGN KEY (NSS) REFERENCES Patient(NSS),
    FOREIGN KEY (NumeroLit) REFERENCES Lit(NumeroLit),
    FOREIGN KEY (IDMedecin) REFERENCES Medecin(IDMedecin)
);