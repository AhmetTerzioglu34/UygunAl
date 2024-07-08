using Microsoft.Data.SqlClient;
using Project.BLL.Managers.Abstracts;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{
    //Abstract olmamasının ciddi bir nedeni var : Cünkü BaseManager'dan instance alınmasının mümkün olmasını istiyoruz...
    public class BaseManager<T> : IManager<T> where T : class, IEntity
    {
        protected readonly IRepository<T> _iRep;
        public BaseManager(IRepository<T> iRep)
        {
            _iRep = iRep;
        }
        void SaatEkle(T item)
        {
            item.CreatedDate = item.CreatedDate.AddHours(3);
        }

        public virtual string Add(T item)
        {
            SaatEkle(item);
            _iRep.Add(item);
            return "Ekleme basarılıdır";
        }

        public async Task AddAsync(T item)
        {
            SaatEkle(item);
            await _iRep.AddAsync(item);
        }
        bool ElemanKontrolEt(List<T> list)
        {
            if (list.Count > 10) return false;
            return true;
        }
        public string AddRange(List<T> list)
        {
            if (!ElemanKontrolEt(list)) return "Maksimum 10 veri ekleyebileceginiz icin işlem gerçekleştirilemedi ";
            _iRep.AddRange(list);
            return "Ekleme basarılı bir şekilde gerçekleştirildi";
        }

        public async Task<string> AddRangeAsync(List<T> list)
        {
            if (!ElemanKontrolEt(list)) return "Maksimum 10 veri ekleyebileceginiz icin işlem gerçekleştirilemedi ";
            await _iRep.AddRangeAsync(list);
            return "Ekleme basarılıdır";
        }

        public bool Any(Expression<Func<T, bool>> exp)
        {
            return _iRep.Any(exp);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> exp)
        {
            return await _iRep.AnyAsync(exp);
        }

        public void Delete(T item)
        {
            if (item.CreatedDate == default)
            {
                return;
            }
            _iRep.Delete(item);
        }

        public void DeleteRange(List<T> list)
        {
            _iRep.DeleteRange(list);
        }

        //public string Destroy(T item)
        //{
        //    if (item.Status == ENTITIES.Enums.DataStatus.Deleted)
        //    {
        //        _iRep.Destroy(item);
        //        return "Veri basarıyla yok edildi";
        //    }
        //    //throw new Exception("Silme durumunda hata ile karsılasıldı");
        //    return $"Veriyi silemezsiniz cünkü {item.ID} id'sine sahip veri pasif degil";
        //}


        public string Destroy(T item) //Aşağıdaki algoritmanın amacı kişi içerisinde ürünler olan kategoriyi silebilir
        {
            if (item.Status == ENTITIES.Enums.DataStatus.Deleted)
            {
                try
                {
                    _iRep.Destroy(item); // Veritabanından silme işlemi yapılır
                    return "Veri basarıyla yok edildi"; // Veri başarıyla silindi
                }

                catch (Exception ex)
                {
                    // Veritabanı hata mesajını yakalayarak özelleştirilmiş hata mesajı döndürülür

                    if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                    {
                        return "Veriler silinemiyor. Lütfen kategorideki ürünleri silin"; 
                    }
                }
               
            }
            return $"Veriyi silemezsiniz cünkü {item.ID} id'sine sahip veri pasif degil";
        }
            public string DestroyRange(List<T> list)
        {
            foreach (T item in list) return Destroy(item);

            return "Silme işleminde bir sorunla karsılasıldı lütfen veri durumunun pasif oldugundan emin olunuz";
        }

        public List<string> DestroyRangeWithText(List<T> list)
        {
            List<string> metinler = new List<string>();
            if (list == null || list.Count == 0)
            {
                metinler.Add("Listeye girilemedi");
                return metinler;
            }

            foreach (T item in list) metinler.Add(Destroy(item));
            return metinler;
        }

        public async Task<T> FindAsync(int id)
        {
            return await _iRep.FindAsync(id);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> exp)
        {
            return _iRep.FirstOrDefault(exp);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> exp)
        {
            return await _iRep.FirstOrDefaultAsync(exp);
        }

        public virtual List<T> GetActives()
        {
            return _iRep.GetActives();
        }

        public async Task<List<T>> GetActivesAsync()
        {
            return await _iRep.GetActivesAsync();
        }

        public List<T> GetAll()
        {
            return _iRep.GetAll();
        }

        public List<T> GetFirstDatas(int count)
        {
            return _iRep.GetFirstDatas(count);
        }

        public List<T> GetLastDatas(int count)
        {
            return _iRep.GetLastDatas(count);
        }

        public List<T> GetModifieds()
        {
            return _iRep.GetModifieds();
        }

        public List<T> GetPassives()
        {
            return _iRep.GetPassives();
        }

        public object Select(Expression<Func<T, object>> exp)
        {
            return _iRep.Select(exp);
        }

        public IQueryable<X> Select<X>(Expression<Func<T, X>> exp)
        {
            return _iRep.Select(exp);
        }

        public async Task UpdateAsync(T item)
        {
            await _iRep.UpdateAsync(item);
        }
        public void UpdateForJunction(T item , T originalEntity)
        {
            _iRep.UpdateForJunction(item, originalEntity);
        }
        public async Task UpdateRangeAsync(List<T> list)
        {
            await _iRep.UpdateRangeAsync(list);
        }

        public List<T> Where(Expression<Func<T, bool>> exp)
        {
            return _iRep.Where(exp);
        }
    }
}
