# UnturnedHTTPServer
A simple HTTP server that runs as a plugin, allowing you to run a website off your server!

With this plugin, you can run a simple HTTP server off of your Unturned Server. You can also manage/check the status of the http server in-game.

## Permission Nodes:
<table>
  <tr>
    <th>Permission Node</th>
    <th>Related Permission</th>
  </tr>
  <tr>
    <td>httpserver</td>
    <td>Base Permission Node to run /HttpServer</td>
  </tr>
  <tr>
    <td>httpserver.start</td>
    <td>Allows the user to Start the HTTP server</td>
  </tr>
  <tr>
    <td>httpserver.stop</td>
    <td>Allows the user to Stop the HTTP Server</td>
  </tr>
  <tr>
    <td>httpserver.status</td>
    <td>Allows the user to check the status of the HTTP Server</td>
  </tr>
</table>

## Commands:
/HTTPServer is the command you use to controll the HTTP Server in-game. With it, you can Start, Stop, and Check the status of the HTTP Server.

### Starting the Server: (Using settings in the config)

/HTTPServer Start

### Starting the Server: (Using the specified port)

/HTTPServer Start (Port)

### Stopping the Server:

/HTTPServer Stop

### Checking the Server's Status:

/HTTPServer Status

## Config

<table>
  <tr>
    <th>Settings Key</th>
    <th>Relevant Setting</th>
  </tr>
  <tr>
    <td>HTTPServerAddress</td>
    <td>The Prefix for the HTTP Server (See below)</td>
  </tr>
  <tr>
    <td>AutostartHTTPServer</td>
    <td>Specifies if the HTTP Server should start when the Unturned Server Starts</td>
  </tr>
  <tr>
    <td>AllowHTTPControls</td>
    <td>Specifies if the HTTP Server Start/Stop Commands are enabled.</td>
  </tr>
  <tr>
    <td>VerboseOutput</td>
    <td>Enables/Disables Verbose Output (Verbose output can get spamy on large servers)</td>
  </tr>
  <tr>
    <td>HTTPServerDirectory</td>
    <td>The Directory (folder) that contains the files/folders to host, Defaults to the 'Server" folder inside of the Plugin's config folder</td>
  </tr>
  <tr>
    <td>RootServerFile</td>
    <td>Specifies the root file (e.g, the file you get when going to www.domain.com), set it do %auto for auto-detect</td>
  </tr>
</table>

### Server Prefix:
The server prefix specifies how the server runs. It must end in a forward slash, specify the method (http or https), the scope, and the port.

#### Method
Most people will just use https. But you can use https, but you need an SSL certificate to be installed on the host computer.

#### Scope
The 3 most common scopes are Local, Specific, and All.

<table>
  <tr>
    <th>Scope</th>
    <th>Key</th>
    <th>Info</th>
    <th>Example</th>
  </tr>
  <tr>
    <td>All</td>
    <td>*</td>
    <td>Allows the server to be connected to from any domain</td>
    <td>http://*:80/</td>
  </tr>
  <tr>
    <td>Spcific</td>
    <td>(Server IP or Domain)</td>
    <td>Allows the server to be only connected to via the key</td>
    <td>http://domain.com:80/</td>
  </tr>
  <tr>
    <td>Local</td>
    <td>Localhost</td>
    <td>Only allows the server to be connected to via localhost (The host pc)</td>
    <td>http://localhost:80/</td>
  </tr>
</table>

Most People will just want to use the default, All (*).


## Setup:

After seting up the plugin on your server, there are 2 more things you need to do.

### Port Forward
Port forward the TCP port the HTTP server is running off

### Open the port
See <a href="https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/configuring-http-and-https">this Article</a> for info on how to do this on Windows.




