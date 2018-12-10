CSharp Implementation of GroupAL (group formation algorithm)

Update 04.12.2019

- The project is now a console application
- The Entrypoint is RestService.StartService.cs
- Running the project with mono on windows:
-- install Mono 4 or higher
-- use Mono x86 command prompt
-- xdbuild GroupFormationAlgorithm.sln
-- mono bin/Debug/GroupFormationAlgorithm.exe
-- the API will be published at http://localhost:12345/users/preferences/{GroupSize}

- Actually running the project in linux
-- install Mono last stable using yum or apt-get, we used: https://www.mono-project.com/download/stable/#download-lin-centos
-- download the dependencies using this idea: https://blog.lextudio.com/how-to-use-nuget-on-mono-part-i-8d2cd63bd1e0
   (in principal, you could also copy Nancy.dll, Mono.Posix.dll and Nancy.Hosting.Self to your linux machine)
-- xdbuild GroupFormationAlgorithm.sln
-- mono bin/Debug/GroupFormationAlgorithm.exe
-- the API will be published at http://[your-server.ip]:12345/users/preferences/{GroupSize}

- Using the API
-- send a POST request to the API with XML data on the payload describing the participants. An example xml can be found in the folder /bin/Debug/
-- the request will return data as json or xml (depending on the header) depicting the cohort that was created
-- NOTE: The value names "value0", "value1" ... are important and must be named exactly like this because the index will be parsed from [0,1,..]. Dont' ask me. Just do it.
