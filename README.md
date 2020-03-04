# UnturnedHTTPServer
A simple HTTP server that runs as a plugin, allowing you to run a website off your server!

With this plugin, you can run a simple HTTP server off your Unturned Server. You can also manage/check the status of the http server in-game.


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
/HTTPServer is the command you use to control the HTTP Server in-game. With it, you can Start, Stop, and Check the status of the HTTP Server.

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
    <td>Specifies the root file (e.g., the file you get when going to www.domain.com), set it do %auto for auto-detect</td>
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
    <td>Specific</td>
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

## Setup; Basic

This HTTP server can run off the main port of your Unturned Server. Since the ports for your server (Your base port, though to your base port + 2) are commonly TCP/UDP, this plugin can utilise the TCP connection for your server to run a http server. This also means you can use this plugin even if you’re not using a dedicated server.

Note: I have not Thoroughly tested this for issues, but basic tests have shown no issue with running the http server on the same port as your base server itself. (I believe the base port uses UDP to query the server, so TCP is left open for the HTTP server)


## Setup; Alternate Port.

For hosting the server on an alternate port, you will need to open that port and port-forward it.

### Port Forward
Port forward the TCP port the HTTP server is running off

### Open the port
See <a href="https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/configuring-http-and-https">this Article</a> for info on how to do this on Windows.



