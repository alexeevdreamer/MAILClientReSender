using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailReSend
{
    public class FromUsers
    { 
        public List<string> List; 
        public string msg = "";
        public View.ListViewGeniral ListViewFrom;

        public FromUsers() : base()
        {
            List = new List<string>();
        }

        public void UpdateListView()
        {
            ListViewFrom.UpdateListView(List);
        }

        public void AddList(List<string> _listValue)
        {
            try
            {
                foreach (string value in _listValue)
                {
                    if (!List.Exists(el => el == value))
                    {
                        List.Add(value); 
                    }
                } 
                msg = "Success: Добавлен Список новых отправителей";
            }
            catch (Exception error)
            {
                msg = "Error: " + error.Message;
            }
        }

        public void Add(string _value)
        {
            if (!List.Exists(el => el == _value))
            {
                List.Add(_value);
                msg = "Success: Добавлен Новый отправитель From: " + _value;
            }
            else
            {
                msg = "Warning: Уже существует Ключевое слово " + _value;
            }
        }

        public void Delete(string _value)
        {
            try
            {
                List.Remove(List.Find(el => el == _value));
                msg = "Success: Удалено Ключевое слово " + _value;
            }
            catch (Exception error)
            {
                msg = "Error in  Delete:" + error.Message;
            }
        }

        public void Edit(string _oldWord, string _newWord)
        {
            try
            {
                //List.Find(_oldWord); 
                List[List.FindLastIndex(el => el == _oldWord)] = _newWord;
                //List.Remove(List.Find(el => el == _oldWord));
                //List.Add(_newWord);
                msg = "Success: Отредактировано Ключевое слово " + _oldWord + " -> " + _newWord;
            }
            catch (Exception error)
            {
                msg = "Error in  Edit:" + error.Message;
            }
        } 
     
    }
}
