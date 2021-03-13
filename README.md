# EnsekInterview

## Setup

From the Ensek.MeterReading.Database project, run the publish profile, pointing this at your local instance 
of SQL.  This will create the schema and seed the initial records.

## Running

Make sure the connection string in Ensek.MeterReading.Data.Api is pointing at your newly created DB.  Default
values will point at the default sql instance on your local pc using integrated security, so will hopefully 
"just work"!

To run, start the DB's API layer (Ensek.MeterReading.Data.Api) and the user facing api (Ensek.MeterReading.Api).
The endpoint required by the spec, which allows you to submit your csv file is in the Ensek.MeterReading.Api project.

Both projects should start to a sswagger home page letting you easily try them out.

There's a bunch of unit tests in there too covering the main bits of functionality.