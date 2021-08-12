using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Xpf;
using System.Collections.ObjectModel;
using System.Linq;

namespace CustomFilteringMVVM {
    public class DataModel {
        public string Text { get; set; }
        public int Number { get; set; }
    }

    public class MainViewModel : ViewModelBase {
        public ObservableCollection<DataModel> Data { get; set; }
        public ObservableCollection<DataModel> ExcludedData { get; set; }

        public DataModel CurrentVisibleItem { get; set; }
        public DataModel CurrentExcludedItem { get; set; }

        public MainViewModel() {
            Data = new ObservableCollection<DataModel>(Enumerable.Range(0, 10).Select(i => new DataModel() { Text = $"Row {i}", Number = i }));
            ExcludedData = new ObservableCollection<DataModel>();
        }

        [Command]
        public void AddExclusion() => ExcludedData.Add(CurrentVisibleItem);

        public bool CanAddExclusion() => CurrentVisibleItem != null && !ExcludedData.Contains(CurrentVisibleItem);

        [Command]
        public void RemoveExclusion() => ExcludedData.Remove(CurrentExcludedItem);

        public bool CanRemoveExclusion() => CurrentExcludedItem != null && ExcludedData.Contains(CurrentExcludedItem);

        [Command]
        public void FilterExclusions(RowFilterArgs args) {
            if(ExcludedData.Contains(Data[args.SourceIndex])) {
                args.Visible = false;
            }
        }
    }
}
