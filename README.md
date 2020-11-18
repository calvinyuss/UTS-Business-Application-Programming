<h1 align="center">UTS-Business-Application-Programming</h1>
<h1 align="center"> <a href="https://github.com/calvinyuss/UTS-Business-Application-Programming/releases/tag/v1.0-beta">Lastest Release</a> </h1>
Made to fullfil the Business Application Programming midterm 7 exam.

## About The Project 
This project is made for a restaurant order system, Which is the customers/users can place order directly from their table. 
This Project made by winform and sql server

## Build With
- .NET Framework 4.7.2
- C#
- Winforms
- SQL Server

## Installation

Of course the first time you need to clone the project, and after clone you can open with your lovely visual studio :smiley:
```
git clone https://github.com/calvinyuss/UTS-Business-Application-Programming.git
```


You also can download directly from [here](https://github.com/calvinyuss/UTS-Business-Application-Programming/releases/tag/v1.0-beta)


To use the application you need to update the connection string to your server.

You need to modify both at the frontend and backend, Don't forget to migrate the database with [DB_DATA.sql](https://github.com/calvinyuss/UTS-Business-Application-Programming/blob/master/DATA_DB.sql)
```XML
<connectionStrings>
  <add name="DB_DATA" connectionString="data source=CY\SQLEXPRESS;initial catalog=DB_DATA;integrated security=True;"/>
  <add name="DB_DATAEntities" connectionString="metadata=res://*/Models.csdl|res://*/Models.ssdl|res://*/Models.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=CY\SQLEXPRESS;initial catalog=DB_DATA;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient"/>
  <add name="UTS_BACKEND.Properties.Settings.DB_DATAConnectionString" connectionString="Data Source=CY\SQLEXPRESS;Initial Catalog=DB_DATA;Integrated Security=True" providerName="System.Data.SqlClient"/>
</connectionStrings>
```

Note: you need to login to view the admin page
```
username: admin 
password: password
```
Note: The image location will be stored at your drive D `D:\resources\images`

image file you find it ![here](https://github.com/calvinyuss/UTS-Business-Application-Programming/blob/master/resources.rar)

![Create Menu](https://res.cloudinary.com/cy-uph-student/image/upload/v1605612077/UTS-BAP/1605550102968_ujbxlp.gif)
![Update Menu](https://res.cloudinary.com/cy-uph-student/image/upload/v1605612063/UTS-BAP/1605550178443_dqqxl4.gif)
![Admin Page](https://res.cloudinary.com/cy-uph-student/image/upload/v1605613433/UTS-BAP/1605613412786_eiyydq.jpg)

![Client View](https://res.cloudinary.com/cy-uph-student/image/upload/v1605613349/UTS-BAP/1605613315571_mtfg7a.jpg)
![Create Order](https://res.cloudinary.com/cy-uph-student/image/upload/v1605612991/UTS-BAP/1605612952711_twrmnm.gif)
