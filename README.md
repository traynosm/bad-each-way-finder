# bad-each-way-finder

This is the front end application. The back end API must be running prior to running the front end.

Please find the link to the back end repository https://github.com/traynosm/bad-each-way-finder-api

Please follow the README.md for the back-end project prior to running this front end project.

Both applications should ideally this should be run on Visual Studio 2022 alongside Microsoft SqlServer Management Studio 18.

Update the connection string settings in appsettings.json in both bad-each-way-finder and bad-each-way-finder-api. They are currently configured to the developers machine. 

```python
"ConnectionStrings": {
    "BadEachWayFinderApi": "Data Source=[Placeholder]"}
```

Now run the bad-each-way-finder.

The index page will load first. In order to view Races, Propositions and History page, the user must be logged in.

A user can be registered by clicking on the register button on the index page. 

