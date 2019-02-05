using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_Chat
{
    public enum ECommands
    {
        NONE = 0,
        SERVER_ONLY_COMMANDS = 1,
        SET_ID,
        IS_VALID,
        USER_JOINED,

        CLIENT_SERVER_COMMANDS = 5000,
        CHANGE_NICKNAME,

        CLIENT_ONLY_COMMANDS = 10000,
        SET_NICKNAME,
    }
}
