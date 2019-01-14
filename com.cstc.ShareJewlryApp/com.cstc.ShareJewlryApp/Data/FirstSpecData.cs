using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.cstc.ShareJewlryApp.Data
{
    public class FirstSpecData : BindableObject
    {
        public string FirstSpecName { get; set; }
        ObservableCollection<FirstSpecDetailData> _主规格明细 = null;
        public ObservableCollection<FirstSpecDetailData> 主规格明细
        {
            get
            {
                if (_主规格明细 == null)
                    _主规格明细 = new ObservableCollection<FirstSpecDetailData>();
                return _主规格明细;
            }
            set
            {
                _主规格明细 = value;
                OnPropertyChanged("主规格明细");
            }
        }
    }
}
