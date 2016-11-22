using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ExileFinder.Helpers
{
    public class Verifier
    {
        public bool IsVerified;
        public bool Cancel;
        internal string Path;
        public BackgroundWorker BackgroundWorker = new BackgroundWorker();
        
        public Verifier(string pathToLog)
        {
            IsVerified = false; //Change to False for live
            Cancel = false;
            Path = pathToLog + "\\Client.txt";
            BackgroundWorker.DoWork += backgroundWorker_DoWork;
            BackgroundWorker.WorkerSupportsCancellation = true;
        }

        public void VerifyCharacter(string Name)
        {
            if (Name != null) BackgroundWorker.RunWorkerAsync(Name);
        }

        public void CancelVerify()
        {
            BackgroundWorker.CancelAsync();
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string characterName = e.Argument.ToString().ToLower();

            while (IsVerified == false && Cancel == false)
            {
                string line = "";
                string pattern = "[@$#&]([\\w ]+): !(?:pvp|PVP) ([\\w]+)";
                Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
                FileStream fileStream = File.Open(Path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                BufferedStream bufferedStream = new BufferedStream(fileStream);
                StreamReader streamReader = new StreamReader(bufferedStream);
                while (!streamReader.EndOfStream)
                {
                    line = streamReader.ReadLine();
                }

                ChatMessage chatMessage = new ChatMessage(line);

                if (chatMessage.Player.Contains(characterName))
                {
                    if (chatMessage.Message.ToLower() == "verify")
                    {
                        IsVerified = true;
                    }
                }
                Thread.Sleep(250);
            }

            Cancel = false;
        }
    }
    public class ChatMessage
    {
        public string Timestamp { get; set; }
        public MessageType Chat { get; set; }
        public string Player { get; set; }
        public string Message { get; set; }
        public string Guild { get; set; }

        private string pattern = "^([0-9/]+ [0-9:]+).*] (<.*>)*(.*): (.*)";

        public enum MessageType
        {
            Trade, Global, Wisper, Party, Guild, Local, Debug, Info 
        }

        public ChatMessage(string message)
        {
            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = reg.Match(message);
            if (message.Contains("DEBUG Client"))
            {
                this.Chat = MessageType.Debug;
            }
            else if (message.Contains("INFO Client"))
            {
                this.Chat = MessageType.Info;
            }
            else
            {
                switch (match.Groups[2].ToString().ToLower())
                {
                    case "#":
                        this.Chat = MessageType.Global;
                        break;
                    case "%":
                        this.Chat = MessageType.Party;
                        break;
                    case "@":
                        this.Chat = MessageType.Wisper;
                        break;
                    case "&":
                        this.Chat = MessageType.Guild;
                        break;
                    case "$":
                        this.Chat = MessageType.Trade;
                        break;
                    default:
                        this.Chat = MessageType.Local;
                        break;
                }
            }
            
            this.Timestamp = match.Groups[1].ToString().ToLower();
            this.Guild = match.Groups[2].ToString().ToLower();
            this.Player = match.Groups[3].ToString().ToLower();
            this.Message = match.Groups[4].ToString().ToLower();
        }
    }
}
