
#Installation & Setup

##Clone the repository:

```
git clone https://github.com/ST10404816/PROG6212_Part2.git
cd POE
```
##Open the solution in Visual Studio:
Open POE.sln in Visual Studio.
##Restore NuGet packages: In Package Manager Console, run:

```
dotnet restore
```
##Set up the database:
Ensure SQL Server LocalDB is installed.
Update the connection string in appsettings.json if needed:
```
"ConnectionStrings": {
  "Part2": "Server=(localdb)\\MSSQLLocalDB;Database=Part2;Trusted_Connection=True;MultipleActiveResultSets=True"
}
```
Run the following commands to apply migrations (if applicable):
```
Add-Migration InitialCreate
Update-Database
```
##Run the application: Press F5 in Visual Studio to run the application.

#Usage Guide
##1. Create a Claim
Navigate to the Create Claim page.
Fill in the required fields.
Optionally, upload a file (supports PDF, DOCX, and XLSX).
Click Submit to save the claim.
##2. View Claims
View all submitted claims on the View Claims page.
##3. Update Claim Status
Use the Update Status button to change the status of a claim.
##4. Edit Claim Details
Go to the Edit page to modify the claim's details (e.g., hours worked).
##5. Delete a Claim
Click Delete on a claim to remove it from the system.
##6. Download Uploaded Files
Use the Download button to retrieve uploaded files.

#Validation and Error Handling
##File Validation: 
Only allows PDF, DOCX, and XLSX files for upload.
##Model Validation: 
Ensures all required fields are filled.
##Error Handling: 
Displays error messages for invalid operations or missing files.
##Success Messages: 
Uses TempData to display success messages between pages.

#Configuration Files
appsettings.json: Contains connection strings and general logging settings.
```
"ConnectionStrings": {
  "Part2": "Server=(localdb)\\MSSQLLocalDB;Database=Part2;Trusted_Connection=True;MultipleActiveResultSets=True"
}
appsettings.Development.json: Contains development-specific logging settings.
json
```
```
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft.AspNetCore": "Warning"
  }
}
```
#Technologies Used
ASP.NET Core: Web framework used to build the application.
Entity Framework Core: ORM for database interaction.
SQL Server LocalDB: Local database used for development.
MSTest: Unit testing framework (for future tests).
C#: Primary programming language.

#License
This project is licensed under the MIT License.

#Contributors
Lisha Naidoo – ST10404816 (Group 1)

#References
*https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/
*https://dotnethow.net/a-step-by-step-guide-to-configuring-entity-framework-in-your-net-web-api-project/
*https://www.bytehide.com/blog/data-annotations-in-csharp
*https://docs.aspnetzero.com/aspnet-core-mvc
*https://www.w3schools.com/




