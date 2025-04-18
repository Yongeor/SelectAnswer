﻿using UnityEngine;

namespace Crosstales.Common.Util
{
   /// <summary>Wrapper for the PlayerPrefs.</summary>
   public static class CTPlayerPrefs
   {
      //TODO add getter and setter: Vector2 - 4, Quaternion
/*
#if UNITY_EDITOR
      private static readonly SerializableDictionary<string, string> content = new SerializableDictionary<string, string>();

      private static readonly string fileName = $"{Application.persistentDataPath}/crosstales.cfg";

      static CTPlayerPrefs()
      {
         if (System.IO.File.Exists(fileName))
            content = XmlHelper.DeserializeFromFile<SerializableDictionary<string, string>>(fileName);

         if (content == null)
            content = new SerializableDictionary<string, string>();
      }
#endif
*/
      /// <summary>Exists the key?</summary>
      /// <param name="key">Key for the PlayerPrefs.</param>
      /// <returns>Value for the key.</returns>
      public static bool HasKey(string key)
      {
         if (string.IsNullOrEmpty(key))
            throw new System.ArgumentNullException(nameof(key));

//#if UNITY_EDITOR
//         return content.ContainsKey(key);
//#else
         return PlayerPrefs.HasKey(key);
//#endif
      }

      /// <summary>Deletes all keys.</summary>
      public static void DeleteAll()
      {
//#if (UNITY_WSA || UNITY_WEBGL) && !UNITY_EDITOR
         PlayerPrefs.DeleteAll();
//#else
//         content.Clear();
//#endif
      }

      /// <summary>Delete the key.</summary>
      /// <param name="key">Key to delete in the PlayerPrefs.</param>
      public static void DeleteKey(string key)
      {
         if (string.IsNullOrEmpty(key))
            throw new System.ArgumentNullException(nameof(key));

//#if (UNITY_WSA || UNITY_WEBGL) && !UNITY_EDITOR
         PlayerPrefs.DeleteKey(key);
//#else
//         content.Remove(key);
//#endif
      }

      /// <summary>Saves all modifications.</summary>
      public static void Save()
      {
//#if (UNITY_WSA || UNITY_WEBGL) && !UNITY_EDITOR
         PlayerPrefs.Save();
/*
#else
         if (content != null && content.Count > 0)
         {
            XmlHelper.SerializeToFile(content, fileName);
         }
#endif
*/
      }


      #region Getter

      /// <summary>Allows to get a string from a key.</summary>
      /// <param name="key">Key for the PlayerPrefs.</param>
      /// <returns>Value for the key.</returns>
      public static string GetString(string key)
      {
         if (string.IsNullOrEmpty(key))
            throw new System.ArgumentNullException(nameof(key));

//#if (UNITY_WSA || UNITY_WEBGL) && !UNITY_EDITOR
         return PlayerPrefs.GetString(key);
//#else
//         return content[key];
//#endif
      }

      /// <summary>Allows to get a float from a key.</summary>
      /// <param name="key">Key for the PlayerPrefs.</param>
      /// <returns>Value for the key.</returns>
      public static float GetFloat(string key)
      {
         if (string.IsNullOrEmpty(key))
            throw new System.ArgumentNullException(nameof(key));

//#if (UNITY_WSA || UNITY_WEBGL) && !UNITY_EDITOR
         return PlayerPrefs.GetFloat(key);
//#else
//         float.TryParse(GetString(key), out float result);
//         return result;
//#endif
      }

      /// <summary>Allows to get an int from a key.</summary>
      /// <param name="key">Key for the PlayerPrefs.</param>
      /// <returns>Value for the key.</returns>
      public static int GetInt(string key)
      {
         if (string.IsNullOrEmpty(key))
            throw new System.ArgumentNullException(nameof(key));

//#if (UNITY_WSA || UNITY_WEBGL) && !UNITY_EDITOR
         return PlayerPrefs.GetInt(key);
//#else
//         int.TryParse(GetString(key), out int result);
//         return result;
//#endif
      }

      /// <summary>Allows to get a bool from a key.</summary>
      /// <param name="key">Key for the PlayerPrefs.</param>
      /// <returns>Value for the key.</returns>
      public static bool GetBool(string key)
      {
         return "true".CTEquals(GetString(key));
      }

      /// <summary>Allows to get a DateTime from a key.</summary>
      /// <param name="key">Key for the PlayerPrefs.</param>
      /// <returns>Value for the key.</returns>
      public static System.DateTime GetDate(string key)
      {
         System.DateTime.TryParseExact(GetString(key), "yyyyMMddHHmmsss", null, System.Globalization.DateTimeStyles.None, out System.DateTime result);

         return result;
      }

      #endregion


      #region Setter

      /// <summary>Allows to set a string for a key.</summary>
      /// <param name="key">Key for the PlayerPrefs.</param>
      /// <param name="value">Value for the PlayerPrefs.</param>
      public static void SetString(string key, string value)
      {
         if (string.IsNullOrEmpty(key))
            throw new System.ArgumentNullException(nameof(key));

//#if (UNITY_WSA || UNITY_WEBGL) && !UNITY_EDITOR
         PlayerPrefs.SetString(key, value);
/*         
#else
         if (content.ContainsKey(key))
         {
            content[key] = value;
         }
         else
         {
            content.Add(key, value);
         }
#endif
*/
      }

      /// <summary>Allows to set a float for a key.</summary>
      /// <param name="key">Key for the PlayerPrefs.</param>
      /// <param name="value">Value for the PlayerPrefs.</param>
      public static void SetFloat(string key, float value)
      {
//#if (UNITY_WSA || UNITY_WEBGL) && !UNITY_EDITOR
         if (string.IsNullOrEmpty(key))
            throw new System.ArgumentNullException(nameof(key));

         PlayerPrefs.SetFloat(key, value);
//#else
//         SetString(key, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
//#endif
      }

      /// <summary>Allows to set an int for a key.</summary>
      /// <param name="key">Key for the PlayerPrefs.</param>
      /// <param name="value">Value for the PlayerPrefs.</param>
      public static void SetInt(string key, int value)
      {
//#if (UNITY_WSA || UNITY_WEBGL) && !UNITY_EDITOR
         if (string.IsNullOrEmpty(key))
            throw new System.ArgumentNullException(nameof(key));

         PlayerPrefs.SetInt(key, value);
//#else
//         SetString(key, value.ToString());
//#endif
      }

      /// <summary>Allows to set a bool for a key.</summary>
      /// <param name="key">Key for the PlayerPrefs.</param>
      /// <param name="value">Value for the PlayerPrefs.</param>
      public static void SetBool(string key, bool value)
      {
         SetString(key, value ? "true" : "false");
      }

      /// <summary>Allows to set a DateTime for a key.</summary>
      /// <param name="key">Key for the PlayerPrefs.</param>
      /// <param name="value">Value for the PlayerPrefs.</param>
      public static void SetDate(string key, System.DateTime value)
      {
         if (value == null)
            throw new System.ArgumentNullException(nameof(value));

         SetString(key, value.ToString("yyyyMMddHHmmsss"));
      }

      #endregion
   }
}
// © 2015-2020 crosstales LLC (https://www.crosstales.com)