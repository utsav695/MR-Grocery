using System.Collections.Generic;

public class Product
{
    public string id;
    public string name;
    public List<string> tags;

    public Product(string id, string name, List<string> tags)
    {
        this.id = id;
        this.name = name;
        this.tags = tags;
    }
}
