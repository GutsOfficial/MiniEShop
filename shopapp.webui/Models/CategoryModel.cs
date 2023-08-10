using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.entity;

public class CategoryModel
{

    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public List<Product> urunler { get; set; }
}
