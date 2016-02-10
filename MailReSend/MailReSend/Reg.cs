using System;
using Microsoft.Win32;
using System.Collections.Generic;

namespace MailReSend
{
    public class Reg
    {
        string SubKeyName;
        string ValueName;
        RegistryKey reg = Registry.CurrentUser;
        RegistryKey hkCurUser;
        private string msg;

        public Reg(string _subKeyName, string _valueName)
        {
            this.SubKeyName = _subKeyName;
            this.ValueName = _valueName; 
            try
            {
                hkCurUser = reg.CreateSubKey(SubKeyName);
                msg = "Suucess: Создан в реестре параметр " + SubKeyName + " = " + _valueName;
            }
            catch (Exception error)
            {
                msg = "Error in Reg: " + error.Message; 
            }
            Log.Write(msg);
        }

        public void CreateSubKey()
        {
            try
            {
                hkCurUser = reg.CreateSubKey(SubKeyName);
                msg = "Suucess: Создан в реестре параметр " + SubKeyName;
            } 
            catch (Exception error)
            {
                msg = "Error in CreateSubKey: " + error.Message; 
            }
            Log.Write(msg);
        }

        private string ListIntoString(List<string> _valueList)
        {
            string result = "";
             
            foreach (string el in _valueList)
            {
                if (result == "")
                {
                    result = el;
                }
                else
                {
                    result += "," + el;
                }
            }
            return result;
        }

        public void SetValueStringList(List<string> _value)
        {
            try
            {
                string addValue = ListIntoString(_value);

                hkCurUser.SetValue(ValueName, addValue);
                msg = "Success: Записано в реестр " + ValueName + " = " + addValue;
            }
            catch (Exception error)
            {
                msg = "Error in SetVlue: " + error.Message;
            }
            Log.Write(msg);
        }

        public List<string> GetValueStringList(List<string> _defaultValue)
        {
            List <string> value =new List<string>();
            try
            {  
                string valuePreArray = hkCurUser.GetValue(ValueName).ToString();
                if (valuePreArray != "")
                {
                    string[] strArray = valuePreArray.Split(',');
                    value.AddRange(strArray);
                    msg = "Suucess: " + ValueName + " = " + valuePreArray.ToString();
                }
                //else
                //{
                //    throw new Exception("Warning: Пустое значение в " +ValueName);
                //} 
            }
            catch (Exception error)
            {
                 
                msg = "Error in GetValueString: " + error.Message;
                value = _defaultValue;
            }
            Log.Write(msg);
            return value; 
        }

        public List<string> GetValueStringList()
        {
            List<string> value = new List<string>();
            try
            {
                string valuePreArray = hkCurUser.GetValue(ValueName).ToString();
                string[] strArray = valuePreArray.Split(',');
                value.AddRange(strArray);
                msg = "Suucess: " + ValueName + " = " + valuePreArray.ToString();
            }
            catch (Exception error)
            {

                msg = "Error in GetValueString: " + error.Message; 
            }
            Log.Write(msg);
            return value; 
        }

        public object GetValueVar()
        {
            object value = null;
            try
            {
                value = hkCurUser.GetValue(ValueName);
                msg = "Suucess: " + ValueName + " = " + value.ToString();
            }
            catch (Exception error)
            {
                msg = "Error in GetValueVar: " + error.Message; 
            }

            Log.Write(msg);
            return value;
        }

        public string GetValueString()
        {
            string value = "";
            try
            {  
                value = hkCurUser.GetValue(ValueName).ToString();
                msg = "Suucess: " + ValueName + " = " + value.ToString(); 
            }
            catch (Exception error)
            {
                msg = "Error in GetValueString: " + error.Message;
                 
            }
            Log.Write(msg);
            return value;
        }
         
        public void SetValue(object _value)
        {
            try
            {
                hkCurUser.SetValue(ValueName, _value);
                msg = "Success: Записано в реестр " + ValueName + " = " + _value; 
            }
            catch (Exception error)
            {
                msg = "Error in SetVlue: " + error.Message; 
            }
            Log.Write(msg);
        }

        public DateTime GetValueDateTime()
        {
            DateTime value = DateTime.Today;
            try
            { 
                value = Convert.ToDateTime(hkCurUser.GetValue(ValueName));
                msg = "Suucess: " + ValueName + " = " + value.ToString();
            }
            catch (Exception error)
            {
                msg = "Error in GetValueDateTime: " + error.Message; 
            }
            Log.Write(msg);
            return value;
        }

        public Int32 GetValueInt32()
        {
            Int32 value = -1;
            try
            {
                value = Convert.ToInt32(hkCurUser.GetValue(ValueName));
                msg = "Suucess: " + ValueName + " = " + value.ToString();
            }
            catch (Exception error)
            {
                msg = "Error in GetValueInt32: " + error.Message;
               
            }
            Log.Write(msg);
            return value;
        }

        public bool GetValueBool()
        {
            bool value = false;
            try
            {
                value = Convert.ToBoolean(hkCurUser.GetValue(ValueName,false));
                msg = "Suucess: " + ValueName + " = " + value.ToString();
            }
            catch (Exception error)
            {
                msg = "Error in GetValueBool: " + error.Message;
                
            }
            Log.Write(msg);
            return value;
        }
         
        public DateTime GetValueDateTime(DateTime _defaultValue)
        {
            DateTime value = DateTime.Today;
            try
            {
                value = Convert.ToDateTime(hkCurUser.GetValue(ValueName));
                if (value == DateTime.MinValue || value == DateTime.MaxValue)
                {
                     throw  (new Exception());
                } 
                msg = "Suucess: " + ValueName + " = " + value.ToString();
                
                   
                 

            }
            catch (Exception error)
            {
                msg = "Warning: Взято дефолтное значение из реестра " + ValueName + " = " + _defaultValue.ToString() + " - " + error.Message;
                value = _defaultValue;
            }
            Log.Write(msg);
            return value;
        }
         
        public Int32 GetValueInt32(Int32 _defaultValue)
        {
            Int32 value = -1;
            try
            {
                value = Convert.ToInt32(hkCurUser.GetValue(ValueName, _defaultValue));
                msg = "Suucess: " + ValueName + " = " + value.ToString();
            }
            catch (Exception error)
            {
                msg = "Warning: Взято дефолтное значение из реестра " + ValueName + " = " + _defaultValue.ToString() + " - " + error.Message;
                value = _defaultValue;
            }
            Log.Write(msg);
            return value;
        }

        public bool GetValueBool(bool _defaultValue)
        {
            bool value = false;
            try
            {
                value = Convert.ToBoolean(hkCurUser.GetValue(ValueName, _defaultValue));
                msg = "Suucess: " + ValueName + " = " + value.ToString();
            }
            catch (Exception error)
            {
                msg = "Warning: Взято дефолтное значение из реестра " + ValueName + " = " + _defaultValue.ToString() + " - " + error.Message;
                value = _defaultValue;
            }
            Log.Write(msg);
            return value;
        }
         
        public object GetValueVar(object _defaultValue)
        {
            object value = null;
            try
            {
                value = hkCurUser.GetValue(ValueName, _defaultValue);
                msg = "Suucess: " + ValueName + " = " + value.ToString();
            }
            catch (Exception error)
            {
                msg = "Warning: Взято дефолтное значение из реестра " + ValueName +" = " + _defaultValue.ToString() + " - " + error.Message;
                value = _defaultValue;
            }

            Log.Write(msg);
            return value;
        }

        public string GetValueString(string _defaultValue)
        {
            string value = "";
            try
            {
                value = hkCurUser.GetValue(ValueName, _defaultValue).ToString();
                msg = "Suucess: " + ValueName + " = " + value.ToString();
            }
            catch (Exception error)
            {
                msg = "Warning: Взято дефолтное значение из реестра " + ValueName + " = " + _defaultValue.ToString() + " - " + error.Message;
                value = _defaultValue;
            }
            Log.Write(msg);
            return value;
        }


    }
}
