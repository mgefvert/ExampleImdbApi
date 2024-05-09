# Example IMDB API

This is an example project for demonstrating, among other things, generic CRUD
services and controllers, as well as decorating each request with logging.

It uses, for really no good reason, the public IMDB datasets available at 
https://developer.imdb.com/non-commercial-datasets/. Running ImdbDataLoader on
the full dataset will take hours.

## Generic CRUD controllers

The class **ImdbApi/GenericControllers/AbstractCrudController.cs** implements a very
generic API providing different API endpoints for creating, fetching, listing,
updating and deleting data records.

This is implemented by `ImdbTitleBasicController`. As you can see, it is a very
minimal controller, yet it provides an easy-to-consume and full-featured CRUD interface
for external applications.

## Generic CRUD services

The **ImdbData/GenericServices/CrudService.cs** file implements a generic service
using a DbContext for all of the API methods used by the CRUD controller. It
provides a common way to create, list and fetch, update and delete objects.

Individual methods can be allowed/disallowed by setting `AllowedOperations`.

The actual services are implemented in **ImdbData/Services**. Since `CrudService` is
abstract, there are a few overrides needed to provide a functional service.

- `AssignObject` is a method that lets you decide how to copy fields from an
  entity over HTTP onto an existing object.
- `GetObjectKey` is required to tell the system how to find an entity key value.
- `LoadQuery` is the basic for all list and get operations, it builds a basic
  query with all appropriate safeguards that the individual methods use as
  the start.
  
A couple of generic parameters are required:

- `TDataKey` - the type for the entity key (typically `int`)
- `TDataObject` - the actual entity type
- `TDataContext` - the type of the DbContext
- `TListQuery` - a class that provides the customization for the List method.

## Logging

A logging object of type `ApiLogRequest` is available in the current scope; this
provides easy methods to log information about the current request.

`Log.Request(data)` adds log data about the _http request_, i.e. query parameters,
ID's passed in, or anything like that.

`Log.Data(data)` adds log data about the _result of the request_, i.e. objects
returned, data exchanged, etc. It is not set in stone, for log brevity, the input
objects to Update or Create may also be logged with Log.Data.

The actual logging is performed in the middleware layer `ApiLoggingMiddleware`.

## Testing

Testing is not complete yet.
