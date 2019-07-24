CREATE DATABASE Agenda

USE Agenda

CREATE TABLE Usuario(
id int primary key identity(1,1),
usuario varchar(30),
password varchar(30)
)

CREATE UNIQUE INDEX IDX_Usuario on usuario(usuario)

CREATE TABLE Agenda(
id int primary key identity,
idUsuario int,
nombreContacto varchar(50),
telefono varchar(15),
email varchar(60),
direccion varchar(70)
)

ALTER TABLE Agenda ADD CONSTRAINT FK_idUSUARIO FOREIGN KEY (idUsuario) REFERENCES Usuario(id)


insert into usuario VALUES ('manito@gmail.com','123456')
insert into usuario VALUES ('usuario@gmail.com','usuario')
SELECT * FROM Usuario
SELECT * FROM Agenda

INSERT INTO Agenda VALUES (1,'Gouri Ramirez','829-546-8509','g.ramirez159@gmail.com','Mi direccion de Cache')
INSERT INTO Agenda VALUES (2,'Luis Daniel','809-597-4259','20175184@itla.edu.do','Mi direccion Bonita para web')
INSERT INTO Agenda VALUES (1,'Willis Polanco','829-132-6547','wpolanco@itla.edu.do','Direccion de Wilis')

DELETE FROM usuario WHERE id >2