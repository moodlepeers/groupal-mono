CSharp Implementation of GroupAL (group formation algorithm)

Update 04.12.2019

- The project is now a console application
- The Entrypoint is RestService.StartService.cs
- Running the project on linux:
-- install Mono 4 or higher
-- use Mono x86 command prompt
-- xdbuild GroupFormationAlgorithm.sln
-- mono bin/Debug/GroupFormationAlgorithm.exe
-- the API will be published at http://localhost:12345/users/preferences/{GroupSize}

- Using the API
-- send a POST request to the API with XML data on the payload describing the participants. An example xml can be found in the folder /bin/Debug/
-- the request will return data as json or xml (depending on the header) depicting the cohort that was created