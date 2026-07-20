# NotesJournalApp project #####################################

## Objectives #################################################

1. Build ASP.NET Core Web API for
   (a) Creating a note
   (b) Editing a note
   (c) Deleting a note
   (d) Listing all notes
   (e) Searching notes
   
2. Build a React front-end to use the above API.

3. Allow the API to use an Azure SQL database for note storage.

4. Deploy the front-end and the back-end to Azure App Service.

## Roadmap according to Microsoft Copilot #####################

1. Create the ASP.NET Core API.

2. Design the data model.

3. Create Azure SQL Database.

4. Secure secrets with Key Vault.

5. Deploy to Azure App Service.

6. Add Monitoring with App Insights.

## Starting a "clean-architecture" version ####################

### In Bash or PowerShell #####################################

```````````````````````````````````````````````
mkdir NullPointersEtc/NotesJournalApp

cd    NullPointersEtc/NotesJournalApp

dotnet new sln -n NotesJournalApp

dotnet new webapi -n NotesBackEnd

dotnet new classlib -n NotesDomain

dotnet new classlib -n NotesHandlers

dotnet new classlib -n NotesStorage

dotnet sln add NotesBackEnd

dotnet sln add NotesDomain

dotnet sln add NotesHandlers

dotnet sln add NotesStorage

dotnet add NotesHandlers reference NotesDomain

dotnet add NotesStorage reference NotesDomain

dotnet add NotesStorage reference NotesHandlers

dotnet add NotesBackEnd reference NotesDomain

dotnet add NotesBackEnd reference NotesHandlers

dotnet add NotesBackEnd reference NotesStorage
```````````````````````````````````````````````





