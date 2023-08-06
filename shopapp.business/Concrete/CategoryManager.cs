using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.business.Abstract;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.business.Concrete;

public class CategoryManager : ICategoryService
{
    private ICategoryRepository repo;
        public CategoryManager(ICategoryRepository _repo)
        {
            repo=_repo;
        }
    public void Create(Category entity)
    {
        repo.Create(entity);
    }

    public void Delete(Category entity)
    {
        repo.Delete(entity);
    }

    public List<Category> GetAll()
    {
      return  repo.GetAll();
    }

    public Category GetById(int id)
    {
        return repo.GetById(id);
    }

    public void Update(Category entity)
    {
        repo.Update(entity);
    }
}
