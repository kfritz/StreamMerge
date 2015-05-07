# StreamMerge for Peloton
This project is a submission for the [Peloton](https://www.pelotoncycle.com) quiz.  It interfaces with the Peloton API to retrieve sequences of increasing integers from named streams.  It provides an HTTP endpoint that, when given the name of two integer streams, will retrieve values from each and merge them together, maintaining increasing order.

## Implementation Details
The project is implemented using the new [ASP.net 5 framework](http://www.asp.net/vnext), which was put into release candidate at the [Microsoft Build conference on April 29, 2015](http://www.buildwindows.com).  This framework was selected so that the author could use this opportunity to experiment with the new technology.  The code is organized into three projects.

The StreamMerge project is the ASP.net web application that hosts the HTTP endpoint.  It has one controller, `QuizController`, that handles incoming HTTP requests.  The controller uses attribute based routing to map the path, `/quiz/merge`, to the `Merge` action on  `QuizController`.  Its `Startup` class is responsible for initializing the [ASP.net MVC 6 framework](http://www.asp.net/vnext/overview/aspnet-vnext/create-a-web-api-with-mvc-6) and configuring dependency injection.

The StreamMerge.App project is a class library that contains the core implementation for issuing HTTP request to the target API and merging streams.  It is separate from the web application project so that its logic could be distributed separately, such as in console apps, desktop apps, XBox apps... whatever.  Most of its implementing types are `internal` to the assembly, leaving only the business logic contract, `IStreamRepository`, and its return objects as `public` to consumers.

The StreamMerge.App.Tests project is responsible for unit testing components in StreamMerge.App.  As a separate project, it is not deployed to the web server when StreamMerge is published.  The project has one high-level smoke test: `StreamRepositoryTests`.  This test simply verifies that successive calls to `StreamRepository` invokes the internal `IStreamClient` and merges the results, returning them in increasing order.  It has a fake implementation of `IStreamClient` to avoid issuing real HTTP requests against the target API during the evaluation of the test.

The code is laid out using C# style and conventions.  It uses C# features like `async`/`await` to issue non-blocking out-of-process I/O.  Feel free to ask about any C# peculiar details.

## Usage
ASP.net 5 can target the newly open sourced [CoreCLR](http://blogs.msdn.com/b/dotnet/archive/2015/02/03/coreclr-is-now-open-source.aspx).  CoreCLR is multi-platform, capable of running on Windows, OS X, or Linux.

### Easier Way
The easier way to run this application is using the package hosted on GitHub:

1.  Install the DNVM (a version manager for DNX runtimes).
	*  Windows: Run this from a console window with administrator privileges: `@powershell -NoProfile -ExecutionPolicy unrestricted -Command "&{$Branch='dev';iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.ps1'))}"`.
	*  OS X: 
		1.  [Install Homebrew](http://www.brew.sh).
		2.  Run `brew tap aspnet/dnx`.
		3.  Run `brew update`.
		4.  Run `brew install dnvm`.
	*  Linux: Follow [this guide](https://github.com/aspnet/Home/blob/dev/GettingStartedDeb.md); good luck.
2.  Run `dnvm upgrade` to install a version of the DNX.
3.  Download [the packaged application](https://github.com/kfritz/StreamMerge/releases/download/v1/StreamMerge.zip).
4.  Extract the zip archive.
5.  Start the application.
	*  Windows: run `web.cmd`.
	*  Bash: run `web`.
6.  Issue GET requests to http://localhost:5000/quiz/merge with the appropriate query string parameters.

### Harder Way

To run the application from scratch, one must [clone this repository](http://git-scm.com/book/en/v2/Git-Basics-Getting-a-Git-Repository) and then [setup DNX](https://github.com/aspnet/Home/blob/dev/README.md).  Once DNX is installed (either on Windows, OS X, or Linux), one should go through the process of getting the required packages in position and starting the runtime.

1.  Install the DNVM (a version manager for DNX runtimes).
	*  Windows: Run this from a console window with administrator privileges: `@powershell -NoProfile -ExecutionPolicy unrestricted -Command "&{$Branch='dev';iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.ps1'))}"`.
	*  OS X: 
		1.  [Install Homebrew](http://www.brew.sh).
		2.  Run `brew tap aspnet/dnx`.
		3.  Run `brew update`.
		4.  Run `brew install dnvm`.
	*  Linux: Follow [this guide](https://github.com/aspnet/Home/blob/dev/GettingStartedDeb.md); good luck.
2.  Run `dnvm upgrade` to install a version of the DNX.
3.  Clone this repository.
4.  Change directories to the root of the cloned repository.
5.  Run `dnu restore` to restore the required packages.
6.  Change directories to .\src\StreamMerge.
7.  Start the application with the command `dnx . web` on Windows or `dnx . kestrel` on OS X/Linux.
8.  Issue GET requests to http://localhost:5000/quiz/merge with the appropriate query string parameters.

Note, I haven't actually tried this on OS X and Linux; your mileage may vary. 

### Tests

Running the unit test project is described [here](https://github.com/aspnet/Testing/wiki/How-to-create-test-projects).

## Future Work
No future work is actually planned for this project.  However, if there were to be updates, these tasks would be valuable:

* **Unit test coverage of corner cases** - The test as written now is simply a quick check that streams are merged and state is preserved between invocations.  Better test coverage should be added to cover corner cases.  For example, what happens when the application is invoked with overlapping stream names?  The quiz prompt was vague on this point, so the behavior right now is undefined.
* **Configurable target API** - The address of the Peloton API is presently hard coded into `StreamClient`.  This should instead be driven by configuration files so that one can retarget the application to a different location like a sandbox API instance.  This could be implemented using the [`IConfiguration` contract](http://whereslou.com/2014/05/23/asp-net-vnext-moving-parts-iconfiguration/).
* **More expressive input contract** - The quiz prompt only asked to merge two named streams.  The `IStreamRepository` contract, though, is capable of merging any positive number of streams.  Extending this capability to consumers of StreamMerge could be valuable.