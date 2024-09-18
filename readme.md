### How to install template?

1. Open terminal in template project directory
2. Execute command: `dotnet new install .`

### How can I create a new project?

1. Open terminal in the directory where you want to create the project
2. Execute command: `dotnet new credocoretemplate -o ProjectName`

### How can I update the template version?

1. First, you have to delete the template  
   a. Open terminal in template project directory  
   b. Execute command: `dotnet new uninstall .`
2. Re-install as we did in the first paragraph

### How can I fix problem with Credo.JCS nuget

1. In visual studio Nuget Package Manager -> Package Sources add new Nuget Sources
   a. Name : credobank
   b. Source: https://pkgs.dev.azure.com/credobank/_packaging/credobank/nuget/v3/index.json
