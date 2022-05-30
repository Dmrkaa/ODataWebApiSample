# Introduction to OData
[The Open Data Protocol](https://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part1-protocol.html#sec_Introduction)  (OData) enables the creation of REST-based data services which allow resources, identified using Uniform Resource Locators (URLs) and defined in a data model, to be published and edited by Web clients using simple HTTP messages. This specification defines the core semantics and the behavioral aspects of the protocol.  
We will consider working with the latest OData package version for now (8.0.10) for .net core.
## Prerequisites
1. Adding the Correct NuGet Package: Microsoft.AspNetCore.OData (version 8.0.10)
2. Defining an Entity Data Model. The Entity Data Model, or EDM, is the abstract data model that is used to describe the data exposed by an OData service.
**Look at EDM folder in this project.**
3. Register it at Startup.cs 
```C#
services.AddControllers()
        .AddOData(opt => opt.AddRouteComponents("odata", new ShopEDM().GetEntityDataModel())
```
4. Also i will use ef core in-memory database for example. So u dont need to have mssql server or anything else for try.
Install Microsoft.EntityFrameworkCore.InMemory (i use 5.0.17), register it at startup.cs
```C#
services.AddDbContext<AppDataContext>(opt => opt.UseInMemoryDatabase(databaseName: "ShopDb"));
```
 and create DbContext class. Check it at Data folder.  
 Ok, now u can run it and try **https://localhost:yourport/odata/** That "odata" prefix is coming from when we registered the Odata model.  
 This gives you the service document. 
 ```json
 {
    "@odata.context": "https://localhost:44350/odata/$metadata",
    "value": [
        {
            "name": "Brand",
            "kind": "EntitySet",
            "url": "Brand"
        },
        {
            "name": "Product",
            "kind": "EntitySet",
            "url": "Product"
        }
    ]
}
```
Add $metadata to the URL, and you get the [metadata](https://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part1-protocol.html#sec_MetadataRequests) document. **ex https://localhost:44350/odata/$metadata**
```xml
<?xml version="1.0" encoding="utf-8"?>
<edmx:Edmx Version="4.0" xmlns:edmx="http://docs.oasis-open.org/odata/ns/edmx">
    <edmx:DataServices>
        <Schema Namespace="Shop" xmlns="http://docs.oasis-open.org/odata/ns/edm">
            <EntityType Name="Brand">
                <Key>
                    <PropertyRef Name="Id" />
                </Key>
                <Property Name="Id" Type="Edm.Int32" Nullable="false" />
                <Property Name="Name" Type="Edm.String" />
                <NavigationProperty Name="Products" Type="Collection(Shop.Product)" />
            </EntityType>
            <EntityType Name="Product">
                <Key>
                    <PropertyRef Name="Id" />
                </Key>
                <Property Name="Id" Type="Edm.Int32" Nullable="false" />
                <Property Name="Name" Type="Edm.String" />
                <Property Name="Price" Type="Edm.Decimal" Nullable="false" />
                <Property Name="BrandId" Type="Edm.Int32" Nullable="false" />
            </EntityType>
            <EntityContainer Name="ShopContainer">
                <EntitySet Name="Brand" EntityType="Shop.Brand">
                    <NavigationPropertyBinding Path="Products" Target="Product" />
                </EntitySet>
                <EntitySet Name="Product" EntityType="Shop.Product" />
            </EntityContainer>
        </Schema>
    </edmx:DataServices>
</edmx:Edmx>
```
## The main goal
Imagine that you have some shopWebApi. With endpoints like this for example.
```c#
        [HttpGet]
        [Route("Products")]
        public IEnumerable<Brand> GetProducts()
        {
            var result = _context.Brands;
            return result;
        }
```
And one day you need to provide a certain sample of data. Ofc u can add some methods with filters to your repository or CQRS queries. But what about ordering, selecting, counts etc.  
Querying data is where OData really helps. Just add **EnableQuery** attribute for your Get endpoint like this
```c#
        [HttpGet]
        [EnableQuery]
        [Route("Products")]
        public IEnumerable<Brand> GetProducts()
        {
            var result = _context.Brands;
            return result;
        }
```
### Select
And now u can get **https://localhost:44350/Shop/Brands?$select=name** for select just only Brands names.  
**Response**
```json
[
    {
        "Name": "Apple"
    },
    {
        "Name": "Samsung"
    }
]
```
### Filter
**https://localhost:44350/Shop/Products?$filter=price ge 900** for request products with price greater than 900
**Response**
```json
[
    {
        "id": 1,
        "name": "12ProMax",
        "price": 1000,
        "brandId": 1
    },
    {
        "id": 2,
        "name": "12",
        "price": 950,
        "brandId": 1
    }
]
```
**More about commands and syntax [here](https://docs.microsoft.com/ru-ru/azure/search/query-odata-filter-orderby-syntax)**
## ODataController
 To define an OData service, simply derive from ODataController.  This is  a base class for OData controllers that support writing and reading data using the OData formats. It derives from ControllerBase, so it still supports most of the controller-related actions we'd expect when building an API, but instead of using the default routing, it supports OData routing principles.
 U can reach it by your "odata" prefix from startup.cs In our case it is **https://localhost:44350/odata/Brands/1** etc.
 
 Hope it helps. Thanks for reading. Hit the star button))
