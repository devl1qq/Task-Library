
namespace Task_Library
{
    public class CarRecord : ICarRecord
    {
        private DateTime _date;
        private string _brandName;
        private int _price;

        public string Date
        {
            get => _date.ToString("dd.MM.yyyy");
            set
            {
                if (DateTime.TryParseExact(value, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                {
                    _date = date;
                }
                else
                {
                    throw new InvalidDateFormatException("Invalid date format.");
                }
            }
        }

        public string BrandName
        {
            get => _brandName;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("BrandName cannot be empty.");
                }
                _brandName = value;
            }
        }

        public string Price
        {
            get => _price.ToString();
            set
            {
                if (int.TryParse(value, out int price) && price > 0)
                {
                    _price = price;
                }
                else
                {
                    throw new InvalidPriceFormatException("Invalid price format or value.");
                }
            }
        }
    }
}
