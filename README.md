# Pokemon App
This is a simple web application for viewing and filtering Pokemon data. The data is retrieved from an API that returns data from a connected database. Users can search for Pokemon by name, HP, attack, and defense, and navigate through the paginated list of results.

## Technologies
C#
ASP.NET Core 5.0
Razor Pages
Bootstrap 5.0
HttpClient
Newtonsoft.Json

## Installation
To run this application, you'll need to have the following installed on your machine:
.NET Core SDK

#To get started:
Clone the repository: git clone https://github.com/hhuumm/Pokemon__App.git
Change into the project directory: cd pokemon-app
Restore the packages: dotnet restore
Run the project: dotnet run
Navigate to the appropriate port 

## Usage
When you open the app, the list of pokemon will be empty. You can provide a csv file with the attributes below that will then create entries into your attached DB in t he appsettings.json. You can use the search bar and the input fields for HP, attack, and defense to filter the list of Pokemon. The table displays the following information for each Pokemon:

Name
Type 1
Type 2
Total
HP
Attack
Defense
Special Attack
Special Defense
Speed
Generation
Legendary
You can use the "Next" and "Previous" buttons to navigate through the paginated results.

## Credits
This app was created by Hamid Ebrahimi. The Pokemon data used in this app is from CobbleStone company.

## License
This project is licensed under the MIT License.
