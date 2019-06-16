using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Extension
{
    public static class StringExtension
    {
        public static string RemoveVietNameseCharacterMark(this string input)
        {

            string[][] characters = new string[][]
            {
                new string[]{"u","ù","ú","ụ","ủ","ũ","ư","ứ","ừ","ự","ử","ữ"},
                new string[]{"e", "è", "é", "ẹ", "ẻ", "ẽ", "ê", "ế", "ề", "ệ", "ể", "ễ" },
                new string[]{"o","ò","ó","ọ","ỏ","õ","ô","ồ","ố","ộ","ổ","ỗ","ơ","ờ","ớ","ợ","ở","ỡ"},
                new string[]{"a", "à", "á", "ạ", "ả", "ã", "â", "ầ", "ấ", "ậ", "ẩ", "ẫ", "ă", "ằ", "ắ", "ặ", "ẳ", "ẵ", },
                new string[]{"i","ì","í","ị","ỉ","ĩ"},
                new string[]{"y", "ỳ", "ý", "ỵ", "ỷ", "ỹ" },
                new string[]{"d", "đ" }
            };


            StringBuilder sb = new StringBuilder(input);
            for (int i = 0; i < characters.Length; i++)
            {
                for (int j = 1; j < characters[i].Length; j++)
                {
                    sb.Replace(characters[i][j], characters[i][0]);
                    sb.Replace(characters[i][j].ToUpper(), characters[i][0].ToUpper());
                }
            }
            //sb.Replace(" ", string.Empty);
            return sb.ToString();
        }

        public static string LowerCaseFirst(this string input)
        {

            return (string.IsNullOrEmpty(input)) ? string.Empty : (char.ToLower(input[0]) + input.Substring(1));
        }

    }
}
