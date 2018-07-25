using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;

using Xamarin.Forms;

namespace AppLogin
{
    public class VaccineModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int determine { get; set; } //month old
        public string status { get; set; } //Ture = ฉีดแล้ว False = ยังไม่ได้ฉีด
        public string BG { get; set; }
        public string TxtColor { get; set; }
        public string Img { get; set; }
    }

    public class vaccined
    {
        public List<int> profile = new List<int>();//{ 1, 2, 3, 4, 5,6,7, 8,9,10,11,12, 13,14,15,16, 17,18,19,20 };
        public List<int> test01 = new List<int>() { 1, 2, 3, 4, 5,7, 8, 13, 17 };

        public vaccined()
        {
            string user_id = LoginPage.myUser_id;
            WebClient wb = new WebClient();    
            string result = wb.UploadString("http://10.80.119.250:8082/vaccine/load_list_vaccine", user_id);
            Console.WriteLine(result);
            foreach(char w in result)
            {
                if (Char.IsNumber(w))
                {
                    string w_number = Char.ToString(w);
                    int number = int.Parse(w_number);
                    profile.Add(number);
                }
            }
        }

        //public class profilevacine
        //{
        //    public int NumberVaccine;
        //}
        public int getProfile(int i)
        {
            return profile[i];
        }
    }

    public class StoreVaccine
    {
        public List<VaccineModel> vac = new List<VaccineModel>();

        public StoreVaccine()
        {

            var mockItems = new List<VaccineModel>
            {
                //Guid.NewGuid().ToString()
                new VaccineModel { Id = 1, Name = "วัคซีนป้องกันวัณโรค (BCG)", Description="วัคซีนป้องกันวัณโรค",determine = 0 },
                new VaccineModel { Id = 2, Name = "วัคซีนป้องกันไวรัสตับอักเสบ บี (HBV) ครั้งที่ 1", Description="วัคซีนป้องกันไวรัสตับอักเสบ บี",determine = 0 },
                new VaccineModel { Id = 3, Name = "วัคซีนป้องกันไวรัสตับอักเสบ บี (HBV) ครั้งที่ 2", Description="วัคซีนป้องกันไวรัสตับอักเสบ บี ",determine = 2 },
                new VaccineModel { Id = 4, Name = "วัคซีนป้องกันคอตีบ บาดทะยัก ไอกรนชนิดทั้งเซลล์ (DTwP) ครั้งที่ 1", Description="วัคซีนป้องกันคอตีบ บาดทะยัก ไอกรนชนิดทั้งเซลล์ ",determine = 2 },
                new VaccineModel { Id = 5, Name = "วัคซีนป้องกันโรคโปลิโอชนิดกิน (OPV) ครั้งที่ 1", Description="วัคซีนป้องกันโรคโปลิโอชนิดกิน",determine=2 },
                new VaccineModel { Id = 6, Name = "วัคซีนป้องกันคอตีบ บาดทะยัก ไอกรนชนิดทั้งเซลล์ (DTwP) ครั้งที่ 2", Description="วัคซีนป้องกันคอตีบ บาดทะยัก ไอกรนชนิดทั้งเซลล์",determine=4 },
                new VaccineModel { Id = 7, Name = "วัคซีนป้องกันโรคโปลิโอชนิดกิน (OPV) ครั้งที่ 2", Description="วัคซีนป้องกันโรคโปลิโอชนิดกิน",determine=4 },
                new VaccineModel { Id = 8, Name = "วัคซีนป้องกันไวรัสตับอักเสบ บี (HBV) ครั้งที่ 3", Description="วัคซีนป้องกันไวรัสตับอักเสบ บี",determine=6},
                new VaccineModel { Id = 9, Name = "วัคซีนป้องกันคอตีบ บาดทะยัก ไอกรนชนิดทั้งเซลล์ (DTwP) ครั้งที่ 3", Description="วัคซีนป้องกันคอตีบ บาดทะยัก ไอกรนชนิดทั้งเซลล์",determine=6},
                new VaccineModel { Id = 10, Name = "วัคซีนป้องกันโรคโปลิโอชนิดกิน (OPV) ครั้งที่ 3", Description="วัคซีนป้องกันโรคโปลิโอชนิดกิน",determine=6 },
                new VaccineModel { Id = 11, Name = "วัคซีนป้องกันโรคหัด คางทูม หัดเยอรมัน (MMR) ครั้งที่ 1", Description="วัคซีนป้องกันโรคหัด คางทูม หัดเยอรมัน",determine=12},
                new VaccineModel{ Id = 12, Name = "วัคซีนป้องกันไวรัสสมองอักเสบ (JE) ครั้งที่ 1", Description="วัคซีนป้องกันไวรัสสมองอักเสบ",determine=12},
                new VaccineModel { Id = 13, Name = "วัคซีนป้องกันไวรัสสมองอักเสบ (JE) ครั้งที่ 2", Description="วัคซีนป้องกันไวรัสสมองอักเสบ",determine=13},
                new VaccineModel { Id = 14, Name = "วัคซีนป้องกันคอตีบ บาดทะยัก ไอกรนชนิดทั้งเซลล์ (DTwP) ครั้งที่ 4", Description="วัคซีนป้องกันคอตีบ บาดทะยัก ไอกรนชนิดทั้งเซลล์",determine=18},
                new VaccineModel { Id = 15, Name = "วัคซีนป้องกันโรคโปลิโอชนิดกิน (OPV) ครั้งที่ 4", Description="วัคซีนป้องกันโรคโปลิโอชนิดกิน",determine=18},
                new VaccineModel { Id = 16, Name = "วัคซีนป้องกันไวรัสสมองอักเสบ (JE) ครั้งที่ 3", Description="วัคซีนป้องกันไวรัสสมองอักเสบ",determine=24},
                new VaccineModel { Id = 17, Name = "วัคซีนป้องกันคอตีบ บาดทะยัก ไอกรนชนิดทั้งเซลล์ (DTwP) ครั้งที่ 5", Description="วัคซีนป้องกันคอตีบ บาดทะยัก ไอกรนชนิดทั้งเซลล์",determine=48},
                new VaccineModel { Id = 18, Name = "วัคซีนป้องกันโรคโปลิโอชนิดกิน (OPV) ครั้งที่ 5", Description="วัคซีนป้องกันโรคโปลิโอชนิดกิน",determine=48},
                new VaccineModel { Id = 19, Name = "วัคซีนป้องกันโรคหัด คางทูม หัดเยอรมัน (MMR) ครั้งที่ 2", Description="วัคซีนป้องกันโรคหัด คางทูม หัดเยอรมัน",determine=48},
                new VaccineModel { Id = 20, Name = "วัคซีนป้องกันคอตีบ บาดทะยัก ไอกรนชนิดทั้งเซลล์ชนิด Td", Description="ฉีดทุก 10 ปีหรือฉีดกรณีมีแผลลึกปากแผลแคบ เช่น ตะปูตำ ถูกสัตว์กัด",determine=144},
            };

            foreach (var item in mockItems)
            {
                item.status = "ยังไม่ได้ฉีด";
                //item.BG = "#F5A9A9";
                item.Img = "cancel1.png";
                item.TxtColor = "#D20E20";
                vac.Add(item);
            }
        }

        public async Task<VaccineModel> GetItemAsync(int id)
        {
            return await Task.FromResult(vac.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<VaccineModel>> GetItemsAsync(bool forceRefresh = false )
        {
            return await Task.FromResult(vac);
        }
    }

    public class BaseVaccineModel : INotifyPropertyChanged
    {
        public StoreVaccine store = new StoreVaccine();
        bool isBusy = false;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    class VaccineViewModel : BaseVaccineModel
    {
        public ObservableCollection<VaccineModel> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public vaccined pro = new vaccined();

        public VaccineViewModel()
        {
            Title = "Vaccine";
            Items = new ObservableCollection<VaccineModel>();

            //pro.SaveVaccine();
            //pro.LoadVaccine();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddItems();
        }

        public async void AddItems()
        {
            Items.Clear();
            var items = await store.GetItemsAsync(true);
            foreach (var item in items)
            {
                for (int i = 0; i < pro.profile.Count; i++)
                {
                    if (item.Id == pro.getProfile(i))
                    {
                        
                        item.status = "ฉีดแล้ว";
                    
                        item.Img = "checked.png";
                        item.TxtColor = "#0C8444";

                    }
                }
                Items.Add(item);
            }
        }


        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                //Items.Clear();
                //var items = await store.GetItemsAsync(true);
                //foreach (var item in items)
                //{
                //    for (int i = 0; i < pro.profile.Count; i++)
                //    {
                //        if (item.Id == pro.getProfile(i))
                //        {

                //            item.status = "ฉีดแล้ว";

                //            item.Img = "checked.png";
                //            item.TxtColor = "#0C8444";

                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
