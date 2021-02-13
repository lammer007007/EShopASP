using System;

namespace EShopASP
{
    public class Product
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public float Price { get; set; }

        public string Description { get; set; }
    }

    public class ProductForm
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public float Price { get; set; }

        public string Description { get; set; }

        public string Clear { get; set; }
    }
}
