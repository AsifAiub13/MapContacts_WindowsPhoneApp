using MapNotes.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MapNotes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddMapNote : Page
    {

        private bool isViewing = false;
        private MapNote mapNote;
        public AddMapNote()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Geopoint myPoint;
            if (e.Parameter == null)
            {
                isViewing = false;

                var locator = new Geolocator();
                locator.DesiredAccuracyInMeters = 50;

                var position = await locator.GetGeopositionAsync();
                myPoint = position.Coordinate.Point;


            }
            else
            {
                isViewing = true;
                mapNote = (MapNote)e.Parameter;
                titleTextBox.Text = mapNote.Title;
                noteTextBox.Text = mapNote.Note;
                addButton.Content = "Delete";

                var myPosition = new Windows.Devices.Geolocation.BasicGeoposition();
                myPosition.Latitude = mapNote.Latitude;
                myPosition.Longitude = mapNote.Longitude;

                myPoint = new Geopoint(myPosition);

            }

            await MyMap.TrySetViewAsync(myPoint, 16D);

        }

        private async void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (isViewing)
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog("Are u sure??");

                messageDialog.Commands.Add(new UICommand(
                    "Delete",
                    new UICommandInvokedHandler(this.CommandInvokedHandler)));
                messageDialog.Commands.Add(new UICommand(
                     "Cancel",
                     new UICommandInvokedHandler(this.CommandInvokedHandler)));
                messageDialog.DefaultCommandIndex = 0;
                messageDialog.CancelCommandIndex = 1;
                await messageDialog.ShowAsync();


            }
            else
            {

                MapNote newMapNote = new MapNote();
                newMapNote.Title = titleTextBox.Text;
                newMapNote.Note = noteTextBox.Text;
                newMapNote.Created = DateTime.Now;
                newMapNote.Latitude = MyMap.Center.Position.Latitude;
                newMapNote.Longitude = MyMap.Center.Position.Longitude;
                App.DataModel.AddMapnote(newMapNote);
                Frame.Navigate(typeof(MainPage));
            }
            Frame.Navigate(typeof(MainPage));
        }

        private void cancleButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        public void CommandInvokedHandler(IUICommand command)
        {
            if (command.Label == "Delete")
            {
                App.DataModel.DeleteMapnote(mapNote);
                Frame.Navigate(typeof(MainPage));


            }
            else
            {
                Frame.Navigate(typeof(MainPage));



            }


        }

    }
}
