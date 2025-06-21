uses .Net Core 8 and Angular 18 with Material UI
DB is SQLite.
 

Before running the project, make sure the following are installed:

install required angular packages via npm install

---
 
on env folder/environment.ts configure/specify the addresss/port to be used. 
in my case  apiBaseUrl: 'https://192.168.100.9:5001'

on program.cs, configure and make sure ip address/port are matched;
please check configure kestrel(builder.WebHost.ConfigureKestrel) and AddCors (builder.Services.AddCors) configurations.

logins: albert@test.com/123456ABCDEFG
