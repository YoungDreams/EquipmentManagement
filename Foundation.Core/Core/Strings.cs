using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Foundation.Core
{
    public static class Strings
    {
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static string ToStringWhiteSpace(this object value)
        {
            if (value == null)
                return "";
            return value.ToString();
        }

        public static int? ToInt32(this string value)
        {
            if (value.IsNullOrWhiteSpace())
                return null;
            var result = 0;
            return int.TryParse(value, out result) ? (int?)result : null;
        }

        public static DateTime? ToDateTime(this string value)
        {
            if (value.IsNullOrWhiteSpace())
                return null;
            var result = DateTime.Now;
            return DateTime.TryParse(value, out result) ? (DateTime?)result : null;
        }

        public static decimal? ToDecimal(this string value)
        {
            if (value.IsNullOrWhiteSpace())
                return null;
            var result = 0.0m;
            return decimal.TryParse(value, out result) ? (decimal?)result : null;
        }
        
        public static List<T> SplitToList<T>(this string value, char separator) where T : IConvertible
        {
            if (string.IsNullOrEmpty(value)) return new List<T>();

            try
            {
                return value.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => (T)Convert.ChangeType(x, typeof(T))).ToList();
            }
            catch (Exception e)
            {
                return new List<T>();
            }
        }

        public static string ToLowerFirstChar(this string value)
        {
            if (!String.IsNullOrEmpty(value) && Char.IsUpper(value[0]))
            {
                value = Char.ToLower(value[0]) + value.Substring(1);
            }
            return value;
        }

        public static DateTime? ToBirthday(this string value)
        {
            if (value.Trim().Length == 15)
            {
                var year = int.Parse(value.Trim().Substring(6, 2));
                var month = int.Parse(value.Trim().Substring(8, 2));
                var day = int.Parse(value.Trim().Substring(10, 2));
                return new DateTime(year, month, day);
            }

            if (value.Trim().Length == 18)
            {
                var year = int.Parse(value.Trim().Substring(6, 4));
                var month = int.Parse(value.Trim().Substring(10, 2));
                var day = int.Parse(value.Trim().Substring(12, 2));
                return new DateTime(year, month, day);
            }

            return null;
        }

        public static string ToSex(this string value)
        {
            var sex = "";
            if (value.Trim().Length == 18)
            {
                sex = value.Trim().Substring(14, 3);
            }
            if (value.Trim().Length == 15)
            {
                sex = value.Trim().Substring(12, 3);
            }
            if (int.Parse(sex) % 2 == 0)
            {
                return "女";
            }
            else
            {
                return "男";
            }
        }
        public static int ToAge(this string value)
        {
            var year = 0;
            if (value.Trim().Length == 15)
            {
                 year = int.Parse(value.Trim().Substring(6, 2));   
            }
            if (value.Trim().Length == 18)
            {
                 year = int.Parse(value.Trim().Substring(6, 4)); 
            }
            return DateTime.Now.Year - year; ;
        }

        /// <summary>
        /// 验证15位身份证号
        /// </summary>
        /// <param name="idCard">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        public static bool CheckIdCard15(this string idCard)
        {
            long n = 0;
            if (long.TryParse(idCard, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idCard.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = idCard.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }
        /// <summary>
        /// 验证18位身份证号
        /// </summary>
        /// <param name="idCard">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        public static bool CheckIdCard18(this string idCard)
        {
            long n = 0;
            if (long.TryParse(idCard.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(idCard.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idCard.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = idCard.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = idCard.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != idCard.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }

        public static string ComputedMD5ByteToString(this string stringify)
        {
            var bytes = Encoding.UTF8.GetBytes(stringify);
            MD5 md5 = new MD5CryptoServiceProvider();
            var targetData = md5.ComputeHash(bytes);
            string byteToString = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byteToString += targetData[i].ToString("x2");
            }

            return byteToString;
        }
    }
}
