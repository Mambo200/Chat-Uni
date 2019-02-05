using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_Chat
{
    class User
    {
        public int ID { get; private set; }
        public string Nickname { get; private set; }


        public User(int _id)
        {
            ID = _id;
            Nickname = "";
        }

        public User(int _id, string _nickname)
        {
            ID = _id;
            Nickname = _nickname;
        }

        public bool ChangeNickname(string _nickname)
        {
            if(string.IsNullOrWhiteSpace(_nickname))
            {
                return false;
            }

            Nickname = _nickname.Trim();
            return true;
        }
    }
}
