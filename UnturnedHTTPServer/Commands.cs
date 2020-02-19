using Rocket.API;
using Rocket.Unturned.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Plugin
{
    //public partial class WebServerHost : IRocketCommand
    //{
    //    public AllowedCaller AllowedCaller
    //    {
    //        get
    //        {
    //            return AllowedCaller.Both;
    //        }
    //    }

    //    public string Name
    //    {
    //        get
    //        {
    //            return "HTTPServer";
    //        }
    //    }

    //    public string Help
    //    {
    //        get
    //        {
    //            return "Starts the local HTTP Server";
    //        }
    //    }

    //    public string Syntax
    //    {
    //        get
    //        {
    //            return "Usage: /HTTPServer <start/stop/status> <port>";
    //        }
    //    }

    //    public List<string> Aliases
    //    {
    //        get
    //        {
    //            return new List<string>();
    //        }
    //    }

    //    public List<string> Permissions
    //    {
    //        get
    //        {
    //            return new List<string>() { "httpserver.start", "httpserver.stop", "httpserver.status" };
    //        }
    //    }

    //    public void Execute(IRocketPlayer caller, string[] command)
    //    {
    //        if (command.Count() != 0)
    //        {
    //            string Base = command[0];
    //            if ((Base.ToLower() ?? "") == "start")
    //            {
    //                if (Vals.MyConfig.AllowHTTPControls)
    //                {
    //                    if (caller.HasPermission("httpserver.start") | caller.HasPermission("httpserver.*"))
    //                    {
    //                        try
    //                        {
    //                            if (command.Count() == 2)
    //                            {
    //                                try
    //                                {
    //                                    int port = (Int32.Parse(command[1]));
    //                                    UnturnedChat.Say(caller, "Starting HTTP Server...", Color.blue);
    //                                    ModHTTPServer.StartServer(port);

    //                                    UnturnedChat.Say(caller, $"HTTP Server Running on port {port}", Color.green);
    //                                }
    //                                catch (Exception)
    //                                {
    //                                    UnturnedChat.Say(caller, $"Error: Invalid Port", Color.red);
    //                                }
    //                            }
    //                            else
    //                            {
    //                                UnturnedChat.Say(caller, "Starting HTTP Server...", Color.blue);
    //                                ModHTTPServer.StartServer();
    //                                UnturnedChat.Say(caller, $"HTTP Server Running on {ModHTTPServer.Listener.Prefixes(0)}", Color.green);
    //                            }
    //                        }
    //                        catch (Exception ex)
    //                        {
    //                            UnturnedChat.Say(caller, $"Failed to start Server: {ex.Message}", Color.red);
    //                            UnturnedChat.Say(caller, $"{ex.Source}", Color.red);
    //                            UnturnedChat.Say(caller, $"{ex.StackTrace}", Color.red);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        UnturnedChat.Say(caller, "Error: You do not have permission to this command.", Color.red);
    //                    }
    //                }
    //                else
    //                {
    //                    UnturnedChat.Say(caller, "Error: HTTP Server Controls have been disabled.", Color.red);
    //                }
    //            }
    //            else if ((Base.ToLower() ?? "") == "stop")
    //            {
    //                if (Vals.MyConfig.AllowHTTPControls)
    //                {
    //                    if (caller.HasPermission("httpserver.stop") | caller.HasPermission("httpserver.*"))
    //                    {
    //                        if (ModHTTPServer.Listener.IsListening)
    //                        {
    //                            UnturnedChat.Say(caller, $"Stopping HTTP Server on port {ModHTTPServer.Port}...", Color.green);
    //                            ModHTTPServer.StopServer();
    //                        }
    //                        else
    //                        {
    //                            UnturnedChat.Say(caller, $"Error: HTTP server not running.", Color.red);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        UnturnedChat.Say(caller, "Error: You do not have permission to this command.", Color.red);
    //                    }
    //                }
    //                else
    //                {
    //                    UnturnedChat.Say(caller, "Error: HTTP Server Controls have been disabled.", Color.red);
    //                }
    //            }
    //            else if ((Base.ToLower() ?? "") == "status")
    //            {
    //                if (caller.HasPermission("httpserver.status") | caller.HasPermission("httpserver.*"))
    //                {
    //                    var Msgl = new List<string>();
    //                    Msgl.Add($"Server Online: {ModHTTPServer.Listener.IsListening}");
    //                    if (ModHTTPServer.Port == 0)
    //                    {
    //                        if (!(ModHTTPServer.Address == null))
    //                        {
    //                            Msgl.Add($"Address: '{ModHTTPServer.Address}'");
    //                        }
    //                    }
    //                    else
    //                    {
    //                        Msgl.Add($"Port: {ModHTTPServer.Port}");
    //                    }

    //                    Msgl.Add($"Started At: {ModHTTPServer.StartedAt.ToShortDateString} {ModHTTPServer.StartedAt.ToLongTimeString}");
    //                    Msgl.Add($"Controls Enabled: {Vals.MyConfig.AllowHTTPControls}");
    //                    Msgl.Add($"Root File: '{Vals.MyConfig.RootServerFile}'");
    //                    string last = "";
    //                    foreach (var line in Msgl)
    //                    {
    //                        if (string.IsNullOrEmpty(last))
    //                        {
    //                            last = line;
    //                        }
    //                        else
    //                        {
    //                            UnturnedChat.Say(caller, last + Environment.NewLine + line, Color.green);
    //                            last = "";
    //                        }
    //                    }

    //                    if (string.IsNullOrEmpty(last))
    //                    {
    //                        UnturnedChat.Say(caller, last, Color.green);
    //                    }
    //                }
    //                else
    //                {
    //                    UnturnedChat.Say(caller, "Error: You do not have permission to this command.", Color.red);
    //                }
    //            }
    //            else
    //            {
    //                UnturnedChat.Say(caller, $"Error: Unknown Parameter '{Base}'", Color.red);
    //            }
    //        }
    //        else
    //        {
    //            UnturnedChat.Say(caller, Syntax, Color.green);
    //        }
    //    }
    //}
}