-- Insertion des types de lits
INSERT INTO TypeLit (LitDesc) VALUES ('Standard');
INSERT INTO TypeLit (LitDesc) VALUES ('Semi-Privée');
INSERT INTO TypeLit (LitDesc) VALUES ('Privée');

-- Insertion des départements
INSERT INTO Departement (NomDepartement) VALUES ('Chirurgie');
INSERT INTO Departement (NomDepartement) VALUES ('Pédiatrie');
INSERT INTO Departement (NomDepartement) VALUES ('Médecine Générale');

-- Insertion des lits (10 de chaque type dans chaque département)
DECLARE @i INT = 1;
WHILE @i <= 30
BEGIN
    INSERT INTO Lit (Occupe, IDType, IDDepartement) 
    VALUES (
        0, 
        CASE 
            WHEN @i <= 10 THEN 1  -- Premier groupe de 10 lits de type 'Standard'
            WHEN @i <= 20 THEN 2  -- Deuxième groupe de 10 lits de type 'Semi-Privée'
            ELSE 3  -- Dernier groupe de 10 lits de type 'Privée'
        END, 
        CASE 
            WHEN @i % 3 = 1 THEN 10  -- Chirurgie (IDDepartement = 10)
            WHEN @i % 3 = 2 THEN 20  -- Pédiatrie (IDDepartement = 20)
            ELSE 30  -- Médecine Générale (IDDepartement = 30)
        END
    );
    SET @i = @i + 1;
END;

-- Insertion des assurances (Assurance privée et sans assurance)
INSERT INTO Assurance (NomCompagnie) VALUES ('Beneva');
INSERT INTO Assurance (NomCompagnie) VALUES ('Desjardins');
INSERT INTO Assurance (NomCompagnie) VALUES ('SunLife');
INSERT INTO Assurance (NomCompagnie) VALUES ('Sans Assurance');

-- Insertion de médecins
INSERT INTO Medecin (Nom, Prenom) VALUES ('Dupont', 'Jean');
INSERT INTO Medecin (Nom, Prenom) VALUES ('Martin', 'Alice');
INSERT INTO Medecin (Nom, Prenom) VALUES ('Lambert', 'Paul');
INSERT INTO Medecin (Nom, Prenom) VALUES ('Arar', 'Lion');
INSERT INTO Medecin (Nom, Prenom) VALUES ('Lepage', 'Dominique');
INSERT INTO Medecin (Nom, Prenom) VALUES ('Tremblay', 'Marc');

-- Insertion de patients
INSERT INTO Patient (NSS, DateNaissance, Nom, Prenom, Adresse, Ville, Province, CodePostal, Telephone, IDAssurance) VALUES
('123456789012', '1990-01-01', 'Dupont', 'Jean', '123 Rue Principale', 'Montréal', 'QC', 'H1A1A1', '5141234567', 102030405),
('234567890123', '2005-05-05', 'Lemoine', 'Sophie', '456 Rue Secondaire', 'Montréal', 'QC', 'H1A2B2', '5142345678', 102030405),
('345678901234', '1975-03-10', 'Martel', 'Louis', '789 Rue Tertiaire', 'Montréal', 'QC', 'G1A3C3', '4181234567', 102031003),
('456789012345', '2010-07-15', 'Gagnon', 'Marie', '101 Rue Quaternaire', 'Montréal', 'QC', 'H1A4D4', '5143456789', 102031601),
('567890123456', '1985-11-20', 'Lachance', 'Pierre', '102 Rue Cinq', 'Laval', 'QC', 'H7A5A5', '4509876543', 102030405),
('678901234567', '1995-09-14', 'Tremblay', 'Julie', '103 Rue Six', 'Montréal', 'QC', 'J8X3Y3', '8199876543', 102031003),
('789012345678', '1980-03-25', 'Fortin', 'André', '104 Rue Sept', 'Longueuil', 'QC', 'J4V1V2', '4508765432', 102032199),
('890123456789', '1998-12-30', 'Pelletier', 'Isabelle', '105 Rue Huit', 'Montréal', 'QC', 'J1G1G1', '8198765432', 102030405),
('901234567890', '1965-06-15', 'Côté', 'Marc', '106 Rue Neuf', 'Laval', 'QC', 'G8Y7Z7', '8197654321', 102031003),
('012345678901', '1973-08-10', 'Morin', 'Anne', '107 Rue Dix', 'Montréal', 'QC', 'G7B2L8', '4187654321', 102031601),
('123456789013', '1983-04-18', 'Bouchard', 'Denis', '108 Rue Onze', 'Longueuil', 'QC', 'J2B4L5', '8196543210', 102030405),
('234567890124', '2002-02-22', 'Desrochers', 'Émilie', '109 Rue Douze', 'Laval', 'QC', 'J2S5V6', '4506543210', 102030405),
('345678901235', '1978-07-07', 'Gauthier', 'Hélène', '110 Rue Treize', 'Montréal', 'QC', 'G6P8T9', '8195432109', 102030405),
('456789012346', '1992-11-11', 'Lefebvre', 'Luc', '111 Rue Quatorze', 'Montréal', 'QC', 'J7C4K5', '4505432109', 102032199),
('567890123457', '1987-09-09', 'Simard', 'Chantal', '112 Rue Quinze', 'Montréal', 'QC', 'J6E5L3', '4504321098', 102032199),
('678901234568', '1960-02-28', 'Dubé', 'Michel', '113 Rue Seize', 'Laval', 'QC', 'G6G2V8', '4184321098', 102032199);

-- Remplissage de la table Admission
INSERT INTO Admission (ChirurgieProgramme, DateAdmission, DateChirurgie, DateDuConge, Televiseur, Telephone, NSS, NumeroLit, IDMedecin) VALUES
(1, '2024-08-25', '2024-08-26', NULL, 1, 0, '901234567890', 9900, 666), -- chirurgie, chambre standard, chirurgien 1
(1, '2024-08-25', '2024-08-27', NULL, 1, 1, '678901234567', 9915, 333), -- chirurgie, chambre semi-privée, chirurgien 3
(0, '2024-05-17', NULL, NULL, 0, 0, '456789012345', 9907, 222), -- pédiatrie, pas de chirurgie, chambre standard
(0, '2024-07-03', NULL, NULL, 0, 0, '678901234568', 9905, 555); -- générale, pas de chirurgie, chambre standard