using System;
using System.Text;
using System.Collections.Generic;
using System.IO; 
using System.Windows.Forms;
using System.Reflection;
 

namespace MailReSend
{
    public class KeyWords
    {
        public string FilePathDefault = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\KeyWords.txt";
        public List<KeyWord> List;
        public string msg = "";

        public KeyWords() : base()
        {
            List = new List<KeyWord>();
        }

        public KeyWords(string _path) : this()
        {
            this.FilePathDefault = _path;
            //List = new List<KeyWord>();
        }

        public void Add(KeyWord _keyWord)
        {
            if (!List.Exists(el => el.Word == _keyWord.Word))
            {
                List.Add(_keyWord); 
                msg = "Success: Добавлено Ключевое слово " + _keyWord.Word;
            }
            else
            {
                msg = "Warning: Уже существует Ключевое слово " + _keyWord.Word; 
            } 
        }

        public void Delete(string _Word)
        {
            try
            {
                List.Remove(List.Find(el => el.Word == _Word)); 
                msg = "Success: Удалено Ключевое слово " + _Word;
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
                List.Find(el => el.Word == _oldWord).Word = _newWord; 
                msg = "Success: Отредактировано Ключевое слово " + _oldWord + " -> " + _newWord;
            }
            catch (Exception error)
            {
                msg = "Error in  Edit:" + error.Message;
            }
        }

        public void ImportFile(string _pathFile)
        {
            try
            {
                // чтение из файла
                using (StreamReader file = new StreamReader(_pathFile))
                {
                    string line = "";
                    while ((line = file.ReadLine())!= null)
                    {
                        if (!String.IsNullOrWhiteSpace(line))
                        {
                            KeyWord keyWord = new KeyWord(line.Trim());
                            Add(keyWord); 
                        } 
                    } 
                    file.Close();
                } 
            }
            catch (Exception error)
            {
                msg = "Error in ImportFile: " + error.Message;
                throw (new Exception(msg)); 
            }
            Log.Write(msg);
        }

        public void ExportFile(string _path)
        {
            try
            {
                using (StreamWriter log = new StreamWriter(_path, false, Encoding.Unicode))
                {
                    foreach (KeyWord key in List)
                    {
                        log.WriteLine(key.Word);
                    }
                    log.Close();
                }
                msg = "Success: Выгружены Ключевые слова в файл " + _path;
            }
            catch (Exception error)
            {
                msg = "Error in ExportFile: " + error.Message;
                throw (new Exception(msg));
            } 
            Log.Write(msg);
           
        
        }


    }


}
