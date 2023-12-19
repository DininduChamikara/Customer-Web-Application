# Customer-Web-Application

## Admin User Credentials
Email - admin@gmail.com

Password - Admin@123

## API User Credentials
Email - customer@gmail.com

Password - Customer@123

## Instructions
* Once you log into the system you can see the Login Page.
* If you are not registered, you can register to the system by using the register page.
* If you registered as an Admin you can visit the Admin Page and see the details about the customers.
* If you registered as a customer you can not access the Admin Page.
* If you want to access the APIs you can visit the link below,
    https://localhost:7135/swagger/index.html

* You can generate the JWT token using your login credentials
```markdown
{
  "email": "admin@gmail.com",
  "password": "Admin@123"
}
```
* After generating the token you can also access the other APIs.
* You can edit the customer name, email, and phone number using Customer ID. Here is the sample request body.
```markdown
{
    "_id": "5aa252be5d1e07697b16d463",
    "Name": "Cain Changed",
    "Email": "caingoodman@99sportan.com",
    "Phone": "+1 (993) 518-3773"
}
```
* You can get the distance calculated in kilometers using Customer ID, longitude, and latitude.
```markdown
{
    "CustomerId": "5aa252be5d1e07697b16d463",
    "Latitude": "37.7749",
    "Longitude": "34.0522"
}
```
* You can search the customers by using search text.
* You can list all Customers of the system by their "zip code".
