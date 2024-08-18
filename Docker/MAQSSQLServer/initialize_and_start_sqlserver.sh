#!/bin/bash
{ 
    # Wait for SQL Server to start up
    # Check if the server is ready
    not_ready=1
    while [ $not_ready != 0 ]
    do
        # Wait for the return code of the following statement to be zero
        /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P globalMAQS2 -d master -C -Q "SELECT TOP 1 message_id FROM sys.messages"
        not_ready=$?

        if [ $not_ready != 0 ]
        then
            echo "Could not contact sql server, will try again in 5 seconds."
            sleep 5s
        fi
    done

    echo "Started initializing database"
    # Set up the schema and stored procedures
    /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -C -P globalMAQS2 -d master -i `dirname $0`/schema.sql
    /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -C -P globalMAQS2 -d master -i `dirname $0`/stored_procedures.sql
    # Use BCP to import test data
    /opt/mssql-tools18/bin/bcp GlobalAutomation.dbo.States in "`dirname $0`/SeedData/GlobalAutomation/States.bcp" \
        -n -u -S localhost -U sa -P globalMAQS2
    /opt/mssql-tools18/bin/bcp GlobalAutomation.dbo.Cities in "`dirname $0`/SeedData/GlobalAutomation/Cities.bcp" \
        -n -u -S localhost -U sa -P globalMAQS2
    /opt/mssql-tools18/bin/bcp GlobalAutomation.dbo.Datatype in "`dirname $0`/SeedData/GlobalAutomation/Datatype.bcp" \
        -n -u -S localhost -U sa -P globalMAQS2
    echo "Finished initializing database"
}&

# Start SQL server
exec /opt/mssql/bin/sqlservr