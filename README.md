BillingNextSys
# **How to contribute?**

1. Download latest dotnet core 3.1.102 SDK from Microsoft site: [here](https://dotnet.microsoft.com/download)
1. Clone the github repo from here: https://github.com/ruchit-patel/BillingNextSys/tree/WebDeploy-mnt
1. Download PostgresSQL 10.12 from https://www.enterprisedb.com/downloads/postgres-postgresql-downloads and make sure it runs on your proffered port number. 
1. Create database by: 'Create database "billingnext" '
1. Run SQL Script named "migrate.sql" placed at project root folder or you can [find it here](https://github.com/ruchit-patel/BillingNextSys/blob/WebDeploy-mnt/BillingNextSys/BillingNextSys/migrate.sql), on your newly created fresh-fresh database to set up all those tables you'll need.   
1. Open appsettings.json file and do exactly as I say: 
  6.1 Navigate to lines that spells as below:  

    "BillingNextSysIdentityDbContextConnection": "Server=[p1]; Database=[p2]; User Id=[p3]; Password=[p4]; ",
    "BillingNextSysContext": "Server=[p1]; Database=[p2]; User Id=[p3]; Password=[p4];"

    6.2 Fill the placeholders with actual values. If you are done with this. You've already won the battle. XD 


| Placeholder| What it means? |
|--|--|
| p1 |ip where db is hosted ("127.0.0.1" or "localhost" In case you host on localhost, which you probably will)  |
| p2 | Name of database (propably billingnext) |
| p3 | postgre user id (default is postgres) |
| p4 | Postgre user Password |


7. Build the project in your favourite IDE.
1. You are done with the process of setting up the perfect environment. 

Wait wait.. what about PR Format?
Well, we don't do that here :P . Just Kidding, we'll update you once we are ready with one or may be you can create and commit one if you like. :)   