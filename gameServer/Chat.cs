using ExitGames.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gameServer
{
    public class Chat
    {
        public static readonly Chat Instance = new Chat();
        // Потокобезопасность ограничивает запись в разделяемые ресурсы
        private readonly ReaderWriterLockSlim readWriteLock;
        // Список истории чата
        private List<string> Messages { get; set; }
        private const int MAX_RECENT_MESSAGES = 10;
        private const int MAX_BUFFERED_MESSAGES = 100;

        public Chat() 
        {
            Messages = new List<string>();
            readWriteLock = new ReaderWriterLockSlim();
        }

        ~Chat()
        {
            readWriteLock.Dispose();
        }

        public List<string> GetRecentMessages() 
        {
            using (ReadLock.TryEnter(this.readWriteLock, 1000))
            {
                var list = new List<string>(MAX_RECENT_MESSAGES);
                if (Messages.Count == 0)
                {
                    return list;
                }

                if (Messages.Count <= MAX_RECENT_MESSAGES)
                {
                    list.AddRange(Messages);
                    return list;
                }

                return Messages.Skip(Messages.Count - MAX_RECENT_MESSAGES).ToList();
            }
        }

        public void AddMessage(string message)
        {
            using (WriteLock.TryEnter(this.readWriteLock, 1000))
            {
                if (Messages.Count == MAX_BUFFERED_MESSAGES)
                {
                    Messages = Messages.Skip(1).ToList();
                }

                Messages.Add(message);

            }
        }

    }
}
