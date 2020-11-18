# CustomerDrivenProject
Customer driven project, group 6 with Sintef Ocean

# Installation guide

## Program dependencies and requirements
We list the required software for running this application, with links for where to download the software.
### .NET core 3.1
The back end is built using .Net core version 3.1, and is therefore needed for running the application development server, and release compilation.
The required software can be downloaded from the [official .Net download site](https://dotnet.microsoft.com/download/dotnet/current).
### Node
Node is required for running the project front end. Development is done with the current (10.2020) LTS version of node which, at the time of writing, is version v12.19.0 with the package manager npm of version 6.14.8.
The required software can be downloaded from [the Node.js webiste](https://nodejs.org/en/download/).
### Timescale / PostgreSQL
TimescaleDB is the selected database for this project. It can be downloaded from [the Timescale website](https://docs.timescale.com/latest/getting-started/installation/). It is recommended to run this through docker using the image: `timescale/timescaledb:latest-pg12`.

### Docker
Docker is used for several aspects of the application. Docker is used for deployment of the application, and for running the database as this is convenient for development purposes. The [docker web-page](https://www.docker.com/) has all the necessary information for installation of docker on different systems, and use of docker.

### Azure
The project requires an Azure account for configuring login through Azure.

### Python
Python v3.6+ is needed for running the Python program for automatically populating the database with test data to simplify the testing process of the program.

## Running in development
The application can run or be deployed in a development state with the use of development servers on your local machine.
Python can be downloaded from [the official Python website](https://www.python.org/).

### Configuration

#### **Azure configuration** 
The Azure configuration is done in the [Azure portal](https://portal.azure.com/#home).
How to set up projects for verification with Azure, will now be shown.
First, select `Azure Active Directory`, to get to the portal.

##### **Groups**
Under the `Groups` tab to the left we will need to create 3 new groups. This is done with the `new group` button at the top.
These groups must be of type ``security``, and the names are listed below.
1. Customer
2. Engineer
3. Researcher

The IDs of these groups are needed in the configuration of the front end later in the configuration.

##### **Users**
For the application to be accessible, users must first be invited.
Select the tab `Users` (To the left, right above groups), and create a new user with the `New user` button.
Select the `Invite user` option and enter the name and email of the user.
After this, the user groups needs to be added. Under `groups and roles` add groups to users.
To access all features of the application you should add all the groups to the user.

##### **Register API application**
Following this, we need to register the API application.
Select the tab `App registrations` from the menu to the left, located half way down the list.
Register the new app with the `New Registration` button at the top. 

This will be the API-application registration. Register a name, single tenant, and no redirect URI.
When the application is registered, we are redirected to a new page for configuring this application.

Now we need to expose the API. This is done at the `Expose an API` tab in the menu on the left.
Here we need to add the necessary scopes of the application to allow for access to the API.
When adding a new scope, we are prompted for the `Application ID URI`. Select the default.
We will add two scopes with names:
- `access_as_user`
- `User.Read`

Select `Admins and Users` under "Who can consent?", and save.

##### **Register client application**
We will register a second application, which is the client application.
Go back to the `Azure Active Directory` by using the 'home' link at the topp, or by once again opening the portal home with the [same link](https://portal.azure.com/#home) used earlier.
Select the tab `App registrations` from the menu on the left, located half-way down the list.
Register the new app with the `New Registration` button at the top. 
Select a name, single tenant and no redirect URI.
We are then redirected to the config of the new application.

We will now register the redirect URIs.
This is done under the `Authentication` tab located in the menu on the left.
For running the application on localhost, add the redirect URIs `http://localhost:3013/` and `http://localhost:3013/auth.html`.
When this is done, we need to allow for usage of access tokens. Scroll down on the page and check the `Access token` checkbox under `Implicit grant`.
Then change the supported account types from single tenant, to multitenant.

We then need to add API-permissions for the client application.
Select the `API permissions` tab, and use the `Add a permission` button at the top. 
Under `APIs my organization uses` select the API application created above and allow for `access_as_user` and `User.Read` permissions.
When this is done, be sure to grant the permissions with the `Grant admin consent` button next to the `Add permissions` button.

We then need to configure the token. Select the `Token configuration` tab. 
Use the `Add groups claim` button to add the user groups as claims in the token.
Select `Security groups` and save.

When this is done, all needed values for configuring Azure authentication is created.
These will be references in the `Frontend config` and `Backend config` sections later.



#### Database config
The database should not need any more configuration if serving with docker.

#### Front end config
The front end configuration is the same for development and production.
The configuration is done in the `appsettings.json` file under `frontend/src/src/appsettings.json`.

The `apiUri` defines the location of the back end API. By following this guide, this will be at `http://localhost:5000`.

The `AAD` defines the Azure configuration. These values were configured in section [Azure configuration](###Azure-configuration).
All of these values can be found in Azure under the two applications that are registered.
- `ClientId` is the value of the client ID of the client application registered.
- `Authority` is composed of `https://login.microsoftonline.com/`, followed by the client application `TenantId`.
- `RedirectUri` is the same as defined in the client application. If the guide above is followed this should be `https://localhost:3013`.

- `Scopes` should be a list of the registered API application from Azure. In this case this is the `Application ID URI` of the API application, followed by the scopes `user.read` and `access_as_user`. An example of this is given: `api://0ea5e8ad-2ac5-4ea4-bb39-74213a21e4f4/user.read`.


#### Back end config
For the back end to run properly, a few changes to the application settings are necessary.
Under the `src` directory at `backend/src`, the config file `appsettings.json` contains the back end configuration.

The variable ``DatabaseConnectionString`` under ``DatabaseConfig`` needs to be altered to connect to the database configured. Such a connection string should be on this format:
- ``Host=<databaseHost>;Username=<username>;Password=<password>;Database=<databaseName>``.

A database served on the local machine would give the connection string:
- ``Host=localhost;Username=postgres;Password=password;Database=fishfarm``. (See [Database](##Database))

The values of `AAD` is the Azure configuration.
These values comes from the API project defined under the [Azure config](###Azure-configuration) section.
- `ResourceId` is the value of `Application ID URI` from Azure.
- `TenantId` is the value of `Directory (tenant) ID` from Azure.


### Database
The database can be served by installing Timescale on the development machine, or through docker.
The application function irregardless of the database hosting option, but this tutorial is going to show how to do this with docker in a few easy steps.
1. Create the docker container of Timescale DB
(This creates the init of the database and creates a superuser postgres with password "password", + exposes the default postgres port 5432)

    ``
    docker run -d --name timescaledb -p 5432:5432 -e POSTGRES_PASSWORD=password timescale/timescaledb:1.7.4-pg12
    ``
2. When the container is created, hook into the container to run psql command, and create a new database 'FishFarm'.

    `` docker exec -it timescaledb psql -U postgres -c 'CREATE DATABASE FishFarm;' ``

    *NB! FOR WINDOWS: We have experienced problems running this command in Windows CMD. Using Powershell should resolve this issue.*

3. For using the database, a user with access is needed. We will here use the default superuser. This is not recommended for a production case, but will suffice in our prototype application.

4. We need to create the data tables. The tables are defined in the `database.sql` file under `backend/src/Database/database.sql`. We use this sql script for creating the tables. The `database.sql` file should be in the current directory of the interactive terminal used. Then run the command below to create the needed tables.

    `` docker exec -i timescaledb psql -U postgres -d fishfarm < database.sql ``

    *NB! FOR WINDOWS: This command will not work in powershell natively. We suggest using "WLS" or "CMD" to run this command in windows. This is because the piping method "<" is not supported in powershell.*

When this is done, the database fishfarm should be available at `localhost` port `5432`, with all tables created.

To create a test database, repeat step 2 - 4 with the database name `fishfarmtest`.

### Back end
The back end application is located under the `backend/` directory.
It is developed using .Net version 3.1, and is therefore needed to run the back end.
To run the back end, run the commands given below when located in the `backend/` directory of the project.
1. ``dotnet restore``
2. ``dotnet watch --project src/ run``

This starts the development server of the back end, serving the GraphQL API with `http` at [http://localhost:5000](http://localhost:5000) and `https` at [https://localhost:5001](https://localhost:5001) by default.

The API also serves a web interface with endpoint documentation and interaction. This can be accessed at the `/playground` path, [http://localhost:5000/playground](http://localhost:5000/playground).

NB! Remember to not exit the back end API runtime, as this will close the API.


### Front end

Navigate into the folder `frontend/src`.

The front end application is located under the `frontend/src/` directory. To compile or run in development, the Node and npm software is required.
To run the development server, first open a terminal with current directory `frontend/src/` relative to the project. While in this directory, run the commands given below to install package dependencies, and start a development server.
1. ``npm install --save``
2. `` npm start ``

When this is done, the front end client is hosted locally at post `3013`, found at [http://localhost:3013](http://localhost:3013).

NB! The server will automatically start a new tab in your browser with the correct port. 

## Deploy in production
The project deployment was accomplished through the use of docker.

### Configure
#### Azure
The Azure config is the same as in the development configuration, with the exception of the return path.
The return URI must be configured for the system where the project is deployed. Se [Register API application](####Register-API-application) under dev config for how to do this.

#### Nginx
The front end is served with nginx and docker. The configuration of nginx is done through one of the two ``.conf`` files in the ``nginx`` directory.
The ``nginx.localhost.conf`` adds a nginx configuration for serving the react front end over HTTP.
The ``nginx.prod.conf``configures nginx to serve the front end client over HTTPS, and thus needs the correct SSL certificates, which we do not supply here.

#### Back end and front end
The front end and back end applications need to be configured in the same way as for deploying in development. The back end configuration is done through the ``appsettings.json`` file under ``./backend/src``, and the front end is configured as in development.
Check this in the [Configuration section](##Configuration).

#### Docker
For configuration of the created docker images, the two dockerfiles can be configured, however this is not necessary.
The creation and start of the containers are configured with the ``docker-compose.yml`` file at root level.

The front end docker image composition can be configured as `Dockerfile.frontend.prod` under the `frontend/` directory.

The back end docker image composition can be configured as `Dockerfile.backend.prod` under the `backend/` directory.

With the current configuration the front end will be served with nginx at port 3013, and the back end will be served at port 3014.


### Build release versions
#### Back end
To deploy the back end, we first compile the project for production. This is done in two simple steps with the commands below. (Assumes the current directory is `backend/src/` )
1. ``dotnet restore``
2. ``dotnet publish -c Release``

This results in compiling the project for deployment. The build files can be found under `backend/src/bin/Release`.
#### Front end
To deploy the front end with nginx we need to compile the react application. This is done with two simple commands, as shown below. (Assumes the current directory is `frontend/src/` )

1. ``npm install``
2. ``npm run build``

The result of this compilation is the compiled project at `frontend/src/build`. 

### Dockerize and deploy
When both the back end and front end are compiled, we can use docker to build images for these two services.
The docker images to be compiled is defined in different "dockerfile" files.  

The back end application is based on the .Net docker image ``mcr.microsoft.com/dotnet/core/aspnet:3.1`` and uses the compiled files from the publish command above.

The front end image used is the Nginx image ``nginx:1.16.0-alpine``. The nginx configuration used can be found under the nginx directory. This config file is used as the config file for nginx hosted with docker.

The back end can be served with docker as defined under the development section. See [#Database](#Database).

We build and deploy the front end and back end locally using docker-compose.
The file ``docker-compose.yml`` contains the configuration of the compose.
The images are built with the command `` docker-compose up -d --build ``.

When this is done, the application should be available. The front end is available at [http://localhost:3013](http://localhost:3013), and the back end is available at [http://localhost:3014](http://localhost:3014) by default.

## Populating the application with data
To test the application, a Python script was created for populating the database with some metadata.
This is done with a Python script located under the `backend/scripts/metadata` directory.
The data files for the script to run, is supplied in the `data` sub-directory.
The Python dependencies are defined in the `requirements.txt` file.
Running the `insert_metadata.py` script automatically populates the database with metadata.

# User Guide
## Log in to the System
A. Log in by clicking the “Sign In” button shown in  Figure 1 and filling in your credentials. You need to be granted access to log into the system. If you do not have access, contact your IT administrator.

B. If you have Two Factor Authentication (2FA) set up on your Microsoft account, you will be prompted to a screen like in Figure 2. You may click the button “Next” or “Neste” and proceed with the instruction on the screen for signing in.


![Sign in](https://github.com/Sanderkk/CustomerDrivenProject/blob/dev/docs/images/user_guide/sign_in.PNG)
Figure 1
![2FA](https://github.com/Sanderkk/CustomerDrivenProject/blob/dev/docs/images/user_guide/login_2fa.PNG)
Figure 2

## Researcher
### 1. Add a New Dashboard

A. Navigate to the dashboards tab by clicking “Dashboards” in the navigation bar,  marked as 1 and with orange outline in Figure 3.

B. Click the “Create Dashboard” button 2a for creating a new dashboard. When your dashboards page is empty, button 2b is visible and has the same functionality as 2a. Both 2a and 2b are marked with a green outline.

C. You will be prompted to a new screen, as shown in Figure 4, which is the screen for creating a new dashboard. The dashboard title and description can be changed by editing the text fields marked with 3 and 4, and changes can be saved by clicking on the “Save” button marked with 6 and a purple outline.

D. A new dashboard is created and can be viewed by navigating to the dashboard tab in the navigation bar, as described in step A above.

![Add a new dashboard 1](https://github.com/Sanderkk/CustomerDrivenProject/blob/dev/docs/images/user_guide/dashboard_empty.PNG)
Figure 3

![Add a new dashboard 2](https://github.com/Sanderkk/CustomerDrivenProject/blob/dev/docs/images/user_guide/dashboard_add_new.PNG)
Figure 4

### 2. Add a New Cell to a Dashboard

A. To add a cell to a given dashboard, navigate to the dashboard tab as described in step A of “2. Add a new Dashboard” section and select a dashboard by clicking on it.

B. Click on the “Add Cell” button marked as 7 with red outline, shown in Figure 4, to create a new cell. 

C. To add a sensor to the cell, select a sensor type and sensor number in the checkboxes shown in Figure  5. This is marked as 1 with green outline and 2 with orange outline in Figure 5. 

D. (optional) Repeat step C for each sensor you want to add to the cell.

E. Thereafter, click the object marked as 3 with a red outline in Figure 5 to show the calendar pop-up, shown in Figure 6. Select the wanted time interval by clicking the start and end date.

F. Graph options can be added to the cell by filling the form marked as 4 with purple outline in Figure 5. 

G. Metadata for the sensors in the cell can be viewed by clicking the “View Metadata” button marked as 5 with pink outline in Figure 5. A new popup screen appears, as shown in Figure 7.

H. To add the cell to the dashboard, press the “Save Cell” button marked as 6 with red outline in Figure 5.

![Add Cell](https://github.com/Sanderkk/CustomerDrivenProject/blob/dev/docs/images/user_guide/cell_edit_double_filled.PNG)
Figure 5

![Date widget](https://github.com/Sanderkk/CustomerDrivenProject/blob/dev/docs/images/user_guide/cell_dato_object.PNG)
Figure 6

![Metadata new cell](https://github.com/Sanderkk/CustomerDrivenProject/blob/dev/docs/images/user_guide/dashboard_created_metadata.PNG)
Figure 7

### 3. Cell Operations

A. Navigate to the dashboard tab as described in step A of “2. Add a new Dashboard” section. Enter the dashboard for which you want to perform cell operations. 

B. Hover over the “cog” icon on the top right of the desidered cell. This will show three options, as shown in Figure 8.
 
C. To edit a cell, press the “Edit graph” button marked as 2 with green outline in Figure 8. Clicking the button redirects you to the same screen as shown in Figure 5. You can thereafter modify the cell. The changes are saved when you click the “Save Cell” button, marked as 6 with red outline in Figure 5.
 
D. To view the sensor's metadata for that cell, click the “Metadata” button marked as 3 with red outline in Figure 8. A pop-up with metadata appears, as shown in Figure 9.

E. To delete a cell, click the “Delete” button marked as 4 with orange outline, shown in Figure 8.

![Created Dashboard Options](https://github.com/Sanderkk/CustomerDrivenProject/blob/dev/docs/images/user_guide/dashboard_created_options.png)
Figure 8

![Created Dashboard Metadata](https://github.com/Sanderkk/CustomerDrivenProject/blob/dev/docs/images/user_guide/dashboard_created_metadata.PNG)
Figure 9

## Engineer

### 1. Edit Metadata of a Sensor
A. Navigate to the admin page by clicking “Admin Page” in the navigation bar.

B. Navigate to the metadata tab by clicking the “Metadata” tab marked as 1 with brown outline in Figure 10.

C. Type the sensor number in the textbox ,marked as 2 with a green outline in  Figure 10, and click the adjacent “Search” button. Autocomplete will assist you in finding the right sensor.

D. A form, like the one marked 3 with blue outline in Figure 10, containing metadata for that sensor will appear. Select and edit some of the fields. The calendar pop-up, shown in Figure 6, will appear when editing dates.

E. Click the “Save” button, marked as 4 with orange outline in Figure 10, to update the metadata for that sensor.

![Edit Metadata](https://github.com/Sanderkk/CustomerDrivenProject/blob/dev/docs/images/user_guide/admin_edit_metadata.PNG)
Figure 10

### 2. Add a New Sensor to the System

A. Navigate to the metadata tab by following steps A and B of the “5. Edit Metadata of a Sensor” section.

B. Write the number of the new sensor in the search field, marked as 1 with dark blue outline in Figure 11. When clicking the adjacent “Search” button, and the sensor number is not registered in the database, the card shown in Figure 11 appears. If you want to add a new sensor, click the button “Yes”, marked as 2 with purple outline.This will make an empty form appear, marked as 3 with red outline in Figure 12.

C. Fill out the form with metadata for the new sensor. The required fields are marked with a red star.

D. Save the new sensor by clicking the button “Save” marked as 4 with orange outline in Figure 12.

![Add Metadata 1](https://github.com/Sanderkk/CustomerDrivenProject/blob/dev/docs/images/user_guide/admin_new_metadata.PNG)
Figure 11

![Add metadata 2](https://github.com/Sanderkk/CustomerDrivenProject/blob/dev/docs/images/user_guide/admin_edit_new_metadata_updated.png)
Figure 12

### 3. Upload Sensor Data to Existing Sensors

A. Navigate to the admin page by clicking “Admin Page” in the navigation bar. A screen as shown in Figure 13 will appear.

B. Make sure you are on the “Upload data” tab. If not, press on the “Upload data” tab marked as 1 with a green outline in Figure 13. 

C. Click the “Velg fil” button next to “Data file:” in the red square marked as 2 in Figure 13 and select the data file you want to upload from your computer.

D. Click the “Velg fil” button next to “Config file:” in the red square marked as 2 in Figure 13, and select the config file you want to use. The config file is a JSON file used to parse the data file. An example of a config file is shown in backend -> services -> parser -> metaocean.json in the GitHub project.

E. Click the “Upload” button, marked as 3 with orange outline in Figure 13, to upload the data. A feedback message will appear below the “Upload” button, showing whether the operation succeeded or failed.

![Upload data](https://github.com/Sanderkk/CustomerDrivenProject/blob/dev/docs/images/user_guide/admin_upload.PNG)
Figure 13

### 4. Sign Out of the System

You can sign out of the system by hovering on your name in the navigation bar, marked as 1 with red outline in Figure 14, and clicking the “Sign out” button marked as 2 with green outline. This can be done from any of the screens. 

![Sign out](https://github.com/Sanderkk/CustomerDrivenProject/blob/dev/docs/images/user_guide/logout.png)
Figure 14

## Customer

### Visualize a dashboard

Log in to the system as explained in the “Log in to the System” section. This will prompt you to a screen similar to Figure 15, with all the available dashboards. By clicking on the desidered dashboard button, a new screen similar to Figure 16 will appear. Information about this dashboard and the graphs will be available at this page.

![All customer's dashboards](https://github.com/Sanderkk/CustomerDrivenProject/blob/dev/docs/images/user_guide/customer_all_dashboards.png)
Figure 15

![Chosen customer dashboard](https://github.com/Sanderkk/CustomerDrivenProject/blob/dev/docs/images/user_guide/customer_dashboard.PNG)
Figure 16
