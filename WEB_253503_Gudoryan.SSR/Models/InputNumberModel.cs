using System.ComponentModel.DataAnnotations;

namespace WEB_253503_Gudoryan.SSR.Models
{
    public class InputNumberModel
    {

        [Range(1, 10, ErrorMessage = "Value must be between 1 and 10")]
        public int Value { get; set; }

    }
}
