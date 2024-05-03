using System.Text.RegularExpressions;

namespace MyApp.Controllers.Serialize_and_Deserialize
{
    public class S_D
    {
        public S_D() { }

        public string Serialize(List<int> ids) 
        {
            string res = "";
            
            foreach (int id in ids) 
            {
                res += id.ToString() + "_";
            }
            return res;
        }

        public List<int> Deserialize(string ids)
        {
            List<int> numbers = new List<int>();

            string pattern = @"\d+";

            foreach (Match m in Regex.Matches(ids, pattern))
            {
                numbers.Add(int.Parse(m.Value));
            }

            // Вывод найденных чисел
            //foreach (int number in numbers)
            //{
            //    Console.WriteLine(number);
            //}

            return numbers;
        }

    }
}
