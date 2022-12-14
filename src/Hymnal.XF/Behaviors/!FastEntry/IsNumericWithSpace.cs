namespace Hymnal.XF.Behaviors.FastEntry
{
    public sealed class IsNumericWithSpace
    {
        public string ProcessIsNumericWithSpace(string entryText, string oldString, string newString)
        {
            var output = entryText;

            if (oldString != null)
            {
                if (newString.Length > oldString.Length)
                {
                    string hil;
                    if (oldString.Length > 0)
                        hil = entryText.Remove(0, entryText.Length - 1);
                    else
                        hil = newString;

                    var isNum = double.TryParse(hil, out _);
                    if (!isNum && hil != " ")
                    {
                        entryText = entryText.Remove(entryText.Length - 1); // remove last char
                        output = entryText;
                    }
                    else if (hil == " ")
                    {
                        if (oldString.Contains(" "))
                        {
                            entryText = entryText.Remove(entryText.Length - 1); // remove last char
                            output = entryText;
                        }
                    }
                }
                else
                {
                    var hil = entryText.Substring(entryText.Length - 1, 1);
                    var isNum = double.TryParse(hil, out _);

                    if (!isNum && hil != " ")
                    {
                        entryText = entryText.Remove(entryText.Length - 1); // remove last char
                        output = entryText;
                    }
                }
            }


            return output;
        }
    }
}
