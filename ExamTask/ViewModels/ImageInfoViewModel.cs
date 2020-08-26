using ExamTask.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExamTask.ViewModels
{
    public class ImageInfoViewModel : ObservableObject
    {
        public ObservableCollection<ImageInfoModel> Images { get; set; }
        private ImageInfoModel _selectedImage;
        private string _directoryName; 
        public ImageInfoModel SelectedImage
        {
            get => _selectedImage;
            set => OnPropertyChanged(ref _selectedImage, value);
        }
        public string DirectoryName
        {
            get => _directoryName;
            set => OnPropertyChanged(ref _directoryName, value);
        }
        public ImageInfoViewModel(string path)
        {
            Images = new ObservableCollection<ImageInfoModel>();
            foreach (string filePath in Directory.GetFiles(path))
            {
                Images.Add(new ImageInfoModel
                {
                    Path = filePath,
                    Name = Path.GetFileName(filePath),
                    Author = string.Empty,
                    Date = File.GetCreationTime(filePath).ToString("dd.MM.yy"),
                    Mark = null
                });
            }
            DirectoryName = Path.GetFileName(path);
            SelectedImage = Images.First();
        }
        public ImageInfoViewModel() { }
    }
}
