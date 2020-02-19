using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using Rocket.API;
using Rocket.Unturned.Chat;
using UnityEngine;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;

namespace Plugin
{
    public static partial class Vals
    {
        public static Config MyConfig;
    }

    public partial class Main : RocketPlugin<Config>
    {
        protected override void Load()
        {
            Vals.MyConfig = Configuration.Instance;
            if (!System.IO.Directory.Exists(@"Plugins\HTTPServer\Server"))
            {
                System.IO.Directory.CreateDirectory(@"Plugins\HTTPServer\Server");
            }

            if (Configuration.Instance.AutostartHTTPServer)
            {
                Server.StartServer(Configuration.Instance.HTTPServerAddress);
            }

            base.Load();
        }

        protected override void Unload()
        {
            base.Unload();
        }
    }

    public partial class Config : IRocketPluginConfiguration
    {
        public string HTTPServerAddress;
        public bool AutostartHTTPServer;
        public bool AllowHTTPControls;
        public bool VerboseOutput;
        public string HTTPServerDirectory;
        public string RootServerFile;

        public void LoadDefaults()
        {
            HTTPServerAddress = "http://*:8000/";
            AutostartHTTPServer = false;
            AllowHTTPControls = true;
            VerboseOutput = true;
            HTTPServerDirectory = @"Plugins\HTTPServer\Server";
            RootServerFile = "%auto";
        }
    }

    internal partial class Server
    {
        public static HttpListener Listener  = new HttpListener();
        public static bool ServerActive = false;
        public static string Version = "1.0";
        public static string Address;
        public static int Port = 0;
        public static DateTime StartedAt = DateTime.Now;

        public static void cw(string msg)
        {
            if (Plugin.Vals.MyConfig.VerboseOutput)
            {
                Console.WriteLine($"[HTTPServer] {msg}");
            }
        }

        public static void StartServer(int Port = -1)
        {
            if (Listener.IsListening)
            {
                Listener.Stop();
                Listener = new HttpListener();
            }

            Listener = new HttpListener();
            cw($"Shimmy's HTTP File Server [v{Version}]");
            if (Port == -1)
            {
                Server.Port = 0;
                Address = Plugin.Vals.MyConfig.HTTPServerAddress;
                Listener.Prefixes.Add(Plugin.Vals.MyConfig.HTTPServerAddress);
            }
            else
            {
                Server.Port = Port;
                Address = null;
                Listener.Prefixes.Add($"http://*:{Port}/");
            }

            try
            {
                if (!ServerActive)
                {
                    Listener.Start();
                    var ST = new Thread(ServerRequestHandler);
                    ST.Start();
                    StartedAt = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                cw($"Failed to start server, error: {ex.Message}");
            }

            cw("Server online");
        }

        public static void StartServer(string Address)
        {
            if (Listener.IsListening)
            {
                Listener.Stop();
                Listener = new HttpListener();
            }

            Listener = new HttpListener();
            Listener.Prefixes.Add(Plugin.Vals.MyConfig.HTTPServerAddress);
            cw($"Shimmy's HTTP File Server [v{Version}]");
            Port = 0;
            Server.Address = Address;
            try
            {
                if (!ServerActive)
                {
                    Listener.Start();
                    var ST = new Thread(ServerRequestHandler);
                    ST.Start();
                    StartedAt = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                cw($"Failed to start server, error: {ex.Message}");
            }

            cw("Server online");
        }

        public static void StopServer()
        {
            cw("Stopping server...");
            Listener.Stop();
            cw("Server offline.");
        }

        public static string GetRootFile()
        {
            if (Plugin.Vals.MyConfig.RootServerFile.ToLower() == "%auto")
            {
                string selname = null;
                foreach (string file in System.IO.Directory.GetFiles(Plugin.Vals.MyConfig.HTTPServerDirectory))
                {
                    var ioinf = new FileInfo(file);
                    if (ioinf.Name.Contains("."))
                    {
                        List<string> parts = ioinf.Name.Split(".".ToCharArray()[0]).ToList();
                        parts.RemoveAt(parts.Count - 1);
                        string recname = string.Join(".", parts);
                        if (recname.EndsWith("_WMH-siteindex"))
                        {
                            selname = file;
                        }
                    }
                }

                if (selname == null)
                {
                    foreach (string file in System.IO.Directory.GetFiles(Plugin.Vals.MyConfig.HTTPServerDirectory))
                    {
                        var ioinf = new FileInfo(file);
                        if (ioinf.Name.ToLower().Contains("index") | ioinf.Name.ToLower().Contains("root"))
                        {
                            selname = file;
                        }
                    }
                }

                return selname;
            }
            else
            {
                return Plugin.Vals.MyConfig.HTTPServerDirectory + Plugin.Vals.MyConfig.RootServerFile;
            }
        }

        public static void HandleRequest(object contexts)
        {
            HttpListenerContext context = (HttpListenerContext)contexts;
            cw($"Request Recieved -> {context.Request.RawUrl}");
            if (context.Request.RawUrl == "/")
            {
                string root = GetRootFile();
                if (!(root == null))
                {
                    using (var memstream = new MemoryStream(File.ReadAllBytes(root)))
                    {
                        var info = new FileInfo(root);
                        MIMEType Mime = (MIMEType)MIMETypeFromFileType(info.Extension.TrimStart(".".ToCharArray()[0]));
                        {
                            var withBlock = context.Response;
                            withBlock.ContentType = Mime.MIMEValue;
                            withBlock.ContentLength64 = memstream.Length;
                            memstream.CopyTo(withBlock.OutputStream);
                            withBlock.Close();
                        }
                    }
                }
                else
                {
                    cw($"Requested file {context.Request.RawUrl} not found.");
                    context.Response.StatusCode = 404;
                    string ResHTML = $@"<html>
<head>
  <title>404 Not Found</title>
</head>
<body>
  <h1>Requested File Not Found</h1><hr>
  <p>The requested URL {context.Request.RawUrl} was not found on the server.</p>
  <address>Shimmy's Unturned HTTP Server (Rocketmod) v{Version}</address>
</body>
</html>";
                    var ResBytes = Encoding.UTF8.GetBytes(ResHTML);
                    try
                    {
                        {
                            var withBlock1 = context.Response;
                            withBlock1.ContentType = "text/html";
                            withBlock1.ContentLength64 = ResBytes.Length;
                            withBlock1.OutputStream.Write(ResBytes, 0, ResBytes.Count());
                            withBlock1.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            else
            {
                string urlprt = context.Request.RawUrl.TrimStart("/".ToCharArray()[0]).Replace("/", @"\");
                urlprt = Plugin.Vals.MyConfig.HTTPServerDirectory + urlprt.Split("?".ToCharArray()[0]).First();
                if (File.Exists(urlprt))
                {
                    try
                    {
                        using (var memstream = new MemoryStream(File.ReadAllBytes(urlprt)))
                        {
                            var info = new FileInfo(urlprt);
                            MIMEType Mime = (MIMEType)MIMETypeFromFileType(info.Extension.TrimStart(".".ToCharArray()[0]));
                            {
                                var withBlock2 = context.Response;
                                withBlock2.ContentType = Mime.MIMEValue;
                                withBlock2.ContentLength64 = memstream.Length;
                                memstream.CopyTo(withBlock2.OutputStream);
                                withBlock2.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    cw($"Requested file {context.Request.RawUrl} not found.");
                    context.Response.StatusCode = 404;
                    string ResHTML = $@"<html>
<head>
  <title>404 Not Found</title>
</head>
<body>
  <h1>Requested File Not Found</h1><hr>
  <p>The requested URL {context.Request.RawUrl} was not found on the server.</p>
  <address>Shimmy's Unturned HTTP Server (Rocketmod) v{Version}</address>
</body>
</html>";
                    var ResBytes = Encoding.UTF8.GetBytes(ResHTML);
                    {
                        var withBlock3 = context.Response;
                        withBlock3.ContentType = "text/html";
                        withBlock3.ContentLength64 = ResBytes.Length;
                        withBlock3.OutputStream.Write(ResBytes, 0, ResBytes.Count());
                        withBlock3.Close();
                    }
                }
            }
        }

        private static int Errors = 0;

        private static bool firstr = true;

 

        public static void ServerRequestHandler()
        {
            Errors = 0;
            ServerActive = true;
            while (ServerActive)
            {
                try
                {
                    HttpListenerContext context = Listener.GetContext();
                    Thread t = new Thread(new ParameterizedThreadStart(HandleRequest));
                    t.Start(context);
                    Errors = 0;
                    firstr = false;
                }
                catch (Exception ex)
                {
                    if (firstr)
                    {
                        Errors = Errors + 1;
                        if (Errors >= 10)
                        {
                            cw("Too many Errors, HTTP server crashed.");
                            StopServer();
                            ServerActive = false;
                        }
                    }

                    Console.WriteLine($"Error handling request: {ex.Message}");
                }
            }

            ServerActive = false;
        }

        public static object MIMETypeFromFileType(string ext)
        {
            ext = ext.TrimStart('.');
            var switchExpr = ext.ToLower();
            switch (switchExpr)
            {
                case "html":
                    {
                        return MIMETypes.HTML;
                    }

                case "htm":
                    {
                        return MIMETypes.HTML;
                    }

                case "pdf":
                    {
                        return MIMETypes.PDF;
                    }

                case "mpeg":
                    {
                        return MIMETypes.Mpeg;
                    }

                case "vorbis":
                    {
                        return MIMETypes.Vorbis;
                    }

                case "woff":
                    {
                        return MIMETypes.FontWoff;
                    }

                case "tff":
                    {
                        return MIMETypes.FontTTF;
                    }

                case "otf":
                    {
                        return MIMETypes.FontOTF;
                    }

                case "jpeg":
                    {
                        return MIMETypes.Jpeg;
                    }

                case var @case when @case == "jpeg":
                    {
                        return MIMETypes.Jpeg;
                    }

                case "png":
                    {
                        return MIMETypes.PNG;
                    }

                case "svg":
                    {
                        return MIMETypes.SVG;
                    }

                case "txt":
                    {
                        return MIMETypes.Plaintext;
                    }

                case "csv":
                    {
                        return MIMETypes.CSV;
                    }

                case "css":
                    {
                        return MIMETypes.CSS;
                    }

                case "js":
                    {
                        return MIMETypes.JavaScript;
                    }

                case "wav":
                    {
                        return MIMETypes.Wav;
                    }

                case "webm":
                    {
                        return MIMETypes.WebmVideo;
                    }

                case "swf":
                    {
                        return MIMETypes.Flash;
                    }

                case "mp4":
                    {
                        return MIMETypes.Mp4Video;
                    }

                case "json":
                    {
                        return MIMETypes.JSON;
                    }

                default:
                    {
                        return MIMETypes.Binary;
                    }
            }
        }

        public partial class WebServerHost : IRocketCommand
        {
            public AllowedCaller AllowedCaller
            {
                get
                {
                    return AllowedCaller.Both;
                }
            }

            public string Name
            {
                get
                {
                    return "HTTPServer";
                }
            }

            public string Help
            {
                get
                {
                    return "Starts the local HTTP Server";
                }
            }

            public string Syntax
            {
                get
                {
                    return "Usage: /HTTPServer <start/stop/status> <port>";
                }
            }

            public List<string> Aliases
            {
                get
                {
                    return new List<string>();
                }
            }

            public List<string> Permissions
            {
                get
                {
                    return new List<string>() { "httpserver.start", "httpserver.stop", "httpserver.status" };
                }
            }

            public void Execute(IRocketPlayer caller, string[] command)
            {
                if (command.Count() != 0)
                {
                    string Base = command[0];
                    if ((Base.ToLower() ?? "") == "start")
                    {
                        if (Vals.MyConfig.AllowHTTPControls)
                        {
                            if (caller.HasPermission("httpserver.start") | caller.HasPermission("httpserver.*"))
                            {
                                try
                                {
                                    if (command.Count() == 2)
                                    {
                                        try
                                        {
                                            int port = (Int32.Parse(command[1]));
                                            UnturnedChat.Say(caller, "Starting HTTP Server...", Color.blue);
                                            StartServer(port);

                                            UnturnedChat.Say(caller, $"HTTP Server Running on port {port}", Color.green);
                                        }
                                        catch (Exception)
                                        {
                                            UnturnedChat.Say(caller, $"Error: Invalid Port", Color.red);
                                        }
                                    }
                                    else
                                    {
                                        UnturnedChat.Say(caller, "Starting HTTP Server...", Color.blue);
                                        StartServer();
                                        UnturnedChat.Say(caller, $"HTTP Server Running on {Listener.Prefixes.First()}", Color.green);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    UnturnedChat.Say(caller, $"Failed to start Server: {ex.Message}", Color.red);
                                    UnturnedChat.Say(caller, $"{ex.Source}", Color.red);
                                    UnturnedChat.Say(caller, $"{ex.StackTrace}", Color.red);
                                }
                            }
                            else
                            {
                                UnturnedChat.Say(caller, "Error: You do not have permission to this command.", Color.red);
                            }
                        }
                        else
                        {
                            UnturnedChat.Say(caller, "Error: HTTP Server Controls have been disabled.", Color.red);
                        }
                    }
                    else if ((Base.ToLower() ?? "") == "stop")
                    {
                        if (Vals.MyConfig.AllowHTTPControls)
                        {
                            if (caller.HasPermission("httpserver.stop") | caller.HasPermission("httpserver.*"))
                            {
                                if (Listener.IsListening)
                                {
                                    UnturnedChat.Say(caller, $"Stopping HTTP Server on port {Port}...", Color.green);
                                    StopServer();
                                }
                                else
                                {
                                    UnturnedChat.Say(caller, $"Error: HTTP server not running.", Color.red);
                                }
                            }
                            else
                            {
                                UnturnedChat.Say(caller, "Error: You do not have permission to this command.", Color.red);
                            }
                        }
                        else
                        {
                            UnturnedChat.Say(caller, "Error: HTTP Server Controls have been disabled.", Color.red);
                        }
                    }
                    else if ((Base.ToLower() ?? "") == "status")
                    {
                        if (caller.HasPermission("httpserver.status") | caller.HasPermission("httpserver.*"))
                        {
                            var Msgl = new List<string>();
                            Msgl.Add($"Server Online: {Listener.IsListening}");
                            if (Port == 0)
                            {
                                if (!(Address == null))
                                {
                                    Msgl.Add($"Address: '{Address}'");
                                }
                            }
                            else
                            {
                                Msgl.Add($"Port: {Port}");
                            }

                            Msgl.Add($"Started At: {StartedAt.ToShortDateString()} {StartedAt.ToLongTimeString()}");
                            Msgl.Add($"Controls Enabled: {Vals.MyConfig.AllowHTTPControls}");
                            Msgl.Add($"Root File: '{Vals.MyConfig.RootServerFile}'");
                            string last = "";
                            foreach (var line in Msgl)
                            {
                                if (string.IsNullOrEmpty(last))
                                {
                                    last = line;
                                }
                                else
                                {
                                    UnturnedChat.Say(caller, last + System.Environment.NewLine + line, Color.green);
                                    last = "";
                                }
                            }

                            if (string.IsNullOrEmpty(last))
                            {
                                UnturnedChat.Say(caller, last, Color.green);
                            }
                        }
                        else
                        {
                            UnturnedChat.Say(caller, "Error: You do not have permission to this command.", Color.red);
                        }
                    }
                    else
                    {
                        UnturnedChat.Say(caller, $"Error: Unknown Parameter '{Base}'", Color.red);
                    }
                }
                else
                {
                    UnturnedChat.Say(caller, Syntax, Color.green);
                }
            }
        }


    }

    public partial class MIMETypes
    {
        public static MIMEType Binary = new MIMEType("application/octet-stream");
        public static MIMEType PDF = new MIMEType("application/pdf");
        public static MIMEType ZIP = new MIMEType("application/zip");
        public static MIMEType JSON = new MIMEType("application/json");
        public static MIMEType Mpeg = new MIMEType("audio/mpeg");
        public static MIMEType Vorbis = new MIMEType("audio/vorbis");
        public static MIMEType FontWoff = new MIMEType("font/woff");
        public static MIMEType FontTTF = new MIMEType("font/ttf");
        public static MIMEType FontOTF = new MIMEType("font/otf");
        public static MIMEType Jpeg = new MIMEType("image/jpeg");
        public static MIMEType PNG = new MIMEType("image/png");
        public static MIMEType SVG = new MIMEType("image/svg+xml");
        public static MIMEType Plaintext = new MIMEType("text/plain");
        public static MIMEType CSV = new MIMEType("text/csv");
        public static MIMEType HTML = new MIMEType("text/html");
        public static MIMEType CSS = new MIMEType("text/css");
        public static MIMEType JavaScript = new MIMEType("text/javascript");
        public static MIMEType Wav = new MIMEType("audio/wave");
        public static MIMEType WebmVideo = new MIMEType("video/webm");
        public static MIMEType WebmAudio = new MIMEType("audio/webm");
        public static MIMEType Flash = new MIMEType("application/x-shockwave-flash");
        public static MIMEType Mp4Video = new MIMEType("video/mp4");
    }

    public partial class MIMEType
    {
        public string MIMEValue;
        public string Base;
        public string Opend;
        public MIMEType(string val)
        {
            MIMEValue = val;
        }
    }
}