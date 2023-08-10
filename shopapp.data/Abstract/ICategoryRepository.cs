using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.entity;

namespace shopapp.data.Abstract
{
    public interface ICategoryRepository:IRepository<Category>
    {
        Category GetByIdWithProduct(int categoryId);
        void  DeleteFromCategory(int categoryId, int productId);
    }
}