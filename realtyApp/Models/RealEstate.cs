using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyApp.Models
{
    /// <summary>
    /// Объект недвижимости
    /// </summary>
    public class RealEstate
    {
        private string _title;
        private string _address;
        private decimal _price;

        /// <summary>
        /// Название
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        /// Цена
        /// </summary>
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }
    }
}
