using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Grid_MVVM_Filtering.Model;
using System.Windows.Input;

namespace Grid_MVVM_Filtering.ViewModel
{
    class DataViewModel : ObservableObject
    {
        public DataViewModel(int length)
        {
            CreateData(length);
        }

        private void CreateData(int length)
        {
            Data = new ObservableCollection<DataModel>();
            VisibleData = new ObservableCollection<DataModel>();
            ExcludedData = new ObservableCollection<DataModel>();
            for (int i = 0; i < length; i++)
            {
                Data.Add(new DataModel { Text = "Row" + i, Number = i });
            }
        }

        ObservableCollection<DataModel> _Data;
        ObservableCollection<DataModel> _VisibleData;
        ObservableCollection<DataModel> _ExcludedData;
        RelayCommand<DataModel> _AddExclusionCommand;
        RelayCommand<DataModel> _RemoveExclusionCommand;

        public ObservableCollection<DataModel> Data
        {
            get
            {
                return _Data;
            }
            set
            {
                _Data = value;
                OnPropertyChanged("Data");
            }
        }

        public ObservableCollection<DataModel> VisibleData
        {
            get
            {
                return _VisibleData;
            }
            set
            {
                _VisibleData = value;
                OnPropertyChanged("VisibleData");
            }
        }

        public ObservableCollection<DataModel> ExcludedData
        {
            get
            {
                return _ExcludedData;
            }
            set
            {
                _ExcludedData = value;
                OnPropertyChanged("ExcludedData");
            }
        }

        public ICommand AddExclusionCommand
        {
            get
            {
                if (_AddExclusionCommand == null)
                {
                    _AddExclusionCommand = new RelayCommand<DataModel>(param => this.AddExclusion(param), param => this.CanAddExclusion(param));
                }
                return _AddExclusionCommand;
            }
        }

        private void AddExclusion(DataModel param)
        {
            ExcludedData.Add(param);
            OnPropertyChanged("ExcludedData");
        }

        private bool CanAddExclusion(DataModel param)
        {
            return param != null && Data.Contains(param) && !ExcludedData.Contains(param);
        }

        public ICommand RemoveExclusionCommand
        {
            get
            {
                if (_RemoveExclusionCommand == null)
                {
                    _RemoveExclusionCommand = new RelayCommand<DataModel>(param => this.RemoveExclusion(param), param => this.CanRemoveExclusion(param));
                }
                return _RemoveExclusionCommand;
            }
        }

        private void RemoveExclusion(DataModel param)
        {
            ExcludedData.Remove(param);
            OnPropertyChanged("ExcludedData");
        }

        private bool CanRemoveExclusion(DataModel param)
        {
            return param != null && Data.Contains(param) && ExcludedData.Contains(param);
        }
    }
}
