using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nova.SmartLCT.Interface;
using System.Collections.ObjectModel;
using Nova.LCT.GigabitSystem.Common;
using System.Collections.Specialized;

namespace Nova.SmartLCT.UI
{
    public class ScreenVirtualContainer_VM : NotificationST
    {
        public ObservableCollection<VirtualLightType> LightSequence
        {
            get { return _lightSequence; }
            set
            {
                _lightSequence = value;
                NotifyPropertyChanged(GetPropertyName(o => this.LightSequence));
            }
        }
        private ObservableCollection<VirtualLightType> _lightSequence = null;

        public ObservableCollection<VirtualLightType> PreviewFirstLightSequence
        {
            get { return _previewFirstLightSequence; }
            set
            {
                _previewFirstLightSequence = value;
                NotifyPropertyChanged(GetPropertyName(o => this.PreviewFirstLightSequence));
            }
        }
        private ObservableCollection<VirtualLightType> _previewFirstLightSequence = null;

        public ObservableCollection<VirtualLightType> PreviewSecondLightSequence
        {
            get { return _previewSecondLightSequence; }
            set
            {
                _previewSecondLightSequence = value;
                NotifyPropertyChanged(GetPropertyName(o => this.PreviewSecondLightSequence));

            }
        }
        private ObservableCollection<VirtualLightType> _previewSecondLightSequence = null;

        internal VirtualModeType PreviewFirstVirtualMode
        {
            get { return _previewFirstVirtualMode; }
            set
            {
                _previewFirstVirtualMode = value;
                NotifyPropertyChanged(GetPropertyName(o => this.PreviewFirstVirtualMode));
            }
        }
        private VirtualModeType _previewFirstVirtualMode = VirtualModeType.Led3;

        internal VirtualModeType PreviewSecondVirtualMode
        {
            get { return _previewSecondVirtualMode; }
            set
            {
                _previewSecondVirtualMode = value;
                NotifyPropertyChanged(GetPropertyName(o => this.PreviewSecondVirtualMode));
            }
        }
        private VirtualModeType _previewSecondVirtualMode = VirtualModeType.Led31;


        public ScreenVirtualContainer_VM()
        {
            PreviewFirstVirtualMode = VirtualModeType.Led3;
            PreviewSecondVirtualMode = VirtualModeType.Led31;

            ObservableCollection<VirtualLightType> sequence = new ObservableCollection<VirtualLightType>();
            sequence.Add(VirtualLightType.Red);
            sequence.Add(VirtualLightType.Green);
            sequence.Add(VirtualLightType.Blue);
            sequence.Add(VirtualLightType.VRed);
            LightSequence = sequence;
            LightSequence.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(LightSequence_CollectionChanged);
            sequence = new ObservableCollection<VirtualLightType>();
            sequence.Add(VirtualLightType.Red);
            sequence.Add(VirtualLightType.Green);
            sequence.Add(VirtualLightType.Blue);
            sequence.Add(VirtualLightType.VRed);
            PreviewFirstLightSequence = sequence;
            PreviewFirstLightSequence.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(LightSequence_CollectionChanged1);

            sequence = new ObservableCollection<VirtualLightType>();
            sequence.Add(VirtualLightType.VRed);
            sequence.Add(VirtualLightType.Blue);
            sequence.Add(VirtualLightType.Red);
            sequence.Add(VirtualLightType.Green);
            PreviewSecondLightSequence = sequence;
            PreviewSecondLightSequence.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(LightSequence_CollectionChanged2);

        }

        private void LightSequence_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }

        private void LightSequence_CollectionChanged1(object sender, NotifyCollectionChangedEventArgs e)
        {

        }

        private void LightSequence_CollectionChanged2(object sender, NotifyCollectionChangedEventArgs e)
        {

        }
    }
}
