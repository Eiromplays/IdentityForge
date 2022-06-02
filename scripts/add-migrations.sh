#!/bin/bash

# Parameters/Arguments setup
while [ $# -gt 0 ]; do
  case "$1" in
    -mp|-migrationProvider|--migrationProvider)
      migrationProvider="$2"
      ;;
    -qm|-quickMode|--quickMode)
      quickMode="$2"
      ;;
    -env|-environment|--environment)
      environment="$2"
      ;;
    -ps|-prerequisites|--prerequisites)
      prerequisites="$2"
      ;;
    -db|-dc|-dbcontext|--dbcontext)
      dbcontext="$2"
      ;;
    *)
      printf "***************************\n"
      printf "* Error: Invalid argument.*\n"
      printf "***************************\n"
      exit 1
  esac
  shift
  shift
done

# Default values for arguments
migrationProvider=${migrationProvider:-ALL}
quickMode=${quickMode:-false}
environment=${environment:-Development}
prerequisites=${prerequisites:-true}
dbcontext=${dbcontext:-ALL}

appsettingsFile="appsettings.$environment.json"

# Set the path to the startup project
cd $PWD/../src/Presentation/Eiromplays.IdentityServer

databaseProviders=(PostgreSql SqlServer MySql Sqlite)

dbContextList=$(dotnet ef dbcontext list)

readarray -t dbContexts <<<"$dbContextList"

# Remove the two first values from the dbcontext list response, as they are not needed
unset dbContexts[0]
unset dbContexts[1]

# If prerequisites is enabled, then install all the required dependencies
if [ "$prerequisites" = "true" ]; then
    echo "Installing prerequisites"
    sudo apt-get install jq -y
    echo "Successfully installed prerequisites"
fi

currentDatabaseProvider=$(jq -r '.DatabaseConfiguration.DatabaseProvider' $appsettingsFile)
currentApplyDefaultSeeds=$(jq -r '.DatabaseConfiguration.ApplyDefaultSeeds' $appsettingsFile)

for databaseProvider in "${databaseProviders[@]}"
do
    echo "$databaseProvider"
    if [ "$migrationProvider" = "ALL" ] || [ "$databaseProvider" = "$migrationProvider" ]; then
        cat <<< $(jq --arg databaseProvider "$databaseProvider" '.DatabaseConfiguration.DatabaseProvider |= $databaseProvider' $appsettingsFile) > $appsettingsFile
        echo "Starting migrations for: $databaseProvider"
        for dbContext in "${dbContexts[@]}"
        do
            dbContextName=${dbContext##*.}
            echo "name: $dbContextName context: $dbcontext"
            if [ "$dbContextName" != "$dbcontext" ] && [ "$dbcontext" != "ALL" ]; then
                continue
            fi

            if [ "$quickMode" = "false" ]; then
                echo "Please input a migration name for $dbContext:"
                read migrationName
                migrationName=${migrationName:-Initial}
            else
                migrationName=Initial
            fi
            migrationFolderName=${dbContextName/DbContext/}
            migrationPath="Migrations/$migrationFolderName"
            echo "Starting migration for: $dbContextName Migration name: $migrationName"
            dotnet ef migrations add "$migrationName" -c $dbContext -o $migrationPath -p $PWD/../../Infrastructure/Migrators.$databaseProvider/Migrators.$databaseProvider.csproj
            echo "Migration for: $dbContextName completed and can be found at: $migrationPath"
        done
    fi
done

echo "Reverting DatabaseProvider back to previos: $currentDatabaseProvider"

cat <<< $(jq --arg currentDatabaseProvider "$currentDatabaseProvider" '.DatabaseConfiguration.DatabaseProvider |= $currentDatabaseProvider' $appsettingsFile) > $appsettingsFile

echo "All migrations should have been created successfully."