**Prozessdokumentation: Entwicklung der Fuhrpark-App**

**Team C: Munib, Justin, Ayoub, Maxim, Amir**

**Inhaltsverzeichnis**

- [**Einleitung**](#Einleitung) 1.1 Ziel des Projekts  
     1.2 verwendete Technologien und Entwicklungsumgebung
- [**Projektplanung**](#Projektplanung) 2.1 Zielsetzung  
     2.2 Erstellung des UML-Diagramms  
     2.3 Auswahl der Technologien  
      2.3.1 XAMPP  
      2.3.2 .NET MAUI  
      2.3.3 MVVM-Architektur
- [**Umsetzung - Der Entwicklungsprozess**](#Umsetzung) 3.1 Einrichtung der Entwicklungsumgebung  
     3.2 Aufbau der Datenbank  
     3.3 Programmierung der Model-Klassen  
     3.4 Aufbau der Datenbankschicht (DatabaseService)  
     3.5 Implementierung der Geschäftslogik (FleetViewModel)  
     3.6 Gestaltung der Benutzeroberfläche (Views)  
     3.7 Verbindung zwischen View und ViewModel
- [**Testdokumentation: User Tests (MAUI App)**](#Testdokumentation) 4.1 Funktionalität: Fahrzeugliste und Initialisierung  
     4.2 Funktionalität: Fahrzeug hinzufügen (Create)  
     4.3 Funktionalität: Fahrzeugaktionen (Update/Delete)  
     4.4 Funktionalität: Fahrzeugsuche (Read - Filter)
- [**Probleme und Lösungen**](#Probleme)
- [**Quellen und Tools**](#Quellen)

**Prozessdokumentation: Entwicklung der Fuhrpark-App**

**1\.** **Einleitung**

Diese Prozessdokumentation beschreibt detailliert die Planung, Entwicklung und Umsetzung der Anwendung „Fuhrpark-App", die zur Verwaltung von Fahrzeugen dient.

Ziel des Projekts war es, eine moderne, datenbankgestützte Anwendung zu entwickeln, mit der PKW und LKW angelegt, gesucht, gelöscht und deren Status geändert werden können.

Die App wurde mit dem .NET MAUI Framework (C#) entwickelt und greift auf eine MySQL-Datenbank zu, die lokal über XAMPP betrieben wird.

Als Entwicklungsumgebung wurde Visual Studio 2022 verwendet.

**2\.** **Projektplanung**

**2.1 Zielsetzung**

Die Anwendung soll die Verwaltung von Fahrzeugen vereinfachen.  
Dazu gehören folgende Funktionen:

- Fahrzeuge hinzufügen (PKW oder LKW)
- Fahrzeuge löschen
- Fahrzeuginformationen anzeigen
- Fahrzeugstatus ändern („Verfügbar" / „Gemietet")
- Fahrzeugsuche nach Kennzeichen oder Klasse

Langfristig könnte das Projekt um Benutzerverwaltung und Statistikfunktionen erweitert werden.

**2.2 Erstellung des UML-Diagramms**

Vor Beginn der Programmierung wurde ein **UML-Klassendiagramm** erstellt, um die Struktur der Anwendung zu planen.  
Das Diagramm half dabei, die Beziehungen zwischen den einzelnen Komponenten (Model, ViewModel, Service, View) festzulegen.

Im Diagramm sind folgende Hauptklassen enthalten:

- **Vehicle** (Fahrzeugdaten)
- **User** (optional für spätere Benutzerverwaltung)
- **DatabaseService** (Verbindung zur Datenbank)
- **FleetViewModel** (Logik und Steuerung)
- **View-Komponenten** wie _HomePage_ und _AddVehicleView_

![Ein Bild, das Screenshot, Diagramm enthält.


**2.3 Auswahl der Technologien**

**2.3.1 XAMPP**

XAMPP wurde als **lokale Serverumgebung** verwendet, um MySQL-Datenbanken einfach und kostenlos auf dem eigenen Rechner zu betreiben.  
Mit **phpMyAdmin** können Tabellen komfortabel erstellt, bearbeitet und geprüft werden.

**Gründe für XAMPP:**

- Schnelle Installation und keine Cloud-Verbindung nötig
- Ideal für lokale Entwicklungs- und Testumgebungen
- Einfache Verwaltung von Datenbanken über Browseroberfläche

![Ein Bild, das Text, Screenshot, Software, Computersymbol enthält.


**2.3.2 .NET MAUI**

MAUI („Multi-platform App UI") ist Microsofts modernstes Framework zur Erstellung von Cross-Plattform-Apps mit einer einzigen Codebasis.  
Die Entscheidung für MAUI fiel, weil damit Anwendungen für **Windows**, **Android** und **iOS** entwickelt werden können, ohne verschiedene Projekte anlegen zu müssen.

**2.3.3 MVVM-Architektur**

Das Projekt basiert auf dem Muster **Model-View-ViewModel (MVVM)**, das für eine klare Trennung von:

- **Daten (Model)**
- **Logik (ViewModel)**
- **Darstellung (View)**  
    sorgt.  
    Dadurch ist der Code besser wartbar, testbar und erweiterbar.



**3\.** **Umsetzung - der Entwicklungsprozess**

**3.1 Einrichtung der Entwicklungsumgebung**

- Installation von **Visual Studio 2022** mit den Workloads „.NET Multi-platform App UI Development" und „.NET Desktop Development".
- Installation von **XAMPP** zur lokalen Bereitstellung der MySQL-Datenbank.
- Erstellen eines neuen **.NET MAUI App-Projekts** in Visual Studio mit dem Namen _Fuhrpark_.
- Strukturierung des Projekts in die drei Hauptordner:
  - _Models_
  - _ViewModels_
  - _Views_

**3.2 Aufbau der Datenbank**

Mit **phpMyAdmin** wurde eine neue Datenbank namens _Fuhrpark_ erstellt.  
Darin wurde die Tabelle _Wagen_ mit folgenden Spalten angelegt:



**3.3 Programmierung der Model-Klassen**

Im Ordner _Models_ wurden zwei Hauptklassen angelegt:

**Vehicle.cs**

Enthält alle Fahrzeugdaten und Eigenschaften, die mit der Datenbanktabelle _wagen_ übereinstimmen:

- Id, LicensePlate, Manufacturer, Model, Mileage, YearOfManufacture, Ton, VehicleClass, State

Zudem enthält sie eine boolesche Eigenschaft isTruck, um automatisch zu erkennen, ob es sich um einen LKW handelt.

**User.cs**

Vorbereitung für zukünftige Erweiterungen mit Benutzerverwaltung (enthält Id, Username, PasswordHash).

**3.4 Aufbau der Datenbankschicht (DatabaseService)**

Der **DatabaseService.cs** stellt die Verbindung zwischen C# und MySQL her.  
Dazu wurde das NuGet-Paket **MySql.Data** eingebunden.  
Die Verbindung erfolgt über den Connection-String:

server=127.0.0.1;port=3306;database=fuhrpark;user=root;password=;

Implementierte Funktionen:

- GetVehiclesAsync() → Fahrzeuge laden
- AddVehicleAsync() → Neues Fahrzeug hinzufügen
- DeleteVehicleAsync() → Fahrzeug löschen
- SearchByClassAsync() → Suche nach Fahrzeugklasse
- SearchByLicensePlateAsync() → Suche nach Kennzeichen
- UpdateVehicleStateAsync() → Fahrzeugstatus ändern

**3.5 Implementierung der Geschäftslogik (FleetViewModel)**

In der Klasse **FleetViewModel.cs** wurde die Logik implementiert, die zwischen View und Datenbank vermittelt.  
Sie enthält:

- ObservableCollection&lt;Vehicle&gt; Vehicles → für die Live-Anzeige in der View
- Properties für neue Fahrzeuge (Kennzeichen, Modell, Tonnen usw.)
- Commands für Benutzeraktionen:
-  AddVehicleCommand
-  DeleteVehicleCommand
-  UpdateStateCommand
-  SearchCommand
-  LoadVehiclesCommand

Die Commands werden über **Binding** direkt an die Benutzeroberfläche gekoppelt, ohne Code-Behind zu verwenden.  
Das sorgt für eine saubere Trennung und erleichtert spätere Änderungen.

**3.6 Gestaltung der Benutzeroberfläche (Views)**

**HomePage.xaml**

Zeigt die Fahrzeugliste, Suchfelder und Schaltflächen.  
Die Fahrzeuge werden in einer **CollectionView** dargestellt.  
Über **SwipeView**\-Elemente können Benutzer Fahrzeuge löschen oder den Status ändern.

**AddVehicleView.xaml**

Ein Popup-Fenster, das mit dem **CommunityToolkit.Maui** erstellt wurde.  
Hier kann der Benutzer ein neues Fahrzeug eintragen.  
Wenn „LKW" ausgewählt wird, erscheint automatisch ein Dropdown-Menü zur Auswahl der Tonnenzahl.

**3.7 Verbindung zwischen View und ViewModel**

In der **HomePage.xaml.cs** wird das FleetViewModel als BindingContext gesetzt.  
So kann die UI direkt auf die Daten und Commands aus dem ViewModel zugreifen.

**4\.** **Testdokumentation: User Tests (MAUI App)**

Die folgenden User-Tests überprüfen die Kernfunktionen der Anwendung auf Basis der umgesetzten MAUI-Benutzeroberfläche. Voraussetzung: Die Anwendung von XAMPP muss erfolgreich starten und eine Verbindung zur MySQL Datenbank herstellen können.

**4.1 Funktionalität: Fahrzeugliste und Initialisierung**

| **Test-ID** | **Testfall** | **Schritte** | **Erwartetes Ergebnis** |
| --- | --- | --- | --- |
| **UT-01** | **Liste laden** | **1\. Anwendung starten und zur Hauptseite navigieren.** | **Die CollectionView zeigt die aktuelle Liste aller Fahrzeuge aus der Datenbank an.** |
| **UT-02** | **Listenstruktur** | **1\. UT-01 erfolgreich durchführen.2. Ein Fahrzeug auswählen und Details prüfen.** | **Die Details (Kennzeichen, Hersteller, Modell, Kilometerstand, Baujahr, Status und Klasse) werden korrekt dargestellt.** |

4.2 Funktionalität: Fahrzeug hinzufügen (Create)

| **Test-ID** | **Testfall** | **Schritte** | **Erwartetes Ergebnis** |
| --- | --- | --- | --- |
| UT-03 | PKW hinzufügen | 1\. Auf „Fahrzeug hinzufügen" klicken.2. „PKW" auswählen.3. Pflichtfelder ausfüllen.4. Auf „Fahrzeug hinzufügen" klicken. | Das Popup schließt sich, die Liste lädt neu und der neue PKW (ohne Tonnage) erscheint in der Liste. |
| UT-04 | LKW hinzufügen | 1\. Auf „Fahrzeug hinzufügen" klicken.2. „LKW" auswählen (Feld „Tonner" sichtbar).3. Pflichtfelder + Tonner ausfüllen.4. Auf „Fahrzeug hinzufügen" klicken. | Das Popup schließt sich und der neue LKW mit korrektem Tonner-Wert wird angezeigt. |
| UT-05 | Validierung (Fehlende Daten) | 1\. Auf „Fahrzeug hinzufügen" klicken.2. Nicht alle Pflichtfelder ausfüllen.3. Auf „Fahrzeug hinzufügen" klicken. | Eine Fehlermeldung („Eingabe fehlt") erscheint, Popup bleibt geöffnet. |

4.3 Funktionalität: Fahrzeugaktionen (Update/Delete)

| **Test-ID** | **Testfall** | **Schritte** | **Erwartetes Ergebnis** |
| --- | --- | --- | --- |
| UT-06 | Fahrzeug löschen | 1\. Fahrzeug auswählen.2. Nach links wischen.3. Auf „Löschen" klicken. | Fahrzeug verschwindet aus der Liste und wird dauerhaft aus der Datenbank entfernt. |
| UT-07 | Status ändern (Verfügbar → Gemietet) | 1\. Fahrzeug mit Status „Verfügbar" auswählen.2. Nach links wischen.3. Auf „Status" klicken. | Der Status ändert sich auf „Gemietet". |
| UT-08 | Status ändern (Gemietet → Verfügbar) | 1\. Fahrzeug mit Status „Gemietet" auswählen.2. Nach links wischen.3. Auf „Status" klicken. | Der Status ändert sich zurück auf „Verfügbar". |

4.4 Funktionalität: Fahrzeugsuche (Read - Filter)

| **Test-ID** | **Testfall** | **Schritte** | **Erwartetes Ergebnis** |
| --- | --- | --- | --- |
| UT-09 | Suche nach Kennzeichen | 1\. „Kennzeichen" auswählen.2. Teil eines Kennzeichens eingeben.3. Auf „Suchen" klicken. | Nur Fahrzeuge mit passendem Kennzeichen werden angezeigt. |
| UT-10 | Suche nach Klasse | 1\. „Klasse" auswählen.2. „LKW" eingeben.3. Auf „Suchen" klicken. | Nur Fahrzeuge des Typs „LKW" werden angezeigt. |
| UT-11 | Suche zurücksetzen | 1\. Nach erfolgreicher Suche (z. B. UT-09).2. Suchfeld leeren.3. Auf „Suchen" klicken. | Alle Fahrzeuge des Fuhrparks werden wieder angezeigt. |

**5\.** **Probleme und Lösungen**

Während der Entwicklung traten folgende Schwierigkeiten auf:

- **Fehlerhafte Datenbankverbindung:**  
    Ursache war ein fehlender Port im Connection-String. → Lösung: Port 3306 hinzugefügt.
- **Popup schloss sich zu früh:**  
    Lösung: Nach erfolgreichem Hinzufügen wurde das Popup manuell geschlossen, nachdem alle Eingaben zurückgesetzt wurden.
- **Binding-Probleme:**  
    Lösung: Überprüfung aller PropertyChanged-Events im ViewModel.

**Fazit und Ausblick**

Das Ziel, eine funktionierende Fuhrpark-App zu entwickeln, wurde vollständig erreicht.  
Alle CRUD-Operationen sind implementiert und getestet.

Ich habe durch dieses Projekt ein tiefes Verständnis für:

- Datenbankanbindung mit C#
- Asynchrone Programmierung
- MVVM-Struktur und Datenbindung
- Benutzeroberflächengestaltung mit XAML  
    gewonnen.

**Zukünftige Erweiterungen:**

- Benutzerverwaltung mit Login
- Erweiterte Suche (nach Baujahr, Kilometerstand etc.)
- Statistiken über Fahrzeugnutzung
- Exportfunktion (PDF oder CSV)

**6\.** **Quellen und Tools**

- **Microsoft .NET MAUI Framework**
- **Visual Studio 2022**
- **MySQL / XAMPP / phpMyAdmin**
- **CommunityToolkit.Maui**
