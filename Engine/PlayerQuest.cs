using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class PlayerQuest
    {
        public Quest QuestDetails { get; set; }
        public bool IsCompleted { get; set; }

        public PlayerQuest(Quest questDetails, bool isCompleted = false)
        {
            QuestDetails = questDetails;
            IsCompleted = isCompleted;
        }
    }
}
