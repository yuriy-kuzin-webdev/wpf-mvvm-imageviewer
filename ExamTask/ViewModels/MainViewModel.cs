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
    public class MainViewModel : ObservableObject
    {
        public ObservableCollection<ImageInfoViewModel> _directories;

        private ImageInfoViewModel _selectedDirectory;
        private ImageInfoModel _selectedImage;
        private string _markFilter = null;
        private int _getMaxValueForSlider;
        private int _index;

        #region Commands
        public ICommand FirstCommand { get; private set; }
        public ICommand LastCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand PreviousCommand { get; private set; }
        public ICommand TreeViewChangedCommand { get; private set; } // TreeView
        public ICommand SearchByNameCommand { get; private set; }
        #endregion Commands
        #region ObservableObjectProperties
        public string MarkFilter
        {
            get => _markFilter;
            set
            {
                FilterByMark(value);
                OnPropertyChanged(ref _markFilter, value);
            }
        }
        public ObservableCollection<ImageInfoViewModel> Directories 
        {
            get => _directories;
            set => OnPropertyChanged(ref _directories, value);
        }

        public int GetMaxValueForSlider
        {
            get => SelectedDirectory.Images.Count - 1;
            set => OnPropertyChanged(ref _getMaxValueForSlider, value);
        }
        public ImageInfoViewModel SelectedDirectory
        {
            get => _selectedDirectory;
            set => OnPropertyChanged(ref _selectedDirectory, value);
        }
        public ImageInfoModel SelectedImage
        {
            get => _selectedImage;
            set => OnPropertyChanged(ref _selectedImage, value);
        }
        public int Index
        {
            get => _index;
            set
            {
                OnPropertyChanged(ref _index, value);
                //Обновляем байндинги
                SelectedImage = 
                    SelectedDirectory.SelectedImage = 
                        SelectedDirectory.Images[Index];
            }
        }
        #endregion ObservableObjectProperties
        #region ctor
        public MainViewModel()
        {
            Directories = new ObservableCollection<ImageInfoViewModel>();
            string currentPath = Directory.GetCurrentDirectory() + "\\images";

            foreach (string path in Directory.GetDirectories(currentPath))
                Directories.Add(new ImageInfoViewModel(path));

            SelectedDirectory   = Directories.First();
            SelectedImage       = SelectedDirectory.SelectedImage;
            //
            FirstCommand = new RelayCommand<object>( 
                execute:    (_)     => Index = 0,
                canExecute: ()      => Index != 0);
            //
            LastCommand = new RelayCommand<object>( 
                execute:    (_)     => Index = GetMaxValueForSlider,
                canExecute: ()      => Index != GetMaxValueForSlider);
            //
            PreviousCommand = new RelayCommand<object>( 
                execute:    (_)     => --Index,
                canExecute: ()      => Index > 0);                 
            //
            NextCommand = new RelayCommand<object>(
                execute:    (_)     => ++Index,
                canExecute: ()      => Index < GetMaxValueForSlider);
            //
            TreeViewChangedCommand  = new RelayCommand<object>(TreeViewChanged);
            //
            SearchByNameCommand     = new RelayCommand<string>(SearchByName);
        }
        #endregion ctor
        #region helpers
        /// <summary>
        ///     Реагирует на изменение в елементе тривью
        /// </summary>
        /// <param name="obj">
        ///     Или директория или сам обьект
        /// </param>
        private void TreeViewChanged(object obj)
        {
            if(obj is ImageInfoViewModel)
            {
                //Directory change
                SelectedDirectory = obj as ImageInfoViewModel;
                Index = 0;
            }
            else
            {
                //Img Change
                ImageInfoViewModel directory = Directories.FirstOrDefault(dir => dir.Images.Contains(obj));
                if (directory != SelectedDirectory)
                    TreeViewChanged(directory);

                Index = directory.Images.IndexOf(obj as ImageInfoModel);
            }
            GetMaxValueForSlider = SelectedDirectory.Images.Count - 1;
        }
        /// <summary>
        ///     Фильтрует коллекцию по совпадающему марку
        /// </summary>
        /// <param name="mark"></param>
        private void FilterByMark(string mark)
        {
            ObservableCollection<ImageInfoViewModel> newList = new ObservableCollection<ImageInfoViewModel>();
            foreach (ImageInfoViewModel directory in Directories)
            {
                ImageInfoViewModel newDir = new ImageInfoViewModel() { DirectoryName = string.Copy(directory.DirectoryName) };
                newDir.Images = new ObservableCollection<ImageInfoModel>(directory.Images.Where(img => img.Mark == mark));
                newDir.SelectedImage = newDir.Images.FirstOrDefault();
                newList.Add(newDir);
            }
            Directories = newList;
            Index = 0;
        }
        /// <summary>
        ///     Ищет совпадения в названии картинки
        /// </summary>
        /// <param name="key"></param>
        private void SearchByName(string key) 
        {
            if(string.IsNullOrWhiteSpace(key))
                return;

            foreach(ImageInfoViewModel directory in Directories)
            {
                ImageInfoModel needle = directory.Images.FirstOrDefault(img => img.Name.Contains(key));
                if (needle != default)
                {
                    TreeViewChanged(needle);
                    break;
                }
            }
        }
        #endregion helpers
    }
}
