# Generic Data Access Layer using EF core


##Update the connection string per your SQL server insatnce
```
"ProductsDb": "server=localhost;Database=Products;User Id=sanjyot;Password=12345;"
```

## Example for extensions
```C#
public static class ProductsPredicates
{
  public static IIncludableQueryable<T, object> IncludeProductDetails<T>(this IQueryable<T> values) 
    where T : Product
  {
    return values.Include(pc => pc.ProductCatalog)
      .Include(pcf => pcf.ProductCustomFields)
      .Include(pl => pl.ProductLanguages);
  }
}
```
  
