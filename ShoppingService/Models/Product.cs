using System;

namespace ShoppingService.Models
{
    /// <summary>
    /// Product class as defined in the requirements document
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Unique Key for item
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// Unique 6 character long number aka Stock Keeping Unit
        /// </summary>
        public string? Sku { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Product description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Product category
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Price of the product
        /// </summary>
        public decimal Price { get; set; }

        //public override bool Equals(object obj)
        //{
        //    if (obj is not Product other)
        //        return false;

        //    return Key == other.Key;
        //}

        //public override int GetHashCode()
        //{
        //    return Key.GetHashCode();
        //}
    }    
    
}