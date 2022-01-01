using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.ViewModels
{
    public class AdvertiseViewModel
    {

        public ProductAdvertise _productAdvertise = null;
        public ProductAdvertise productAdvertise
        {
            get
            {
                if (_productAdvertise == null)
                    _productAdvertise = new ProductAdvertise();
                return _productAdvertise;
            }
            set
            {
                _productAdvertise = value;
            }
        }
        public Product _prod = null;
        public Product product
        {
            get
            {
                if (_prod == null)
                    _prod = new Product();
                return _prod;
            }
            set
            {
                _prod = value;
            }
        }
        public Supplier _supp = null;
        public Supplier supplier
        {
            get
            {
                if (_supp == null)
                    _supp = new Supplier();
                return _supp;
            }
            set
            {
                _supp = value;
            }
        }
        public List<ProductPhotoBank> _prodphoto = null;
        public List<ProductPhotoBank> photoBank
        {
            get
            {
                if (_prodphoto == null)
                    _prodphoto = new List<ProductPhotoBank>();
                return _prodphoto;
            }
            set
            {
                _prodphoto = value;
            }
        }
             public CategoryDetail _categoryDetail = null;
        public CategoryDetail categoryDetail
        {
            get
            {
                if (_categoryDetail == null)
                    _categoryDetail = new CategoryDetail();
                return _categoryDetail;
            }
            set
            {
                _categoryDetail = value;
            }
        }

        public Category _category = null;
        public Category category
        {
            get
            {
                if (_category == null)
                    _category = new Category();
                return _category;
            }
            set
            {
                _category = value;
            }
        }




    }
}
