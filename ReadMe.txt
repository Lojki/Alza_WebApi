Project: Alza_WebApi
Contact: Jakub Lojkásek

Framework: .NET Core 3.1

Application is divided into two projects.
-----------------------------------------
"Data" project with Repository to get Data.
"Api" project with Endpoints (Controllers, Actions) and Response model.

Unit Tests are divided into two projects.
-----------------------------------------
"Shared" with logic to create test server.
"Api" with tests for Endpoints.

Description
-----------
I created three versions of Product API to show Endpoint versioning in URL path segment. (v1, v2 and v3)

Third version should met your goals with endpoints:
-Endpoint for getting all available Products.
-Endpoint for getting Product by id.
-Endpoint for partial update (Description property update) of Product.; I choose PATCH method. // From Wikipedia: The PATCH method is a request method supported by the Hypertext Transfer Protocol (HTTP) protocol for making partial changes to an existing resource
And with Product response model:
-Id
-Name
-ImgUri
-Price
-Description

Third version has breaking changes in compare to first and second version:
-Product response model has Available property in first and second version.

Third and second version has breaking changes in compare to first version:
-First version API has one more Endpoint for getting all Products.

Endpoints:
-Get Product by id [all versions]
Method: GET
Parameters: id (query)
/v3/Product/{id}

-Update product description (by id) [all versions]
Method: PATCH
Parameters: id (query), description (body)
/v3/Product/{id}/UpdateDescription

-Update product description [all versions]
Method: PATCH
Parameters: Product (body)
/v3/Product/UpdateDescription

-Get available products [all versions]
Method: GET
Parameters: none
/v3/Product/AvailableProducts

-Get all products [only first version]
Method: GET
Parameters: none
/v1/Product/AllProducts

IoC
---
Autofac is used for IoC.

Request & Response objects
-------------------------
For Request and Response objects JSON format is used.

Model validation
----------------
Validation of Product response model is done by Fluent validation. (Product as parameter is used only in Update product description endpoint - /v3/Product/UpdateDescription)
Invalid requests are automatically logged in DEBUG level.

Rules I created:
-Id => required; greater than 0
-Name => required
-ImgUri => required; should be valid URI
-Price => required; greater than 0

Logging
-------
Logging is done by NLog.
Configured to File target ${basedir}/logs/${shortdate}.log with minimum TRACE level.
Logging is also limited in appsettings.json to INFORMATION level and for Development environment to DEBUG level.

Swagger
-------
Swagger documentation is automatically generated for each version of API.

Prerequisities to run application using dotnet run
--------------------------------------------------
Installed .NET Core 3.1 SDK
Download page: https://dotnet.microsoft.com/en-us/download/visual-studio-sdks

Steps how to start application
------------------------------
Open command line.
Go to folder with API project "Alza_WebApi.Api".
Run "dotnet run" command.

or

Open Visual Studio 2022.
Open solution file "Alza_WebApi\Alza_WebApi.sln".
Set "Alza_WebApi.Api" project as Startup project. (from Right click context menu) and Start Debugging (F5) or Start Without Debugging (Ctrl + F5).
Or Right click to project "Alza_WebApi.Api" and choose from context menu Debug->Start New Instance or Debug->Start Without Debugging

Note: launchSettings.json is not changed so application is available on:
-when application started by dotnet run command:
--https://localhost:5001
--http://localhost:5000
-when application started from Visual Studio:
--https://localhost:44369
--http://localhost:34973

How to run Unit tests
---------------------
Open Visual Studio 2022
Open solution file Alza_WebApi\Alza_WebApi.sln
-Open "Test Explorer" (Ctrl + E, T) from "Test" menu and Run All Tests In View (Ctrl + R, V)