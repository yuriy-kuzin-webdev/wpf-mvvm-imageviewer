using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamTask.Models
{
    public class ImageInfoModel : ObservableObject
    {
        private string _name;
        private string _date;
        private string _author;
        private string _mark;
        private string _path;

        public string Name
        {
            get => _name;
            set => OnPropertyChanged(ref _name, value);
        }
        public string Date
        {
            get => _date;
            set => OnPropertyChanged(ref _date, value);
        }
        public string Author
        {
            get => _author;
            set => OnPropertyChanged(ref _author, value);
        }
        public string Mark
        {
            get => _mark;
            set => OnPropertyChanged(ref _mark, value);
        }
        public string Path
        {
            get => _path;
            set => OnPropertyChanged(ref _path, value);
        }
    }
}
