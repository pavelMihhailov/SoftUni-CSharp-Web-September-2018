namespace SIS.HTTP.Extensions
{
    public class StringExtensions
    {
        public string Capitalize(string str)
        {
            string lowerCase = str.ToLower();
            string firstLetterUpper = lowerCase[0].ToString().ToUpper();
            string result = lowerCase.Remove(0, 1);
            result = result.Insert(0, firstLetterUpper);

            return result;
        }
    }
}
